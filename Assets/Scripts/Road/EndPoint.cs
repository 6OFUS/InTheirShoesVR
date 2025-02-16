/*
    Author: Kevin Heng
    Date: 30/1/2025
    Description: The EndPoint class is used to handle the functions when player reaches end point of level
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EndPoint : MonoBehaviour
{
    /// <summary>
    /// Reference Database class
    /// </summary>
    private Database database;
    /// <summary>
    /// Reference MessagesController class on Phone
    /// </summary>
    private MessagesController messagesController;

    /// <summary>
    /// Current level name
    /// </summary>
    public string currentLevelName;

    /// <summary>
    /// Global volume for blindness effect
    /// </summary>
    [Header("Global volume")]
    public Volume globalVolume;
    /// <summary>
    /// Duration for global volume transition effect
    /// </summary>
    public float volumeFadeDuration;

    /// <summary>
    /// Audio source to play level complete audio
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// Points awarded for the level
    /// </summary>
    private int levelPoints = 500;
    /// <summary>
    /// Number of times player hit by car
    /// </summary>
    public int hitCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FadeOutVolume();
            database.UpdateLevelComplete(GameManager.Instance.playerID, currentLevelName, true, "A3", DateTime.UtcNow.ToString("yyyy-MM-dd"), true);
            database.UpdatePlayerPoints(GameManager.Instance.playerID, CalculatePoints());
            audioSource.Play();
            StartCoroutine(messagesController.SendMultipleMessages(3, 2));
        }
    }

    /// <summary>
    /// Fade out global volume function
    /// </summary>
    public void FadeOutVolume()
    {
        StartCoroutine(FadeVolumeRoutine(1f, 0f)); // Fade effect out
    }

    /// <summary>
    /// Coroutine to fade global volume from weight 1 to 0
    /// </summary>
    /// <param name="startWeight">weight of global volume before transition</param>
    /// <param name="endWeight">weight of global volume after transition</param>
    /// <returns></returns>
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

    /// <summary>
    /// Calculate points awarded after the level
    /// </summary>
    /// <returns>Player's new total points</returns>
    private int CalculatePoints()
    {
        levelPoints -= 10 * hitCount;
        GameManager.Instance.playerPoints += levelPoints;
        return GameManager.Instance.playerPoints;
    }

    // Start is called before the first frame update
    void Start()
    {
        database = FindObjectOfType<Database>();
        audioSource = GetComponent<AudioSource>();
        messagesController = FindObjectOfType<MessagesController>();
        StartCoroutine(messagesController.SendMultipleMessages(0, 2));
    }
}
