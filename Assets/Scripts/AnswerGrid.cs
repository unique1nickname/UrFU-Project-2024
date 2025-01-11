using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerGrid : MonoBehaviour
{
    [SerializeField] public GameObject PagesScrollView;
    [HideInInspector] public bool isSolved = false; // для проверки на то, решено ли уже это поле ответов

    public int GetPreviousSlotsNumber()
    {
        //Transform canvas = PagesScrollView.transform.parent;
        //int slotsCount = 0;
        //foreach (Transform child in canvas)
        //{
        //    if (child.tag == "AnswerGrid")
        //    {
        //        if (child == transform) break;
        //        slotsCount += child.childCount;
        //    }
        //}
        //return slotsCount;

        BindingControl bc = PagesScrollView.gameObject.GetComponent<BindingControl>();
        int slotCount = 0;
        foreach (GameObject grid in bc.Grids)
        {
            if (grid == transform.gameObject) break;
            else slotCount += grid.transform.childCount;
        }
        return slotCount;
    }
}
