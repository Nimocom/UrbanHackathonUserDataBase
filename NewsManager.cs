using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NewsManager : MonoBehaviour
{
    public static NewsManager Instance;

    public NewsRoot newsRoot;

    [SerializeField] NewsPanel newsPanelPrefab;
    [SerializeField] Transform newsContentRoot;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(LoadNews());
    }

    IEnumerator LoadNews()
    {
        //while (true)
        //{

            for (int i = 0; i < newsContentRoot.childCount; i++)
            {
                Destroy(newsContentRoot.GetChild(i).gameObject);
            }

            using (UnityWebRequest webRequest = UnityWebRequest.Get("http://10.1.2.0:5000/getnews"))
            {
                yield return webRequest.SendWebRequest();

                var jsonString = webRequest.downloadHandler.text;
                //    string result = "{\"data\":[" + jsonString + "]}";
                print(jsonString);
                newsRoot = JsonUtility.FromJson<NewsRoot>(jsonString);
            }
          //  print()
            for (int i = 0; i < newsRoot.news.Count; i++)
            {
                Instantiate(newsPanelPrefab, newsContentRoot).InitializePanel(newsRoot.news[i]);
            }

            //yield return new WaitForSeconds(30);
        //}
    }

    private void OnEnable()
    {
        StartCoroutine(LoadNews());
    }
}

[System.Serializable]
public class NewData
{
    public string name;
    public string surname;
    public string date;
    public string url_photo;
    public string title;
    public string content;
}

[System.Serializable]
public class NewsRoot
{
    public List<NewData> news;
}