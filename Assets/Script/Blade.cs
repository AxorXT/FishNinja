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
    private Vector3 lastPosition;

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
        Vector3 previousPosition = transform.position;

        UpdatePosition();
        UpdateCutState();

        lastPosition = previousPosition;
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
            Fish fish = other.GetComponentInParent<Fish>();

            if (fish != null)
            {
                Vector3 direction = (transform.position - lastPosition).normalized;
                fish.Slice(direction);
            }
        }
    }
}
