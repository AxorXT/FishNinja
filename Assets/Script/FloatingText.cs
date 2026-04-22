using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Show(string message)
    {
        text.text = message;

        //empieza pequeño
        transform.localScale = Vector3.zero;

        //POP fuerte
        transform.DOScale(1.4f, 0.15f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                transform.DOScale(1f, 0.1f);
            });

        //subir
        transform.DOMoveY(transform.position.y + 80f, 1f)
            .SetEase(Ease.OutCubic);

        //fade
        text.DOFade(0, 1f);

        //pequeña rotación aleatoria
        transform.DORotate(new Vector3(0, 0, Random.Range(-10f, 10f)), 0.5f);

        Destroy(gameObject, 1f);
    }
}