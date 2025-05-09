using System.IO;
using UnityEngine;

public class LancerLogger : MonoBehaviour
{
    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "Lancers.csv");

        if (!File.Exists(filePath))
        {
            string header = "Time,MainUsed,StartPosition,TargetPosition,Velocity,Distance";
            File.WriteAllText(filePath, header + "\n");
            Debug.Log("Fichier CSV créé.\n Chemin : " + filePath);
        }
        else
        {
            Debug.Log("Fichier CSV déjà existant.\n Chemin : " + filePath);
        }
    }
}
