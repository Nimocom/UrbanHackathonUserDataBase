using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Xml;
public class RatesManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI usdRateText;
    [SerializeField] TextMeshProUGUI eurRateText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRates("USD", "RUB", usdRateText));
        StartCoroutine(GetRates("EUR", "RUB", eurRateText));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GetRates(string from, string to, TextMeshProUGUI text)
    {
        using (UnityWebRequest www = new UnityWebRequest("https://api.apilayer.com/exchangerates_data/convert?to="+to+"&from="+from+"&amount=1", UnityWebRequest.kHttpVerbGET))
        {
            //UploadHandler uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonStr));
            www.SetRequestHeader("apikey", "ZrWLd50zlsGOPh5bGItsCfSk4EMtszQP");
   
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                //WeatherRoot weatherRoot = JsonUtility.FromJson<WeatherRoot>(www.downloadHandler.text);
                RatesRoot ratesRoot = JsonUtility.FromJson<RatesRoot>(www.downloadHandler.text);
                text.SetText(ratesRoot.info.rate.ToString("0.000"));

                //ZrWLd50zlsGOPh5bGItsCfSk4EMtszQP
            }
            else
                print(www.error);

        }
    }
}

[System.Serializable]
public class Info
{
    public int timestamp;
    public double rate;
}
[System.Serializable]
public class Query
{
    public string from;
    public string to;
    public int amount;
}
[System.Serializable]
public class RatesRoot
{
    public bool success;
    public Query query;
    public Info info;
    public string date;
    public double result;
}
