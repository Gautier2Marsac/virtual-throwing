using System.IO;
using System.Globalization;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LancerRecorder : MonoBehaviour
{
    // je dois pas oublir de glisser la reference dans l'inspecteur 
    public Transform targetTransform;

    private XRGrabInteractable grab;
    private Rigidbody rb;
    private string filePath;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        // afin d'afficher le chemin vers le fichier CSV qui va stocker les données
        filePath = Path.Combine(Application.persistentDataPath, "Lancers.csv");

        // si le fichier n'existe pas -> je le recrée en remplissant les colonnes 
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
        //declenchement de l'envent qu'oon on lache la balle 
        grab.selectExited.AddListener(LogThrow);
    }

    void OnDisable()
    {
        // pour se desabonner si l'objet et lacher 
        grab.selectExited.RemoveListener(LogThrow);
    }

    //a chaque fois ou on lance le ballon elle se lance 
    private void LogThrow(SelectExitEventArgs args)
    {
        if (rb == null || targetTransform == null)
        {
            Debug.LogWarning("Le rigidbody ou la cible n’est pas assigné.");
            return;
        }

        var culture = CultureInfo.InvariantCulture;

        // heure actuelle au moment du lancer elle nous sera utile pour synchro les tests
        string realTime = System.DateTime.Now.ToString("HH:mm:ss");

        //la duree depuis le lancer de la scene en cliquant sur play
        float timeSinceStart = Time.time;

        // detection de la main utilise 
        string hand = args.interactorObject.transform.name.Contains("Left") ? "Left" : "Right";

        // pos et vitesse de la balle lors ce qu'on le lache (xyz)
        Vector3 velocity = rb.linearVelocity;
        Vector3 posStart = transform.position;
        Vector3 posTarget = targetTransform.position;

        // distance entre la main et la cible c qu'une geometrie elle n'a pas d'impacte 
        float distanceLancer = Vector3.Distance(posStart, posTarget);

        // distance fixe entre la zone de lancer standard et la cible à modifier vu que j'ai ajoute deux autres cibles pour que le csv soit coherent 
        float distanceFixe = Vector3.Distance(new Vector3(1f, 0.5f, -30f), new Vector3(1f, 0.5f, 4f));

        //csv
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

        // envoyer la pos de depart a ImpactLogger pour qu’il enregistre la distance R au sol
        GetComponent<ImpactLogger>()?.SetStartPosition(posStart);
    }
}
