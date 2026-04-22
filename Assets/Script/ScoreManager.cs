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

    [Header("Combo")]
    public int comboCount = 0;
    public float comboTimer = 0f;
    public float comboResetTime = 1.5f;

    public TextMeshProUGUI comboText;
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (comboCount > 0)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }
    }

    public int AddPoints(int amount)
    {
        int multiplier = Mathf.Max(1, comboCount);
        int finalPoints = amount * multiplier;

        score += finalPoints;

        scoreText.text = "" + score;

        scoreText.transform.DOKill();
        scoreText.transform.localScale = Vector3.one;

        scoreText.transform.DOScale(1.3f, 0.1f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutBack);

        return finalPoints;
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

    public void AddCombo()
    {
        comboCount++;
        comboTimer = comboResetTime;

        UpdateComboUI();
    }

    public void ResetCombo()
    {
        comboCount = 0;
        comboTimer = 0;

        comboText.text = "COMBO BREAK!";

        comboText.transform.DOKill();
        comboText.transform.localScale = Vector3.one;

        comboText.transform.DOScale(1.3f, 0.2f)
            .SetLoops(2, LoopType.Yoyo);

        DOVirtual.DelayedCall(0.5f, () => comboText.text = "");
    }

    void UpdateComboUI()
    {
        if (comboCount <= 5)
        {
            comboText.text = "";
            return;
        }

        comboText.text = "x" + comboCount + " COMBO!";

        comboText.transform.DOKill();
        comboText.transform.localScale = Vector3.zero;

        comboText.transform.DOScale(1.5f, 0.2f)
            .SetEase(Ease.OutBack);
    }
}
