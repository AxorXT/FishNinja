using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerInputActions inputActions;

    [Header("Effects")]
    public TrailRenderer playerTrail;

    [Header("Menus")]
    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    [Header("HUD")]
    public GameObject hud;

    [Header("Score")]
    public TextMeshProUGUI finalScoreText;

    [Header("Lose Condition")]
    public int octopusCutLimit = 5;
    private int octopusCutCount = 0;

    bool isPaused = false;
    bool isGameOver = false;

    void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        Instance = this;

        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();

        inputActions.Gameplay.Press.performed += OnPress;
    }

    void OnDisable()
    {
        inputActions.Gameplay.Press.performed -= OnPress;
        inputActions.Disable();
    }

    void Start()
    {
        Time.timeScale = 0f;

        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        hud.SetActive(false);
        SetTrail(false);
    }

    void Update()
    {
        // PAUSA (ESC opcional, si quieres mantenerlo en PC)
        if (UnityEngine.InputSystem.Keyboard.current != null &&
            UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame &&
            !isGameOver)
        {
            TogglePause();
        }
    }

    //CLICK / TOUCH
    void OnPress(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isGameOver || isPaused)
            return;

        Debug.Log("Click o Touch detectado");
        // aquí va tu lógica de gameplay
    }

    // PLAY
    public void StartGame()
    {
        Time.timeScale = 1f;

        startMenu.SetActive(false);
        hud.SetActive(true);
        SetTrail(true);
        AnimatePanel(hud);
    }

    // PAUSA
    public void TogglePause()
    {
        isPaused = !isPaused;

        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        SetTrail(!isPaused);
        if (isPaused)
            AnimatePanel(pauseMenu);
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    // GAME OVER
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;

        hud.SetActive(false);
        gameOverMenu.SetActive(true);
        SetTrail(false);
        int finalScore = ScoreManager.Instance.score;
        finalScoreText.text = "SCORE: " + finalScore;
        
        AnimatePanel(gameOverMenu);
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void AnimatePanel(GameObject panel)
    {
        RectTransform rt = panel.GetComponent<RectTransform>();

        Debug.Log("ANTES TWEEN: " + rt.localScale);

        rt.DOKill();

        rt.localScale = Vector3.one;

        rt.DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutBack);
    }

    void SetTrail(bool active)
    {
        if (playerTrail == null) return;

        playerTrail.enabled = active;

        if (!active)
            playerTrail.Clear(); // evita que quede rastro viejo
    }

    public void CutOctopus()
    {
        octopusCutCount++;

        if (octopusCutCount >= octopusCutLimit)
        {
            GameOver();
        }
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // por si estás en pausa

        Application.Quit();
    }
}
