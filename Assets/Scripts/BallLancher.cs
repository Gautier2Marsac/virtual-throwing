// le script suivant n'est q'un test avec le clavier car je n'avais pas  acces au casque lors ce que je travaillais hors salle 
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
        //remettre la vitesse a 0 pour eviter l accumul des forces 
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // force vers l avant 
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }
}
