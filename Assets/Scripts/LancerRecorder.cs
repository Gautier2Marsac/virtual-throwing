using System.IO;
using System.Globalization;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LancerRecorder : MonoBehaviour
{
    public Transform targetTransform;

    private XRGrabInteractable grab;
    private Rigidbody rb;
    private string filePath;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        filePath = Path.Combine(Application.persistentDataPath, "Lancers.csv");

        if (!File.Exists(filePath))
        {
            string header = "RealTime,TimeSinceStart,MainUsed," +
                            "StartPosX,StartPosY,StartPosZ," +
                            "TargetPosX,TargetPosY,TargetPosZ," +
                            "VelocityX,VelocityY,VelocityZ," +
                            "DistanceLancer,DistanceFixe";
            File.WriteAllText(filePath, header + "\n");
        }
    }

    void OnEnable()
    {
        grab.selectExited.AddListener(LogThrow);
    }

    void OnDisable()
    {
        grab.selectExited.RemoveListener(LogThrow);
    }

    private void LogThrow(SelectExitEventArgs args)
    {
        if (rb == null || targetTransform == null)
        {
            Debug.LogWarning("Rigidbody ou cible manquant.");
            return;
        }

        var culture = CultureInfo.InvariantCulture;

        // Heure réelle (ex. 14:30:21)
        string realTime = System.DateTime.Now.ToString("HH:mm:ss");

        // Temps depuis début (ex. 22.35s)
        float timeSinceStart = Time.time;

        // Main utilisée
        string hand = args.interactorObject.transform.name.Contains("Left") ? "Left" : "Right";

        // Données physiques
        Vector3 velocity = rb.linearVelocity;
        Vector3 posStart = transform.position;
        Vector3 posTarget = targetTransform.position;

        float distanceLancer = Vector3.Distance(posStart, posTarget);
        float distanceFixe = Vector3.Distance(new Vector3(1f, 0.5f, -30f), new Vector3(1f, 0.5f, 4f));

        // Ligne CSV propre
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
        Debug.Log("OK Lancer enregistré dans le CSV.");
    }
}
