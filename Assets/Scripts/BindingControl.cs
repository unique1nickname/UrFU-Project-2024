using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class BindingControl : MonoBehaviour
{
    [SerializeField] public GameObject[] Grids;
    [HideInInspector] public int currentGrid;
    public GameObject Grid => Grids.Length > 0 ? Grids[currentGrid] : null;

    [SerializeField] public GameObject LevelPages;
    [SerializeField] public GameObject pagePrefab;
    [SerializeField] public GameObject answerCounter;
    [SerializeField] public GameObject inventorySlot;

    // [SerializeField] InputField inputItemNameField;
    
    // дл€ функции в инспекторе
    [HideInInspector] public string newItemName;

    [SerializeField] public GameObject[] itemsArray;
    //[HideInInspector] public GameObject[] itmesInAnswerGridArray; // так и не понадобилс€(

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
        if (LevelPages != null) 
        {     
            OnItemsArrayUpdate();

            OnAnswerGridUpdate();
        }
        // itmesInAnswerGridArray = new GameObject[Grid.transform.childCount];

    }

    public void OnItemsArrayUpdate()
    {
        bool[] isChecked = new bool[itemsArray.Length];

        for (int pageNum = 0; pageNum < LevelPages.transform.childCount; pageNum++)
        {
            Transform page = LevelPages.transform.GetChild(pageNum);
            int ItmesInsideCount = 0; // дл€ удалени€ пустой страницы, возможно подвергнетс€ изменени€м
            for (int slotNum = 0; slotNum < page.childCount; slotNum++)
            {
                Transform slot = page.transform.GetChild(slotNum);
                if (slot.childCount != 0) // удал€ет объекты, которых нет в массиве по тэгу 
                {
                    GameObject item = slot.GetChild(0).gameObject;

                    int itemIndex = Array.IndexOf(itemsArray, item);
                    if (itemIndex == -1) UnityEditor.EditorApplication.delayCall += () => { DestroyImmediate(item); };
                    else if (itemIndex < itemsArray.Length) isChecked[itemIndex] = true;

                    //int itemTag = -1;
                    //if (item.tag.Length >= 5) itemTag = int.Parse(item.tag.Substring(4, 1)) - 1;
                    //if (itemTag < itemsArray.Length && itemTag != -1)
                    //{
                    //    if (itemsArray[itemTag] == null)
                    //    {
                    //        UnityEditor.EditorApplication.delayCall += () => {DestroyImmediate(item);};
                    //    }
                    //    isChecked[itemTag] = true;
                    //}
                    //else UnityEditor.EditorApplication.delayCall += () => { DestroyImmediate(item); };
                }
                else ItmesInsideCount++; //дл€ удалени€ пустой страницы
                if (ItmesInsideCount == 6 && pageNum == LevelPages.transform.childCount - 1) UnityEditor.EditorApplication.delayCall += () => { DelatePage(); }; //дл€ удалени€ пустой страницы
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

                    //newItem.tag = string.Format("Slot{0}", index + 1); // тут поко€тс€ теги, не тревожить Press F to basically do nothing, they don't deserve any respect
                    //Debug.Log(string.Format("{0}", index + 1));

                    GameObject itemNameObject = new GameObject(itemName);
                    Text itemUIText = itemNameObject.AddComponent<Text>();
                    itemUIText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                    itemUIText.text = itemName;
                    itemUIText.raycastTarget = false;
                    itemUIText.fontSize = 24;
                    itemUIText.color = Color.black;
                    itemUIText.alignment = TextAnchor.MiddleCenter;
                    if (newItem.transform.childCount != 0)
                    {
                        foreach (Transform child in newItem.transform)
                        {
                            GameObject.DestroyImmediate(child.gameObject);
                        }
                    }
                    itemNameObject.transform.SetParent(newItem.transform);

                    newItem.name = itemName;
                    // замен€ем прифаб на созданный айтем, ибо а как ещЄ проводить сравнение по индексам в двух массивах??
                    itemsArray[index] = newItem;
                    return;
                }
            }
        }
        MakeNewPage();
        AddNewItem(index);
    }

    public void OnAnswerGridUpdate()
    {
        int slotCount = itemsArray.Count(s => s != null);
        int slotCountInGrids = GetSlotCountInGrids();
        //Debug.Log("slotCount = " + slotCount + "; Grid.childCount = " + Grid.transform.childCount);
        if (slotCount < slotCountInGrids)
        {
            for (int i = 0; i < slotCountInGrids; i++)
            {
                if (slotCount == 0)
                {
                    GameObject slot = GetSlotInGrid(i).gameObject;
                    UnityEditor.EditorApplication.delayCall += () => 
                    {
                        DestroyImmediate(slot);
                        answerCounter.GetComponent<TMP_Text>().text = "0/" + GetSlotCountInGrids();
                    };
                }
                else slotCount--;
            }
        }
        else if (slotCount > slotCountInGrids)
        {
            int count = slotCountInGrids;
            for (; count < slotCount; count++)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    GameObject slot = Instantiate(inventorySlot);
                    slot.transform.SetParent(Grid.transform);
                    slot.transform.localScale = Vector3.one;
                    // Debug.Log("hello there!");
                    answerCounter.GetComponent<TMP_Text>().text = "0/" + GetSlotCountInGrids();
                };
            }
        }
    }

    public int GetSlotCountInGrids()
    {
        int count = 0;
        foreach (GameObject grid in Grids)
        {
            count += grid.transform.childCount;
        }
        return count;
    }

    public Transform GetSlotInGrid(int index)
    {
        int count = 0;
        foreach (GameObject grid in Grids)
        {
            if (count + grid.transform.childCount > index) return grid.transform.GetChild(index - count);
            count += grid.transform.childCount;
        }
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // проверка прогресса выполнени€ задани€
        if (Input.GetMouseButtonUp(0) && Grid != null)
        {
            int answerCount = 0;
            int slotCountInGrids = GetSlotCountInGrids();
            TMP_Text answerCounterText = answerCounter.GetComponent<TMP_Text>();
            for (int slotNum = 0; slotNum < slotCountInGrids; slotNum++)
            {
                GameObject slot = GetSlotInGrid(slotNum).gameObject;
                if (slot.transform.childCount != 0)
                {
                    GameObject item = slot.transform.GetChild(0).gameObject;
                    //if (item.tag == slot.tag) answerCount++;  // остаток от сравнени€ по тегам
                    if (item == itemsArray[slotNum])
                    {
                        answerCount++;
                    }
                }
            }
            answerCounterText.text = answerCount.ToString() + "/" + slotCountInGrids;
        }
    }
}
