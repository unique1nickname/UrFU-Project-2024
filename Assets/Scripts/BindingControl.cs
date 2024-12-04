using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BindingControl : MonoBehaviour
{
    [SerializeField] public GameObject LevelPages;
    [SerializeField] public GameObject pagePrefab;
    [SerializeField] public GameObject itemPrefab;

    [SerializeField] InputField inputItemNameField;

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

    public void BeginMakingNewItem()
    {

    }

    public void AddNewItem()
    {
        string itemName = inputItemNameField.text;
        for (int pageNum = 0; pageNum < LevelPages.transform.childCount; pageNum++)
        {
            Transform page = LevelPages.transform.GetChild(pageNum);
            for (int slotNum = 0; slotNum < page.childCount; slotNum++)
            {
                Transform slot = page.transform.GetChild(slotNum);
                if (slot.childCount == 0)
                {
                    GameObject newItem = Instantiate(itemPrefab);
                    newItem.transform.SetParent(slot);
                    //GameObject itemNameObject = new GameObject(itemName);
                    //itemNameObject.name = itemName;
                    //itemNameObject.transform.SetParent(newItem.transform);
                    GameObject itemNameObject = new GameObject(itemName);
                    Text itemUIText = itemNameObject.AddComponent<Text>();
                    itemUIText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                    itemUIText.text = itemName;
                    itemUIText.raycastTarget = false;
                    itemUIText.fontSize = 24;
                    itemNameObject.transform.SetParent(newItem.transform);
                    return;
                }
            }
        }
        MakeNewPage();
        AddNewItem();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
