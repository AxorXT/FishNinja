using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Effects")]
    public GameObject splashEffect;
    public AudioClip sliceSound;

    [Header("Slice")]
    public GameObject slicedPrefab;

    public void Slice(Vector3 direction)
    {
        // efectos (igual que antes)

        if (slicedPrefab != null)
        {
            GameObject sliced = Instantiate(slicedPrefab, transform.position, transform.rotation);

            Rigidbody[] parts = sliced.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rb in parts)
            {
                //Fuerza controlada
                Vector3 force = new Vector3(
                    direction.x * 2f,   // leve horizontal
                    2f,                 // peque�o impulso hacia arriba
                    0
                );

                rb.linearVelocity = force;

                // Rotaci�n ligera
                rb.AddTorque(new Vector3(0, 0, Random.Range(-5f, 5f)), ForceMode.Impulse);
            }

            Destroy(sliced, 2f);
        }

        Destroy(gameObject);
    }
}
