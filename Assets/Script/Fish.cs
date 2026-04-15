using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Effects")]
    public GameObject splashEffect;
    public AudioClip sliceSound;

    [Header("Slice")]
    public GameObject slicedPrefab;

    public void Slice()
    {
        //Partículas
        if (splashEffect != null)
        {
            Instantiate(splashEffect, transform.position, Quaternion.identity);
        }

        //Sonido
        if (sliceSound != null)
        {
            AudioSource.PlayClipAtPoint(sliceSound, transform.position);
        }

        //Crear pez cortado
        if (slicedPrefab != null)
        {
            GameObject sliced = Instantiate(slicedPrefab, transform.position, transform.rotation);

            Rigidbody[] parts = sliced.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rb in parts)
            {
                rb.AddForce(Random.onUnitSphere * 5f, ForceMode.Impulse);
            }
        }

        Destroy(gameObject);
    }
}
