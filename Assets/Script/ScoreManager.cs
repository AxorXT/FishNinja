using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score;
    public TextMeshProUGUI scoreText;
    public GameObject floatingTextPrefab;
    public Canvas canvas;

    void Awake()
    {
        Instance = this;
    }

    public void AddPoints(int amount)
    {
        score += amount;

        scoreText.text = score.ToString();

        //POP del score
        scoreText.transform.DOScale(1.3f, 0.1f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutBack);
    }

    public void ShowPoints(int amount, Vector3 worldPosition)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);

        // offset bonito
        screenPos += new Vector3(
            Random.Range(-25f, 25f),
            Random.Range(20f, 50f),
            0
        );

        GameObject txt = Instantiate(floatingTextPrefab, canvas.transform);
        txt.transform.position = screenPos;

        txt.GetComponent<FloatingText>().Show("+" + amount);
    }
}
