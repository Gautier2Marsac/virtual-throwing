using System.IO;
using System.Globalization;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LancerRecorder : MonoBehaviour
{
    // référence vers la cible fixe (à glisser dans l’inspecteur)
    public Transform targetTransform;

    private XRGrabInteractable grab;
    private Rigidbody rb;
    private string filePath;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        // chemin vers le fichier CSV qui va stocker les données
        filePath = Path.Combine(Application.persistentDataPath, "Lancers.csv");

        // si le fichier n’existe pas encore, on écrit la première ligne (les noms de colonnes)
        if (!File.Exists(filePath))
        {
            string header = "RealTime,TimeSinceStart,MainUsed," +
                            "StartPosX,StartPosY,StartPosZ," +
                            "TargetPosX,TargetPosY,TargetPosZ," +
                            "VelocityX,VelocityY,VelocityZ," +
                            "DistanceLancer,DistanceFixe,DistanceImpact";
            File.WriteAllText(filePath, header + "\n");
        }
    }

    void OnEnable()
    {
        // on s’abonne à l’événement qui déclenche lorsqu’on relâche la balle
        grab.selectExited.AddListener(LogThrow);
    }

    void OnDisable()
    {
        // on se désabonne proprement si l’objet est désactivé
        grab.selectExited.RemoveListener(LogThrow);
    }

    // cette fonction s’exécute à chaque fois qu’on relâche la balle
    private void LogThrow(SelectExitEventArgs args)
    {
        if (rb == null || targetTransform == null)
        {
            Debug.LogWarning("Le rigidbody ou la cible n’est pas assigné.");
            return;
        }

        var culture = CultureInfo.InvariantCulture;

        // heure actuelle au moment du lancer (utile pour synchroniser les tests)
        string realTime = System.DateTime.Now.ToString("HH:mm:ss");

        // durée depuis le début de la scène (Time.time commence à 0 au lancement)
        float timeSinceStart = Time.time;

        // détection de la main utilisée (gauche ou droite)
        string hand = args.interactorObject.transform.name.Contains("Left") ? "Left" : "Right";

        // on récupère la position de départ et la vitesse de la balle au moment du lâcher
        Vector3 velocity = rb.linearVelocity;
        Vector3 posStart = transform.position;
        Vector3 posTarget = targetTransform.position;

        // distance entre la main et la cible (pure géométrie, pas le vrai impact)
        float distanceLancer = Vector3.Distance(posStart, posTarget);

        // distance fixe entre la zone de lancer standard et la cible
        float distanceFixe = Vector3.Distance(new Vector3(1f, 0.5f, -30f), new Vector3(1f, 0.5f, 4f));

        // ligne à écrire dans le fichier CSV, on n’ajoute pas encore distanceImpact ici
        string line =
            $"{realTime}," +
            $"{timeSinceStart.ToString("F2", culture)}," +
            $"{hand}," +
            $"{posStart.x.ToString("F2", culture)}," +
            $"{posStart.y.ToString("F2", culture)}," +
            $"{posStart.z.ToString("F2", culture)}," +
            $"{posTarget.x.ToString("F2", culture)}," +
            $"{posTarget.y.ToString("F2", culture)}," +
            $"{posTarget.z.ToString("F2", culture)}," +
            $"{velocity.x.ToString("F2", culture)}," +
            $"{velocity.y.ToString("F2", culture)}," +
            $"{velocity.z.ToString("F2", culture)}," +
            $"{distanceLancer.ToString("F2", culture)}," +
            $"{distanceFixe.ToString("F2", culture)}";

        File.AppendAllText(filePath, line + "\n");
        Debug.Log("Lancer enregistré dans le fichier CSV.");

        // on envoie la position de départ à ImpactLogger pour qu’il enregistre la distance réelle au sol
        GetComponent<ImpactLogger>()?.SetStartPosition(posStart);
    }
}
