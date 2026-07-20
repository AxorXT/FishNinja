using UnityEngine;
using UnityEngine.InputSystem;

public class BladeVR : MonoBehaviour
{
    //[Header("References")]
    //public Camera cam;

    //[Header("Settings")]
    //public float distanceFromCamera = 5f;

    private PlayerInputActions input;
    private bool cutting;
    private Vector3 lastPosition;
    public float minCutSpeed = 0.01f;
    private float bladeSpeed;

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
        UpdateCutState();

        bladeSpeed = (transform.position - lastPosition).magnitude;

        lastPosition = transform.position;
    }

    /*void UpdatePosition()
    {
        Vector2 screenPos = input.Gameplay.PointerPosition.ReadValue<Vector2>();

        Vector3 screenPoint = new Vector3(screenPos.x, screenPos.y, distanceFromCamera);
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPoint);

        transform.position = worldPos;
    }*/

    void UpdateCutState()
    {
        cutting = true;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("TOQUE ALGO");

        if (!cutting || bladeSpeed < minCutSpeed) return;

        if (other.CompareTag("Fish"))
        {
            Fish fish = other.GetComponentInParent<Fish>();

            if (fish != null)
            {
                Vector3 direction = (transform.position - lastPosition).normalized;

                Vector3 hitPoint = other.ClosestPoint(transform.position);

                fish.Slice(direction, hitPoint);
            }
        }
    }
}
