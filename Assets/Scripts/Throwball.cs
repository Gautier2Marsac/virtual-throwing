using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    public float throwForce = 500f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //avertir Marc et Gautier de la touche que j'ai choisi 
        {
            rb.AddForce(transform.forward * throwForce);
        }
    }
}
