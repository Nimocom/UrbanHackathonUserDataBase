using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Net;

public class SidePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI userName;
    [SerializeField] RawImage userAvatar;

    [SerializeField] bool isShown;

    Animation animation;

    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoginManager.Instance.OnLoginAction += OnLogin;
       // StartCoroutine(Send());
    }

    //IEnumerator Send()
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("userName", "Hello");
    //    form.AddField("phone", "6384ca02bbb2883443c524f6");
    //    form.AddField("email", "6384ca02bbb2883443c524f6@gmail.com");
    //    form.AddField("password", "jksdkjsdjkjksd");

    //    //    "userName": "Hello", 
    //    //"phone": "6384ca02bbb2883443c524f6", 
    //    //"email": "6384ca02bbb2883443c524f6@gmail.com", 
    //    //"password ": "jksdkjsdjkjksd"
        
    //    string jsonStr = JsonUtility.ToJson(new LoginData() { email = "Asdasd", password = "asdsad", phone = "123213", userName = "adsdsa" });
    //    print(jsonStr);

    //    using (UnityWebRequest www = new UnityWebRequest("http://10.1.2.0:5000/adduser", UnityWebRequest.kHttpVerbPOST))
    //    {
    //        UploadHandler uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonStr));
    //        uploadHandler.contentType = "application/json";
    //        //www.SetRequestHeader("Content-Type", "application/json");
    //        www.uploadHandler = uploadHandler;
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            Debug.Log(www.responseCode);
    //        }
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchPanelState()
    {
        isShown = !isShown;
        animation.Play(isShown ? "Show" : "Hide");
    }

    public void OnLogin()
    {
        userName.SetText(LoginManager.activeUserData.data.surname[0].ToString() + ". " +  LoginManager.activeUserData.data.name);
        StartCoroutine(LoadAvatar());
    }

    IEnumerator LoadAvatar()
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture("https://raw.githubusercontent.com/Nimocom/UrbanHackathonUserDataBase/main/0.png"))
        {
            yield return webRequest.SendWebRequest();

            var texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;


            userAvatar.texture = texture;
        }
    }
}