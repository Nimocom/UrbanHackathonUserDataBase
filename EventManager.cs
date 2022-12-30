using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EventManager : MonoBehaviour
{
    [SerializeField] EventsPanel panelPrefab;
    [SerializeField] Transform contentRoot;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadEvents());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadEvents()
    {
        //while (true)
       // {

            for (int i = 0; i < contentRoot.childCount; i++)
            {
                Destroy(contentRoot.GetChild(i).gameObject);
            }

            using (UnityWebRequest webRequest = UnityWebRequest.Get("http://10.1.2.0:5000/getevents"))
            {
                yield return webRequest.SendWebRequest();

                var jsonString = webRequest.downloadHandler.text;

                EventsRoot eventsRoot = JsonUtility.FromJson<EventsRoot>(jsonString);

                for (int i = 0; i < eventsRoot.events.Count; i++)
                {
                    Instantiate(panelPrefab, contentRoot).InitializePanel(eventsRoot.events[i]);
                }
            }

            //yield return new WaitForSeconds(30);
       // }
    }

    private void OnEnable()
    {
        StartCoroutine(LoadEvents());
    }
}

[System.Serializable]
public class EventData
{
    public string title;
    public string author;
    public string location;
    public string date;
    public string cost;
    public string content;
}

[System.Serializable]
public class EventsRoot
{
    public List<EventData> events;
}
