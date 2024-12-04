using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollPages : MonoBehaviour
{
    [SerializeField] public GameObject LevelPages;
    [SerializeField] public GameObject pagePrefab;
    public void MakeNewPage()
    {
        if (pagePrefab != null)
        { 
            GameObject newPage = Instantiate(pagePrefab);
            newPage.transform.SetParent(LevelPages.transform);
            newPage.transform.localScale = LevelPages.transform.localScale;
        }
    }

    public void DelatePage()
    {
        if (LevelPages.transform.childCount > 1)
        {
            Destroy(LevelPages.transform.GetChild(LevelPages.transform.childCount - 1).gameObject);
            // LevelPages.transform.GetChild(LevelPages.transform.childCount - 1).gameObject.SetActive(false);
        }
    }
}
