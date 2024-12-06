using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BindingControl : MonoBehaviour
{
    private bool OnSetting = false;
    [SerializeField] public GameObject Grid;
    [SerializeField] public GameObject LevelPages;
    [SerializeField] public GameObject pagePrefab;
    [SerializeField] public GameObject itemPrefab;
    [SerializeField] public GameObject inputItemNameWindow;
    [SerializeField] public GameObject answerCounter;


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
        inputItemNameWindow.SetActive(true);
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
                    newItem.transform.localScale = Vector3.one;
                    if (OnSetting) newItem.tag = "setting";
                    //GameObject itemNameObject = new GameObject(itemName);
                    //itemNameObject.name = itemName;
                    //itemNameObject.transform.SetParent(newItem.transform);
                    GameObject itemNameObject = new GameObject(itemName);
                    Text itemUIText = itemNameObject.AddComponent<Text>();
                    itemUIText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                    itemUIText.text = itemName;
                    itemUIText.raycastTarget = false;
                    itemUIText.fontSize = 24;
                    itemUIText.color = Color.black;
                    itemUIText.alignment = TextAnchor.MiddleCenter;
                    itemNameObject.transform.SetParent(newItem.transform);
                    inputItemNameWindow.SetActive(false);
                    return;
                }
            }
        }
        MakeNewPage();
        AddNewItem();
    }

    public void StartSettingItems()
    {
        // обходит пулл и ставит всем айтемам тег setting
        OnSetting = true;
        for (int pageNum = 0; pageNum < LevelPages.transform.childCount; pageNum++)
        {
            Transform page = LevelPages.transform.GetChild(pageNum);
            for (int slotNum = 0; slotNum < page.childCount; slotNum++)
            {
                Transform slot = page.transform.GetChild(slotNum);
                if (slot.childCount != 0)
                {
                    GameObject item = slot.GetChild(0).gameObject;
                    item.tag = "setting";
                }
            }
        }
    }

    public void EndSettingItems()
    {
        OnSetting = false;
        for (int pageNum = 0; pageNum < LevelPages.transform.childCount; pageNum++)
        {
            Transform page = LevelPages.transform.GetChild(pageNum);
            for (int slotNum = 0; slotNum < page.childCount; slotNum++)
            {
                Transform slot = page.transform.GetChild(slotNum);
                if (slot.childCount != 0)
                {
                    GameObject item = slot.GetChild(0).gameObject;
                    item.tag = "Untagged";
                }
            }
        }
        for (int slotNum = 0; slotNum < Grid.transform.childCount; slotNum++)
        {
            GameObject slot = Grid.transform.GetChild(slotNum).gameObject;
            if (slot.transform.childCount != 0)
            {
                GameObject item = slot.transform.GetChild(0).gameObject;
                item.tag = slot.tag;
            } 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // проверка прогресса выполнения задания
        if (Input.GetMouseButtonUp(0) && Grid != null)
        {
            int answerCount = 0;
            TMP_Text answerCounterText = answerCounter.GetComponent<TMP_Text>();
            for (int slotNum = 0; slotNum < Grid.transform.childCount; slotNum++)
            {
                GameObject slot = Grid.transform.GetChild(slotNum).gameObject;
                if (slot.transform.childCount != 0)
                {
                    GameObject item = slot.transform.GetChild(0).gameObject;
                    if (item.tag == slot.tag)
                    {
                        answerCount++;
                    }
                }
            }
            answerCounterText.text = answerCount.ToString() + "/6";
            if (OnSetting) answerCounterText.text += "\n(on setting)";
        }
    }
}
