using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            
            if (transform.parent != null && transform.parent.tag == "AnswerGrid")
            {
                AnswerGrid grid = transform.parent.GetComponent<AnswerGrid>();
                GameObject scrollView = grid.PagesScrollView;
                if (scrollView != null && scrollView.tag == "PagesScrollView") 
                { 
                    int prevSlotsNumber = grid.GetPreviousSlotsNumber(); // ����� �� ������, ���� �� ����� ��������� Grid'��
                    BindingControl bc = scrollView.GetComponent<BindingControl>();
                    if (transform.GetSiblingIndex() + prevSlotsNumber == Array.IndexOf(bc.itemsArray, dropped)) draggableItem.parentAfterDrag = transform;
                }
            }
            else if (draggableItem != null && (gameObject.tag == "Untagged" || draggableItem.tag == gameObject.tag || draggableItem.tag == "setting")) draggableItem.parentAfterDrag = transform;
            //// ���������... ���������� ������ �� �������� BindingControl ����� ��������������� ������.
            //if (transform.parent != null && transform.parent.tag == "AnswerGrid")
            //{
            //    Transform page;
            //    try
            //    {
            //        page = draggableItem.initialParent.parent;
            //    }
            //    catch
            //    {
            //        return;
            //    }
            //    if (page != null && page.tag == "LevelPage")
            //    {
            //        Transform ScrollView = page.parent.parent;
            //        if (ScrollView != null && ScrollView.tag == "PagesScrollView")
            //        {
            //            BindingControl bc = ScrollView.GetComponent<BindingControl>();
            //            if (transform.GetSiblingIndex() == Array.IndexOf(bc.itemsArray, dropped)) draggableItem.parentAfterDrag = transform;
            //            //else
            //            //{
            //            //    var matchingIndices = bc.itemsArray
            //            //        .Select((obj, index) => new  { obj, index })
            //            //        .Where(pair => pair.obj != null && pair.obj.name == dropped.name)
            //            //        .Select(pair => pair.index)
            //            //        .ToList();
            //            //    if (matchingIndices.Contains(transform.GetSiblingIndex())) draggableItem.parentAfterDrag = transform;
            //            //    // Debug.Log("��������� � " + string.Join(", ", matchingIndices));
            //            //}
            //            // else Debug.Log("������������ �������� " + transform.GetSiblingIndex().ToString() + " != " + Array.IndexOf(bc.itemsArray, dropped).ToString());
            //        }
            //    }
            //}
            // ���� ����� �� ������ � ���� ��� ������ ����� ��� ������ ����� � ����� �� ����� ��� ����� � ������ ���������, �� ���������� ����� � ������ ������
            //else if (draggableItem != null && (gameObject.tag == "Untagged" || draggableItem.tag == gameObject.tag || draggableItem.tag == "setting")) draggableItem.parentAfterDrag = transform;
        }
    }
}
