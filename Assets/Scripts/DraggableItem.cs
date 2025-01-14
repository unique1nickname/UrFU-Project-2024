using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform initialParent; // ����� ��� ������� � InventorySlot, ����� ����� ����� ������ �� ������ �� Scroll View (upd: ������ ����� ��� ������� �������)

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
        if (initialParent.parent != null && initialParent.parent.tag == "LevelPage")
        {
            // ��������� ��������
            Image image = initialParent.GetComponent<Image>();
            Color color = image.color;
            color.a = 1;
            image.color = color;
        }

        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        initialParent = transform.parent;

        if (transform.parent.parent != null && transform.parent.parent.tag == "LevelPage")
        {
            Transform item = transform;
            GridLayoutGroup gridLayoutGroup = transform.parent.GetComponent<GridLayoutGroup>();
            Transform itemNameObject = item.transform.GetChild(0);
            Text itemUIText = itemNameObject.GetComponent<Text>();
            float PaddingX = 25; // ������� �� ����� �� ������
            gridLayoutGroup.cellSize = new Vector2((itemUIText.preferredWidth + PaddingX) * itemNameObject.localScale.x, gridLayoutGroup.cellSize.y);

            // ������� ��������
            Image image = transform.parent.GetComponent<Image>();
            Color color = image.color;
            color.a = 0;
            image.color = color;
        }
    }


    // ��������� �������, �������.
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

