/*
    Author: Kevin Heng
    Date: 30/1/2025
    Description: The EndPoint class is used to handle the functions when player reaches end point of level
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EndPoint : MonoBehaviour
{
    Database database;
    public string currentLevelName;

    public Volume globalVolume;
    public float volumeFadeDuration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FadeOutVolume();
            database.UpdateLevelComplete(GameManager.Instance.playerID, currentLevelName, true);
            Debug.Log("Level completed");
        }
    }

    public void FadeOutVolume()
    {
        StartCoroutine(FadeVolumeRoutine(1f, 0f)); // Fade effect out
    }

    private IEnumerator FadeVolumeRoutine(float startWeight, float endWeight)
    {
        float timer = 0;
        while (timer < volumeFadeDuration)
        {
            globalVolume.weight = Mathf.Lerp(startWeight, endWeight, timer / volumeFadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        globalVolume.weight = endWeight;
    }


    // Start is called before the first frame update
    void Start()
    {
        database = FindObjectOfType<Database>();
    }
}
