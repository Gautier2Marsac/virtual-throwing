using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public float force = 10f; // Force du lancer
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Si l'utilisateur appuie sur Espace
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchBall();
        }
    }

    void LaunchBall()
    {
        // On remet la vitesse à zéro pour éviter d'accumuler les forces
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Appliquer une force vers l'avant (Z)
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }
}
