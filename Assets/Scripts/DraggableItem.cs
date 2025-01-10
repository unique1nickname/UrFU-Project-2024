using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform initialParent; // нужен для костыля в InventorySlot, чтобы выйти через скрипт по айтему на Scroll View

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin drag");
        initialParent = transform.parent;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        initialParent = transform.parent;

        if (transform.parent.parent != null && transform.parent.parent.tag == "LevelPage")
        {
            Transform item = transform;
            GridLayoutGroup gridLayoutGroup = transform.parent.GetComponent<GridLayoutGroup>();
            Transform itemNameObject = item.transform.GetChild(0);
            Text itemUIText = itemNameObject.GetComponent<Text>();
            float PaddingX = 25; // отступы по бокам от текста
            gridLayoutGroup.cellSize = new Vector2((itemUIText.preferredWidth + PaddingX) * itemNameObject.localScale.x, gridLayoutGroup.cellSize.y);
        }
    }


    // сравнение айтемов, удалить.
    //public override bool Equals(object other)
    //{
    //    //if (other is DraggableItem && transform.childCount != 0 && ((DraggableItem) other).transform.childCount != 0)
    //    //{
    //    //    var otherDI = (DraggableItem) other;
    //    //    var otherText = otherDI.transform.GetChild(0).GetComponent<TMP_Text>();
    //    //    var text = transform.GetComponent<TMP_Text>();
    //    //    if (text.text == otherText.text) return true;
    //    //}
    //    if (other is DraggableItem && name == ((DraggableItem)other).name) return true; 
    //    return base.Equals(other);
    //}

    //public override int GetHashCode()
    //{
    //    return (name != null ? name.GetHashCode() : 0);
    //}
}

