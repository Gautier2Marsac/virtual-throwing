using System.IO;
using System.Globalization;
using UnityEngine;

public class ImpactLogger : MonoBehaviour
{
    private Vector3 startPosition; // position de départ envoyée par le lancer
    private bool hasLogged = false;
    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "Lancers.csv");
    }

    // appelée depuis LancerRecorder juste après le lancer
    public void SetStartPosition(Vector3 pos)
    {
        startPosition = pos;
        hasLogged = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // on veut capturer seulement le premier impact avec le sol
        if (hasLogged) return;
        if (!collision.gameObject.CompareTag("Sol")) return;

        hasLogged = true;

        Vector3 impactPoint = collision.contacts[0].point;
        float distanceImpact = Vector3.Distance(startPosition, impactPoint);

        // lecture de toutes les lignes du fichier
        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length < 2)
        {
            Debug.LogWarning("Impossible d’ajouter la distance d’impact : pas de données disponibles.");
            return;
        }

        // modification de la dernière ligne
        var culture = CultureInfo.InvariantCulture;
        string lastLine = lines[lines.Length - 1];

        // si la ligne contient déjà la distance d’impact, on évite de la doubler
        if (lastLine.Contains("DistanceImpact")) return;

        lastLine += "," + distanceImpact.ToString("F2", culture);

        // réécriture complète du fichier
        lines[lines.Length - 1] = lastLine;
        File.WriteAllLines(filePath, lines);

        Debug.Log("Distance d’impact ajoutée : " + distanceImpact.ToString("F2", culture));
    }
}
