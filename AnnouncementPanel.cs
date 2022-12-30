using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
public class AnnouncementPanel : MonoBehaviour
{
    [SerializeField] RawImage banner;

    [SerializeField] TextMeshProUGUI author;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI date;

    [SerializeField] TextMeshProUGUI content;

    public static AnnouncementPanel currentExpandedPanel;

    [SerializeField] bool isExpanded;

    Animation animation;

    [SerializeField] TextMeshProUGUI readMoreButton;

    [SerializeField] ScrollRect scroll;

    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    public void InitializePanel(Anon data)
    {
        if (data.url_photo.Length > 3)
            StartCoroutine(LoadBanner(data.url_photo));

        author.SetText(data.surname[0] + ". " + data.name);
        title.SetText(data.title);
        date.SetText(data.date);
        content.SetText(data.content);
    }

    public void Expand()
    {
        if (currentExpandedPanel != null)
            currentExpandedPanel.Shrink();

        animation.Play("ExpandAnnPanel");
        readMoreButton.gameObject.SetActive(false);

        isExpanded = true;

        currentExpandedPanel = this;
        scroll.vertical = true;
    }

    public void Shrink()
    {
        animation.Play("ShrinkAnnPanel");
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
