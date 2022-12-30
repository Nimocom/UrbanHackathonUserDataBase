using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NewsPanel : MonoBehaviour
{
    [SerializeField] RawImage banner;

    [SerializeField] TextMeshProUGUI author;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI date;

    [SerializeField] TextMeshProUGUI content;

    public static NewsPanel currentExpandedPanel;

    [SerializeField] bool isExpanded;

    Animation animation;

    [SerializeField] TextMeshProUGUI readMoreButton;

    [SerializeField] ScrollRect scroll;

    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    public void InitializePanel(NewData newsData)
    {
        StartCoroutine(LoadBanner(newsData.url_photo));

        author.SetText(newsData.surname[0] + ". " + newsData.name);
        title.SetText(newsData.title);
        date.SetText(newsData.date);
        content.SetText(newsData.content);
    }

    public void Expand()
    {
        if (currentExpandedPanel != null)
            currentExpandedPanel.Shrink();

        animation.Play("ExpandNewsPanel");
        readMoreButton.gameObject.SetActive(false);

        isExpanded = true;

        currentExpandedPanel = this;
        scroll.vertical = true;
    }

    public void Shrink()
    {
        animation.Play("ShrinkNewsPanel");
        readMoreButton.gameObject.SetActive(true);

        scroll.vertical = false;
    }

    IEnumerator LoadBanner(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return webRequest.SendWebRequest();

            var texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;


            banner.texture = texture;
        }
    }

    public void AddToFavs()
    {
        FavoritesManager.Instance.Add(gameObject);
    }
}
