using System.Collections;
using System.Collections.Generic;
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
            // если айтем не пустой и если это €чейка пулла или €чейка грида с таким же тегом или айтем в режиме настройки, то перетащить айтем в данную €чейку
            if (draggableItem != null && (gameObject.tag == "Untagged" || draggableItem.tag == gameObject.tag || draggableItem.tag == "setting")) draggableItem.parentAfterDrag = transform;
            // if (draggableItem != null) draggableItem.parentAfterDrag = transform;
        }
    }
}
