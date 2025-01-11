using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckSolution : MonoBehaviour
{
    public GameObject[] inventorySlots;

    [SerializeField] public GameObject answerMark;
    [SerializeField] public Sprite Correct;
    [SerializeField] public Sprite Wrong;

    private void Start()
    {
        
    }

    public void Check()
    {
        BindingControl bc = transform.GetComponent<BindingControl>();
        inventorySlots = new GameObject[bc.GetSlotCountInGrids()];
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i] = bc.GetSlotInGrid(i).gameObject;
        }


        bool isAllRight = true;
        foreach (GameObject slot in inventorySlots)
        {
            //if (slot.transform.childCount > 0)
            //{
            //    if (slot.tag == slot.transform.GetChild(0).gameObject.tag)
            //    {
            //        Debug.Log("Yeeeeah!! Peace Peace!!!");
            //    }
            //    else
            //    {
            //        Debug.Log("That's wrong tho.");
            //        isAllRight = false;
            //    }
            //}
            //else isAllRight = false;
            if (slot.transform.childCount == 0) isAllRight = false;  
        }

        if (isAllRight)
        {
            answerMark.GetComponent<SpriteRenderer>().sprite = Correct;
        }
        else
        {
            answerMark.GetComponent<SpriteRenderer>().sprite = Wrong;
        }
    }
    
    public bool Check(GameObject grid)
    {
        Transform[] slots = new Transform[grid.transform.childCount];
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            slots[i] = grid.transform.GetChild(i);
        }
        foreach (Transform slot in slots)
        {
            if (slot.childCount == 0) return false;
        }
        return true;
    }
}
