using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class InkEffectManager : MonoBehaviour
{
    public static InkEffectManager Instance;
    Tween inkTween;
    Tween resetTween;

    public Volume volume;

    LensDistortion lens;
    Vignette vignette;
    ColorAdjustments color;
    ChromaticAberration chroma;

    public float duration = 2f;

    void Awake()
    {
        Instance = this;

        volume.profile.TryGet(out lens);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out color);
        volume.profile.TryGet(out chroma);
    }

    public void ShowInk()
    {
        inkTween?.Kill();
        resetTween?.Kill();

        Vector2 randomCenter = new Vector2(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );

        vignette.center.value = randomCenter;

        Vector2 targetCenter = new Vector2(
            Random.value,
            Random.value
        );

        inkTween = DOTween.To(
            () => vignette.intensity.value,
            x => vignette.intensity.value = x,
            1f,
            0.2f
        );


        vignette.center.value = randomCenter;
        //activar efecto fuerte
        DOTween.To(() => lens.intensity.value, x => lens.intensity.value = x, -0.5f, 0.2f);
        DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0.7f, 0.2f);
        DOTween.To(() => color.saturation.value, x => color.saturation.value = x, -80f, 0.2f);
        DOTween.To(() => chroma.intensity.value, x => chroma.intensity.value = x, 1f, 0.2f);

        //regresar a normal
        resetTween = DOVirtual.DelayedCall(duration, () =>
        {
            DOTween.To(() => lens.intensity.value, x => lens.intensity.value = x, 0f, 0.5f);
            DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0f, 0.5f);
            DOTween.To(() => color.saturation.value, x => color.saturation.value = x, 0f, 0.5f);
            DOTween.To(() => chroma.intensity.value, x => chroma.intensity.value = x, 0f, 0.5f);
        });
    }
}