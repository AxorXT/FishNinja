using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Effects")]
    public GameObject splashEffect;
    public AudioClip sliceSound;

    [Header("Slice")]
    public GameObject leftPrefab;
    public GameObject rightPrefab;

    [Header("Score")]
    public int points = 10;

    [Header("Type")]
    public bool isBadFish;
    public void Slice(Vector3 direction, Vector3 hitPoint)
    {
        if (isBadFish)
        {
            GameManager.Instance.CutOctopus();
            ScoreManager.Instance.ResetCombo();
            //castigo
            ScoreManager.Instance.AddPoints(-20);
            ScoreManager.Instance.ShowPoints(-20, hitPoint);

            InkEffectManager.Instance.ShowInk();
        }
        else
        {
            ScoreManager.Instance.AddCombo();

            int finalPoints = ScoreManager.Instance.AddPoints(points);
            ScoreManager.Instance.ShowPoints(finalPoints, hitPoint);
        }

        if (sliceSound != null)
        {
            AudioSource.PlayClipAtPoint(sliceSound, transform.position);
        }

        StartCoroutine(SliceRoutine(direction));
    }

    IEnumerator SliceRoutine(Vector3 direction)
    {
        // POP
        transform.DOScale(transform.localScale * 1.2f, 0.1f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.1f);

        // ocultar
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }

        // spawn mitades
        SpawnSliced(direction);

        Destroy(gameObject, 0.1f);
    }

    void SpawnSliced(Vector3 direction)
    {
        // Crear ambas mitades
        Vector3 offset = new Vector3(0.1f, 0, 0);

        GameObject left = Instantiate(leftPrefab, transform.position - offset, transform.rotation);
        GameObject right = Instantiate(rightPrefab, transform.position + offset, transform.rotation);

        if (splashEffect != null)
        {
            GameObject target = Random.value > 0.5f ? left : right;

            GameObject blood = Instantiate(
                splashEffect,
                target.transform.position,
                Quaternion.identity,
                target.transform
            );
        }

        Rigidbody rbLeft = left.GetComponent<Rigidbody>();
        Rigidbody rbRight = right.GetComponent<Rigidbody>();

        //Dirección base del corte
        Vector3 dir = direction.normalized;

        float separationForce = 3f;
        float upwardForce = 2.5f;

        //perpendicular (para separar lados)
        Vector3 perpendicular = Vector3.Cross(dir, Vector3.forward).normalized;

        //fuerzas
        if (rbLeft != null)
        {
            rbLeft.linearVelocity = (-perpendicular * separationForce) + (dir * 1.5f) + Vector3.up * upwardForce;

            rbLeft.AddTorque(Random.insideUnitSphere * 8f, ForceMode.Impulse);
        }

        if (rbRight != null)
        {
            rbRight.linearVelocity = (perpendicular * separationForce) + (dir * 1.5f) + Vector3.up * upwardForce;

            rbRight.AddTorque(Random.insideUnitSphere * 8f, ForceMode.Impulse);
        }

        Destroy(left, 2f);
        Destroy(right, 2f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Instantiate(splashEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
