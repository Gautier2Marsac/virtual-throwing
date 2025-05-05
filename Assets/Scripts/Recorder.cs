using System.IO;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    public string playerID;
    public int    heightCM;
    public string mainHand;

    public Transform ball;
    public Transform target;

    private string csvPath;

    private void Awake()
    {
        csvPath = Path.Combine(Application.persistentDataPath, "results.csv");
        if (!File.Exists(csvPath))
            File.WriteAllText(csvPath, "PlayerID,HeightCM,Hand,Time,PositionX,PositionY,PositionZ,DistanceToTarget\n");
    }

    public void Record()
    {
        float time     = Time.time;
        Vector3 pos    = ball.position;
        float dist     = Vector3.Distance(ball.position, target.position);

        string line = $"{playerID},{heightCM},{mainHand},{time},{pos.x},{pos.y},{pos.z},{dist}\n";
        File.AppendAllText(csvPath, line);
        Debug.Log($"Recorded: {line}");
    }
}
