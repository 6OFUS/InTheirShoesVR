using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherController : MonoBehaviour
{
    private string apiKey = "4fe5f1e49fd8416b83163812251302"; 
    public string city = "Singapore";
    private string apiUrl;
    public bool isRain = false;

    void Start()
    {
        apiUrl = $"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={city}";
        StartCoroutine(UpdateWeather());
    }

    IEnumerator UpdateWeather()
    {
        while (true)
        {
            yield return StartCoroutine(GetWeather());
            yield return new WaitForSeconds(300);
        }
    }

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

    void ParseWeatherData(string json)
    {
        WeatherResponse weatherData = JsonUtility.FromJson<WeatherResponse>(json);

        if (weatherData != null && weatherData.current.condition != null)
        {
            string weatherCondition = weatherData.current.condition.text.ToLower();
            isRain = weatherCondition.Contains("rain") || weatherCondition.Contains("drizzle");

            Debug.Log("Weather Condition: " + weatherCondition + " | isRain: " + isRain);
        }
        else
        {
            Debug.LogError("Failed to parse weather data.");
        }
    }

    // JSON parsing
    [System.Serializable]
    private class ConditionInfo
    {
        public string text;  // condition
    }

    [System.Serializable]
    private class CurrentWeather
    {
        public ConditionInfo condition;
    }

    [System.Serializable]
    private class WeatherResponse
    {
        public CurrentWeather current;
    }
}
