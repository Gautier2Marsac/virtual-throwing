using System.IO;
using System.Globalization;
using UnityEngine;

public class ImpactLogger : MonoBehaviour
{
    private Vector3 startPosition; // pos de depart envoye par le lancer
    private bool hasLogged = false;
    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "Lancers.csv");
    }

    // appelee depuis LancerRecorder  apres le lancer
    public void SetStartPosition(Vector3 pos)
    {
        startPosition = pos;
        hasLogged = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // que le premier impact avec le sol vu que j'ai configuré le rebondissement de la balle pour etre realiste 
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

        //modif derniere ligne
        var culture = CultureInfo.InvariantCulture;
        string lastLine = lines[lines.Length - 1];

        // pour ne pas doubler la distance d'inpact si elle existe 
        if (lastLine.Contains("DistanceImpact")) return;

        lastLine += "," + distanceImpact.ToString("F2", culture);

        //reecriture complete de la data
        lines[lines.Length - 1] = lastLine;
        File.WriteAllLines(filePath, lines);

        Debug.Log("Distance d’impact ajoutée : " + distanceImpact.ToString("F2", culture));
    }
}
