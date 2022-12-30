using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventsPanel : MonoBehaviour
{
    public static EventsPanel currentEventPanel;

    [SerializeField] TextMeshProUGUI authorText;
    [SerializeField] TextMeshProUGUI locationText;
    [SerializeField] TextMeshProUGUI dateText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI titleText;

    [SerializeField] TextMeshProUGUI readMoreButton;

    Animation animation;
    [SerializeField] ScrollRect scroll;
    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    public void InitializePanel(EventData eventData)
    {
        authorText.SetText(eventData.author);
        locationText.SetText(eventData.location);
        dateText.SetText(eventData.date);
        costText.SetText(eventData.cost);
        descriptionText.SetText(eventData.content);
        titleText.SetText(eventData.title);
    }

    public void Expand()
    {
        if (currentEventPanel != null)
            currentEventPanel.Shrink();

        animation.Play("ExpandEventPanel");
        readMoreButton.gameObject.SetActive(false);

        scroll.vertical = true;
    }

    public void Shrink()
    {
        animation.Play("ShrinkEventPanel");
        readMoreButton.gameObject.SetActive(true);

        scroll.vertical = false;
    }

    public void AddToFavs()
    {
        FavoritesManager.Instance.Add(gameObject);
    }
}
