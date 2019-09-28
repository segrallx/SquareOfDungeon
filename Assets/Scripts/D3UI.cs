using UnityEngine;
using System;
using UnityEngine.UI;

public class D3UI : MonoBehaviour
{
    public void HiddenAll()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
    }

    public void ShowAll()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            child.gameObject.SetActive(true);
        }
    }

    public void DeleteAll()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}
