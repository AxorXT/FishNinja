using UnityEngine;
using UnityEngine.InputSystem;

public class Blade : MonoBehaviour
{
    [Header("References")]
    public Camera cam;

    [Header("Settings")]
    public float distanceFromCamera = 5f;

    private PlayerInputActions input;
    private bool cutting;

    void Awake()
    {
        input = new PlayerInputActions();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        UpdatePosition();
        UpdateCutState();
    }

    void UpdatePosition()
    {
        Vector2 screenPos = input.Gameplay.PointerPosition.ReadValue<Vector2>();

        Vector3 screenPoint = new Vector3(screenPos.x, screenPos.y, distanceFromCamera);
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPoint);

        transform.position = worldPos;
    }

    void UpdateCutState()
    {
        if (input.Gameplay.Press.WasPressedThisFrame())
        {
            cutting = true;
        }
        else if (input.Gameplay.Press.WasReleasedThisFrame())
        {
            cutting = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!cutting) return;

        if (other.CompareTag("Fish"))
        {
            other.GetComponent<Fish>().Slice();
        }
    }
}
