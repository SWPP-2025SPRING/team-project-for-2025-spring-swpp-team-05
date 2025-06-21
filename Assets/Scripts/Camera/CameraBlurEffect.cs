using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraBlurEffect : MonoBehaviour
{
    public Material blurMaterial;

    [Header("Blur Settings")]
    public float maxBlurSize = 15f;
    public float maxWhiteIntensity = 0.4f;
    public float effectDuration = 5f;

    private bool isBlurring = false;
    private float t = 0f;

    public void TriggerMosquitoBlur()
    {
        if (!isBlurring)
        {
            StartCoroutine(ApplyMosquitoBlur());
        }
    }

    private IEnumerator ApplyMosquitoBlur()
    {
        isBlurring = true;
        t = 0f;

        while (t < effectDuration)
        {
            float progress = 1 - (t / effectDuration);
            blurMaterial.SetFloat("_BlurSize", maxBlurSize * progress);
            blurMaterial.SetFloat("_WhiteIntensity", maxWhiteIntensity * progress);

            t += Time.deltaTime;
            yield return null;
        }

        blurMaterial.SetFloat("_BlurSize", 0);
        blurMaterial.SetFloat("_WhiteIntensity", 0);
        isBlurring = false;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (isBlurring)
        {
            Graphics.Blit(src, dest, blurMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
