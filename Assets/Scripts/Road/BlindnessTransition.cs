using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BlindnessTransition : MonoBehaviour
{
    public Volume globalVolume;
    public float fadeDuration = 1f;

    
    public void FadeOutVolume()
    {
        StartCoroutine(FadeVolumeRoutine(1f, 0f)); // Fade effect out
    }
    
    private IEnumerator FadeVolumeRoutine(float startWeight, float endWeight)
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            globalVolume.weight = Mathf.Lerp(startWeight, endWeight, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        globalVolume.weight = endWeight;
    }
    


}
