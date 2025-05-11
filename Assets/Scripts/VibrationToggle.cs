using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VibrationToggle : MonoBehaviour
{
    [Header("Boutons pour activer/désactiver la vibration")]
    public InputActionReference buttonX;
    public InputActionReference buttonA;

    [Header("Références vers les interactor de chaque main")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor leftInteractor;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor rightInteractor;

    [Header("Paramètres des vibrations")]
    [Range(0f, 1f)] public float amplitude = 0.5f;
    public float duration = 0.1f;
    public float repeatInterval = 0.2f;

    private bool isLeftVibrating = false;
    private bool isRightVibrating = false;
    private float leftTimer = 0f;
    private float rightTimer = 0f;

    void OnEnable()
    {
        if (buttonX != null && buttonX.action != null)
            buttonX.action.performed += ToggleLeftVibration;

        if (buttonA != null && buttonA.action != null)
            buttonA.action.performed += ToggleRightVibration;
    }

    void OnDisable()
    {
        if (buttonX?.action != null)
            buttonX.action.performed -= ToggleLeftVibration;

        if (buttonA?.action != null)
            buttonA.action.performed -= ToggleRightVibration;
    }

    void Update()
    {
        if (isLeftVibrating && Time.time >= leftTimer && leftInteractor != null)
        {
            leftInteractor.SendHapticImpulse(amplitude, duration);
            leftTimer = Time.time + repeatInterval;
        }

        if (isRightVibrating && Time.time >= rightTimer && rightInteractor != null)
        {
            rightInteractor.SendHapticImpulse(amplitude, duration);
            rightTimer = Time.time + repeatInterval;
        }
    }

    private void ToggleLeftVibration(InputAction.CallbackContext context)
    {
        isLeftVibrating = !isLeftVibrating;
        Debug.Log("Vibration gauche " + (isLeftVibrating ? "activée" : "désactivée"));
    }

    private void ToggleRightVibration(InputAction.CallbackContext context)
    {
        isRightVibrating = !isRightVibrating;
        Debug.Log("Vibration droite " + (isRightVibrating ? "activée" : "désactivée"));
    }
}
