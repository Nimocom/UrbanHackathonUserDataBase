using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Linq;
using System;
using Assets.SimpleAndroidNotifications;
using System.Text;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageMeshPrefab;
    [SerializeField] Transform contentRoot;

    [SerializeField] TMP_InputField messageMesh;

    Coroutine getMessageCoroutine;

    [SerializeField] MessageRoot messageRoot;
    [SerializeField] ScrollRect scroll;
    int messagesCount;

    // Start is called before the first frame update
    void Start()
    {
        getMessageCoroutine = StartCoroutine(GetMessage());
    }

    // Update is called once per frame
    void Update()
    {
        //  scroll.value = Mathf.Lerp(scroll.value, 0f, 12f * Time.deltaTime);

    }

    public void UpdateMessages()
    {
        if (getMessageCoroutine != null)
            StopCoroutine(getMessageCoroutine);

        getMessageCoroutine = StartCoroutine(GetMessage());
    }

    void OnLoginAction()
    {

    }

    public IEnumerator GetMessage()
    {
        while (true)
        {
            using (UnityWebRequest www = new UnityWebRequest("http://10.1.2.0:5000/getmessage", UnityWebRequest.kHttpVerbGET))
            {
                www.downloadHandler = new DownloadHandlerBuffer();
                print("Sent");
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    print("Error");
                }
                else
                {
                    string json = www.downloadHandler.text;
                    //string result = "{\"data\":[" + json + "]}";
                    //print(result);

                    messageRoot = JsonUtility.FromJson<MessageRoot>(json);

                    for (int i = 0; i < contentRoot.childCount; i++)
                    {
                        Destroy(contentRoot.GetChild(i).gameObject);
                    }

                    print(json);

                    for (int i = 0; i < messageRoot.messages.Count; i++)
                    {
                        var messageElement = Instantiate(messageMeshPrefab, contentRoot);
                        // result = messageRoot.messages[i].surname + ". " + messageRoot.messages[i].name;

                        messageElement.SetText(messageRoot.messages[i].surname[0] + ". " + messageRoot.messages[i].name + ": " + messageRoot.messages[i].messages);
                        if (messageRoot.messages[i].email == LoginManager.activeUserData.data.email)
                        {
                            messageElement.alignment = TextAlignmentOptions.Right;
                        }
                    }
                    if (messageRoot.messages.Count > messagesCount)
                    {
                        yield return new WaitForEndOfFrame();
                        scroll.verticalNormalizedPosition = 0f;
                        messagesCount = messageRoot.messages.Count;
                    }
                }
            }

            yield return new WaitForSeconds(1);
        }
    }


    public void Send()
    {
        StartCoroutine(SendMessage());
    }

    public IEnumerator SendMessage()
    {
        string jsonStr = JsonUtility.ToJson(new MessageData() { date = DateTime.Now.ToString("dddd.MM.yyyy HH:mm"), email = LoginManager.activeUserData.data.email, messages = messageMesh.text, name = LoginManager.activeUserData.data.name, surname = LoginManager.activeUserData.data.surname });

        using (UnityWebRequest www = new UnityWebRequest("http://10.1.2.0:5000/sendmessage", UnityWebRequest.kHttpVerbPOST))
        {
            UploadHandler uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonStr));

            uploadHandler.contentType = "application/json; charset=utf-8";

            www.uploadHandler = uploadHandler;
            www.downloadHandler = new DownloadHandlerBuffer();
            print("Sent");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print("Error");
            }
            else
            {
                messageMesh.text = "";
                UpdateMessages();
            }
        }
    }
}

[System.Serializable]
public class MessageData
{
    public string name;
    public string surname;
    public string email;
    public string date;
    public string messages;
}

[System.Serializable]
public class MessageRoot
{
    public List<MessageData> messages;
}
