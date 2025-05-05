using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables; // pour XRGrabInteractable

public class BallHandler : MonoBehaviour
{
    [Header("Hands Transforms")]
    public Transform leftHandTransform;
    public Transform rightHandTransform;

    [Header("Input Actions")]
    public InputActionReference triggerReleaseLeft;
    public InputActionReference triggerReleaseRight;
    public InputActionReference buttonB;
    public InputActionReference buttonY;

    [Header("Recorder")]
    public Recorder recorder;  // nous permet de glisser notre  RecorderObject

    // Composants internes
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;
    private bool isHeld    = false;
    private bool isThrown  = false;
    private float timer    = 0f;
    private float returnDelay = 15f;

    private void Awake()
    {
        rb               = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        triggerReleaseLeft.action.performed  += OnTriggerReleased;
        triggerReleaseRight.action.performed += OnTriggerReleased;
        buttonB.action.performed            += OnButtonPressed;
        buttonY.action.performed            += OnButtonPressed;
    }

    private void OnDisable()
    {
        triggerReleaseLeft.action.performed  -= OnTriggerReleased;
        triggerReleaseRight.action.performed -= OnTriggerReleased;
        buttonB.action.performed            -= OnButtonPressed;
        buttonY.action.performed            -= OnButtonPressed;
    }

    private void Update()
    {
        if (isThrown)
        {
            timer += Time.deltaTime;
            if (timer >= returnDelay)
                ReturnToHand();
        }
    }

    private void OnTriggerReleased(InputAction.CallbackContext ctx)
    {
        if (!isHeld) return;

        isThrown = true;
        timer    = 0f;

        // Enregistre le lancer
        if (recorder != null)
            recorder.Record();
    }

    private void OnButtonPressed(InputAction.CallbackContext ctx)
    {
        if (isThrown)
            ReturnToHand();
    }

    private void ReturnToHand()
    {
        isThrown = false;
        timer    = 0f;

        // Choix de la main selon l'interactor
        Transform targetHand = rightHandTransform;
        if (grabInteractable.interactorsSelecting.Count > 0)
        {
            var intor = grabInteractable.interactorsSelecting[0];
            if (intor.transform.name.Contains("Left"))
                targetHand = leftHandTransform;
        }

        rb.linearVelocity        = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = targetHand.position;
        transform.rotation = targetHand.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            isHeld   = true;
            isThrown = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
            isHeld = false;
    }
}
