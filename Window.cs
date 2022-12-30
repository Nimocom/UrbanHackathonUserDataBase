using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Window : MonoBehaviour
{
    public static List<Window> allWindows = new List<Window>();

    //private void Awake()
    //{
    //    animation = GetComponent<Animation>();
    //}

    private void Awake()
    {
        allWindows.Add(this);
    }

    public void Show()
    {
        for (int i = 0; i < allWindows.Count; i++)
        {
            allWindows[i].gameObject.SetActive(false);
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
