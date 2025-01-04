using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerGrid : MonoBehaviour
{
    [SerializeField] public GameObject PagesScrollView;

    public int GetPreviousSlotsNumber()
    {
        Transform canvas = PagesScrollView.transform.parent;
        int slotsCount = 0;
        foreach (Transform child in canvas)
        {
            if (child.tag == "AnswerGrid")
            {
                if (child == transform) break;
                slotsCount += child.childCount;
            }
        }
        return slotsCount;
    }
}
