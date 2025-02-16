/*
* Author: Alfred Kang
* Date: 15/2/2025
* Description: Weather tracking script
* */
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherController : MonoBehaviour
{
    /// <summary>
    /// The API key used to authenticate requests to the weather API.
    /// </summary>
    private string apiKey = "4fe5f1e49fd8416b83163812251302";

    /// <summary>
    /// The city name for weather tracking. Default is set to Singapore.
    /// </summary>
    public string city = "Singapore";

    /// <summary>
    /// The URL endpoint for the weather API, dynamically built using the city and API key.
    /// </summary>
    private string apiUrl;

    /// <summary>
    /// Flag indicating whether it is raining. This is set based on the weather conditions.
    /// </summary>
    public bool isRain = false;

    /// <summary>
    /// The visual effect for rain that will be shown or hidden depending on weather conditions.
    /// </summary>
    public GameObject rainVFX;

    /// <summary>
    /// Called at the start of the scene. Initializes the weather API URL and starts updating the weather.
    /// </summary>
    void Start()
    {
        apiUrl = $"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={city}";
        StartCoroutine(UpdateWeather());
    }

    /// <summary>
    /// Repeatedly updates the weather information every 5 minutes (300 seconds).
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateWeather()
    {
        while (true)
        {
            yield return StartCoroutine(GetWeather());
            yield return new WaitForSeconds(300);
        }
    }

    /// <summary>
    /// Sends a request to the weather API and handles the response.
    /// </summary>
    /// <returns></returns>
    IEnumerator GetWeather()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                ParseWeatherData(jsonResponse);
            }
            else
            {
                Debug.LogError("Weather API Error: " + request.error);
            }
        }
    }

    /// <summary>
    /// Parses the weather data and determines if it is raining. Updates the rain visual effect.
    /// </summary>
    /// <param name="json">The raw JSON response from the weather API.</param>
    void ParseWeatherData(string json)
    {
        WeatherResponse weatherData = JsonUtility.FromJson<WeatherResponse>(json);

        if (weatherData != null && weatherData.current.condition != null)
        {
            string weatherCondition = weatherData.current.condition.text.ToLower();
            isRain = weatherCondition.Contains("rain") || weatherCondition.Contains("drizzle");

            Debug.Log("Weather Condition: " + weatherCondition + " | isRain: " + isRain);
            rainVFX.SetActive(isRain);
        }
        else
        {
            Debug.LogError("Failed to parse weather data.");
        }
    }

    /// <summary>
    /// Represents the condition information in the weather response.
    /// </summary>
    // JSON parsing
    [System.Serializable]
    private class ConditionInfo
    {
        public string text;  // condition
    }

    /// <summary>
    /// Represents the current weather data in the weather response.
    /// </summary>
    [System.Serializable]
    private class CurrentWeather
    {
        public ConditionInfo condition;
    }

    /// <summary>
    /// Represents the full weather response structure.
    /// </summary>
    [System.Serializable]
    private class WeatherResponse
    {
        public CurrentWeather current;
    }
}
