using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;

    float deltaTime;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.05f;

        float fps = 1.0f / deltaTime;

        fpsText.text = fps.ToString("F0") + " FPS";
    }
}
