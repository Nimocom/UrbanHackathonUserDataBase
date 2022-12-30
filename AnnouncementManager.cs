using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class AnnouncementManager : MonoBehaviour
{
    public static AnnouncementManager Instance;

    public AnnouncementRoot root;

    [SerializeField] AnnouncementPanel newsPanelPrefab;
    [SerializeField] Transform newsContentRoot;

    [SerializeField] TMP_InputField titleText;
    [SerializeField] TMP_InputField contentText;

    [SerializeField] Window annWindow;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(LoadNews());
    }

    private void OnEnable()
    {
        StartCoroutine(LoadNews());
    }

    public void CreateAnnouncement()
    {
        StartCoroutine(SendAnnouncementData());
        print("started");
    }

    IEnumerator SendAnnouncementData()
    {
        print("Cor");
        Anon anon = new Anon()
        {
            content = contentText.text,
            date = System.DateTime.Now.ToString("dddd.MM.yyyy HH:mm"),
            name = LoginManager.activeUserData.data.name,
            surname = LoginManager.activeUserData.data.surname,
            title = titleText.text,
            url_photo = ""
        };

        string jsonStr = JsonUtility.ToJson(anon);

        using (UnityWebRequest www = new UnityWebRequest("http://10.1.2.0:5000/addannouncements", UnityWebRequest.kHttpVerbPOST))
        {
            UploadHandler uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonStr));

            uploadHandler.contentType = "application/json";

            www.uploadHandler = uploadHandler;

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                annWindow.gameObject.SetActive(false);
                StartCoroutine(LoadNews());
            }

        }
    }

   

    IEnumerator LoadNews()
    {
       // while (true)
        //{

            for (int i = 0; i < newsContentRoot.childCount; i++)
            {
                Destroy(newsContentRoot.GetChild(i).gameObject);
            }

            using (UnityWebRequest webRequest = UnityWebRequest.Get("http://10.1.2.0:5000/getannouncements"))
            {
                yield return webRequest.SendWebRequest();

                var jsonString = webRequest.downloadHandler.text;
                //    string result = "{\"data\":[" + jsonString + "]}";
                print(jsonString);
                root = JsonUtility.FromJson<AnnouncementRoot>(jsonString);
            }
            print(root.anons[0].name);
            for (int i = 0; i < root.anons.Count; i++)
            {
                Instantiate(newsPanelPrefab, newsContentRoot).InitializePanel(root.anons[i]);
            }

           // yield return new WaitForSeconds(10);
       // }
    }
}

[System.Serializable]
public class Anon
{
    public string name;
    public string surname;
    public string date;
    public string url_photo;
    public string title;
    public string content;
}

[System.Serializable]
public class AnnouncementRoot
{
    public List<Anon> anons;
}
