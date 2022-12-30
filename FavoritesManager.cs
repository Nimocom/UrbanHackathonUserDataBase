using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FavoritesManager : MonoBehaviour
{
    public static FavoritesManager Instance;
    public static List<GameObject> favs;

    [SerializeField] Transform contentRoot;
    [SerializeField] TextMeshProUGUI note;
    private void Awake()
    {
        Instance = this;
        favs = new List<GameObject>();
    }

    public void Add(GameObject panel)
    {
        favs.Add(panel);
        note.gameObject.SetActive(false);
        Instantiate(panel, contentRoot);


    }

}
