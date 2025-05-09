using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BallHandler : MonoBehaviour
{
    public XRBaseInteractor leftHandInteractor;
    public XRBaseInteractor rightHandInteractor;

    public InputActionReference buttonB;
    public InputActionReference buttonY;

    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    private System.Action<InputAction.CallbackContext> bCallback;
    private System.Action<InputAction.CallbackContext> yCallback;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        bCallback = ctx => ReturnToHand(rightHandInteractor);
        yCallback = ctx => ReturnToHand(leftHandInteractor);

        buttonB.action.performed += bCallback;
        buttonY.action.performed += yCallback;
    }

    void OnDisable()
    {
        buttonB.action.performed -= bCallback;
        buttonY.action.performed -= yCallback;
    }

    void Start()
    {
        StartCoroutine(DelayedPlacement());
    }

    private IEnumerator DelayedPlacement()
    {
        yield return new WaitForSeconds(1f);
        ReturnToHand(rightHandInteractor);
    }

    public void ReturnToHand(XRBaseInteractor handInteractor)
    {
        Debug.Log("Retour de la balle vers la main");

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Positionner la balle manuellement dans la main
        transform.position = handInteractor.transform.position;
        transform.rotation = handInteractor.transform.rotation;

        // Libérer l'interactor précédent s'il existe
        var previousInteractor = grabInteractable.interactorsSelecting.FirstOrDefault();
        if (previousInteractor != null)
        {
            grabInteractable.interactionManager.SelectExit(previousInteractor as IXRSelectInteractor, grabInteractable);
        }

        // Forcer le grab avec l’interactor cible
        grabInteractable.interactionManager.SelectEnter(handInteractor as IXRSelectInteractor, grabInteractable);
    }
}
