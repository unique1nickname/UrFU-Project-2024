using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class BindingControl : MonoBehaviour
{
    [SerializeField] public GameObject Grid;
    [SerializeField] public GameObject LevelPages;
    [SerializeField] public GameObject pagePrefab;
    [SerializeField] public GameObject answerCounter;


    [SerializeField] InputField inputItemNameField;
    
    // для функции в инспекторе
    [HideInInspector] public string newItemName;

    [SerializeField] public GameObject[] itemsArray;

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
            DestroyImmediate(LevelPages.transform.GetChild(LevelPages.transform.childCount - 1).gameObject);
        }
    }

    public void ClearAllSlots()
    {
        for (int pageNum = 0; pageNum < LevelPages.transform.childCount; pageNum++)
        {
            Transform page = LevelPages.transform.GetChild(pageNum);
            for (int slotNum = 0; slotNum < page.childCount; slotNum++)
            {
                Transform slot = page.transform.GetChild(slotNum);
                if (slot.childCount != 0)
                {
                    DestroyImmediate(slot.GetChild(0).gameObject);
                }
            }
        }
    }

    private void OnValidate()
    {
        if (LevelPages != null) OnItemsArrayUpdate();
    }

    public void OnItemsArrayUpdate()
    {
        bool[] isChecked = new bool[itemsArray.Length];

        for (int pageNum = 0; pageNum < LevelPages.transform.childCount; pageNum++)
        {
            Transform page = LevelPages.transform.GetChild(pageNum);
            int ItmesInsideCount = 0; // для удаления пустой страницы, возможно подвергнется изменениям
            for (int slotNum = 0; slotNum < page.childCount; slotNum++)
            {
                Transform slot = page.transform.GetChild(slotNum);
                if (slot.childCount != 0)
                {
                    GameObject item = slot.GetChild(0).gameObject;
                    int itemTag = -1;
                    if (item.tag.Length >= 5) itemTag = int.Parse(item.tag.Substring(4, 1)) - 1;
                    if (itemTag < itemsArray.Length && itemTag != -1)
                    {
                        if (itemsArray[itemTag] == null)
                        {
                            UnityEditor.EditorApplication.delayCall += () => {DestroyImmediate(item);};
                        }
                        isChecked[itemTag] = true;
                    }
                    else UnityEditor.EditorApplication.delayCall += () => { DestroyImmediate(item); };
                }
                else ItmesInsideCount++; //для удаления пустой страницы
                if (ItmesInsideCount == 6 && pageNum == LevelPages.transform.childCount - 1) UnityEditor.EditorApplication.delayCall += () => { DelatePage(); }; //для удаления пустой страницы
            }
        }
        for (int i = 0; i < itemsArray.Length; i++)
        {
            if (itemsArray[i] != null && isChecked[i] == false)
            {
                int current = i;
                UnityEditor.EditorApplication.delayCall += () => { AddNewItem(current); };
            }
        }
    }

    private void AddNewItem(int index)
    {
        string itemName = newItemName;
        for (int pageNum = 0; pageNum < LevelPages.transform.childCount; pageNum++)
        {
            Transform page = LevelPages.transform.GetChild(pageNum);
            for (int slotNum = 0; slotNum < page.childCount; slotNum++)
            {
                Transform slot = page.transform.GetChild(slotNum);
                if (slot.childCount == 0)
                {
                    GameObject newItem = Instantiate(itemsArray[index]);
                    newItem.transform.SetParent(slot);
                    newItem.transform.localScale = Vector3.one;

                    newItem.tag = string.Format("Slot{0}", index+1);
                    // Debug.Log(string.Format("{0}", index + 1));

                    GameObject itemNameObject = new GameObject(itemName);
                    Text itemUIText = itemNameObject.AddComponent<Text>();
                    itemUIText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                    itemUIText.text = itemName;
                    itemUIText.raycastTarget = false;
                    itemUIText.fontSize = 24;
                    itemUIText.color = Color.black;
                    itemUIText.alignment = TextAnchor.MiddleCenter;
                    itemNameObject.transform.SetParent(newItem.transform);
                    return;
                }
            }
        }
        MakeNewPage();
        AddNewItem(index);
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
        }
    }
}
