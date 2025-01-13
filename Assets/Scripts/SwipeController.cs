using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] GameObject pages;
    int maxPage;
    int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;

    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;

    [SerializeField] GameObject pageNumberText;

    float dragThreshould;

    private void Awake()
    {
        maxPage = pages.transform.childCount;
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
        dragThreshould = Screen.width / 15;
        pageNumberText.GetComponent<TMP_Text>().text = "<color=#9A9A9A>" + currentPage + "/</color>" + maxPage;
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            pageNumberText.GetComponent<TMP_Text>().text = "<color=#9A9A9A>" + currentPage + "/</color>" + maxPage;
            MovePage();
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            pageNumberText.GetComponent<TMP_Text>().text = "<color=#9A9A9A>" + currentPage + "/</color>" + maxPage;
            MovePage();
        }
    }

    void MovePage()
    {
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        maxPage = pages.transform.childCount;
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshould)
        {
            if (eventData.position.x > eventData.pressPosition.x) Previous();
            else Next();
        }
        else MovePage();
    }
}
