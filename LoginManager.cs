using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Linq;
using System;
using Assets.SimpleAndroidNotifications;
using System.Text;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance;

    public Action OnLoginAction;

    public static Root activeUserData;

    [SerializeField] GameObject loginPanel;


    [SerializeField] TMP_InputField Login;
    [SerializeField] TMP_InputField Password;
    [SerializeField] TMP_InputField NewLogin;
    [SerializeField] TMP_InputField NewPassword;
    [SerializeField] TMP_InputField Name;
    [SerializeField] TMP_InputField Surname;
    [SerializeField] TMP_InputField Address;
    [SerializeField] TMP_InputField Phone;

    [SerializeField] GameObject signInPage;
    [SerializeField] GameObject signUpPage;
    [SerializeField] GameObject infoPage;

    [SerializeField] GameObject signInButton;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject readyButton;

    [SerializeField] GameObject annAddButton;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //  StartCoroutine(LoadUserDataBase());
        NotificationManager.Send(TimeSpan.Zero, "TestTitle", "Mesage", Color.red);
    }


    //IEnumerator LoadUserDataBase()
    //{
    //    using (UnityWebRequest webRequest = UnityWebRequest.Get("https://raw.githubusercontent.com/Nimocom/UrbanHackathonUserDataBase/main/TestData.json"))
    //    {
    //        yield return webRequest.SendWebRequest();

    //        var jsonString = webRequest.downloadHandler.text;

    //        dataRoot = JsonUtility.FromJson<UserDataRoot>(jsonString);
    //    }
    //}

    public void SignIn()
    {
        if (Login.text.Length > 4)
            if (Password.text.Length > 6)
                StartCoroutine(LogIn());
    }

    public IEnumerator LogIn()
    {
        string jsonStr = JsonUtility.ToJson(new LoginData() { email = Login.text, password = Password.text });

        using (UnityWebRequest www = new UnityWebRequest("http://10.1.2.0:5000/login", UnityWebRequest.kHttpVerbPOST))
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
                // string json = Encoding.UTF8.GetString(www.downloadHandler.data);
                string json = www.downloadHandler.text;

               // print(json);
                if (json.Contains("-erorr"))
                    yield break;

                json = json.Replace(@"\", string.Empty);
                string result = json.Substring(1, json.Length - 3);
                result = "{\"data\":" + result + "}"; 


                activeUserData = JsonUtility.FromJson<Root>(result);

                loginPanel.SetActive(false);

                if (activeUserData.data.role_id == 0)
                    annAddButton.SetActive(false);
                else if (activeUserData.data.role_id == 1)
                    annAddButton.SetActive(true);

                    OnLoginAction?.Invoke();
            }
        }
    }

    public void CreateAccount()
    {
        signInPage.SetActive(false);
        signUpPage.SetActive(true);
    }

    public void GoToNextStep()
    {
        if (NewLogin.text.Length > 4)
            if (NewPassword.text.Length > 6)
                StartCoroutine(SignUp());
    }

    public IEnumerator SignUp()
    {
        string jsonStr = JsonUtility.ToJson(new LoginData() { email = NewLogin.text, password = NewPassword.text});

        using (UnityWebRequest www = new UnityWebRequest("http://10.1.2.0:5000/adduser", UnityWebRequest.kHttpVerbPOST))
        {
            UploadHandler uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonStr));

            uploadHandler.contentType = "application/json";

            www.uploadHandler = uploadHandler;
            print("Sent");
            yield return www.SendWebRequest();
      
            if (www.result != UnityWebRequest.Result.Success)
            {
                print("Error");
            }
            else
            {
                signUpPage.SetActive(false);
                infoPage.SetActive(true);

                print("Success!");
            }
        }
    }

    public void Finish()
    {
        if (Name.text.Length > 0)
            if (Surname.text.Length > 0)
                if (Address.text.Length > 0)
                    if (Phone.text.Length > 0)
                        StartCoroutine(FillInfo());
    }

    public IEnumerator FillInfo()
    {
        string jsonStr = JsonUtility.ToJson(new ProfileData() {email = NewLogin.text, password = NewPassword.text,  name = Name.text, surname = Surname.text, address = Address.text, phone = Phone.text });

        using (UnityWebRequest www = new UnityWebRequest("http://10.1.2.0:5000/updateuser", UnityWebRequest.kHttpVerbPOST))
        {
            UploadHandler uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonStr));

            uploadHandler.contentType = "application/json";

            www.uploadHandler = uploadHandler;

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {

                infoPage.SetActive(false);
                OnLoginAction?.Invoke();
            }

        }
    }

}

[System.Serializable]
public class ProfileData
{
    public string email;
    public string password;
    public string name;
    public string surname;
    public string address;
    public string phone;
}


[System.Serializable]
public class LoginData
{
    public string email;
    public string password;
}

[System.Serializable]
public class Data
{
    public string email;
    public string password;
    public string name;
    public string surname;
    public string address;
    public string phone;
    public int role_id;
}


[System.Serializable]
public class Root
{
    public Data data;
}