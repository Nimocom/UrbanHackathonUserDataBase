using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class WeatherManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tempText;
    [SerializeField] TextMeshProUGUI windSpeedText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetWeather());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GetWeather()
    {

        using (UnityWebRequest www = new UnityWebRequest("https://api.open-meteo.com/v1/forecast?latitude=43.3188&longitude=45.6865&current_weather=true", UnityWebRequest.kHttpVerbGET))
        {
            //UploadHandler uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonStr));
            //www.SetRequestHeader("X-Gismeteo-Token", "56b30cb255.3443075");
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                WeatherRoot weatherRoot = JsonUtility.FromJson<WeatherRoot>(www.downloadHandler.text);
                tempText.SetText(weatherRoot.current_weather.temperature.ToString() + "C");
                windSpeedText.SetText("Ветер " + weatherRoot.current_weather.windspeed.ToString() + " м/с.");
            }
            else
                print(www.error);

        }
    }
}

[System.Serializable]
public class CurrentWeather
{
    public double temperature;
    public double windspeed;
    public double winddirection;
    public int weathercode;
    public string time;
}

[System.Serializable]
public class WeatherRoot
{
    public double latitude;
    public double longitude;
    public double generationtime_ms;
    public int utc_offset_seconds;
    public string timezone;
    public string timezone_abbreviation;
    public double elevation;
    public CurrentWeather current_weather;
}
