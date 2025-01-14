using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1Script : MonoBehaviour
{
    private int currentStage = 0;

    [SerializeField] GameObject[] grids;
    [SerializeField] GameObject[] stages;

    [SerializeField] GameObject helpPage;
    [SerializeField] GameObject wrongAnswerWarning;

    public void ChangeStage()
    {
        if (currentStage < stages.Length) 
        {
            stages[currentStage - 1].SetActive(false);
            stages[currentStage].SetActive(true); 
        }
    }

    public bool Check(GameObject grid)
    {
        // Debug.Log("start checking");
        Transform[] slots = new Transform[grid.transform.childCount];
        AnswerGrid answerGrid = grid.transform.GetComponent<AnswerGrid>();
        BindingControl bc = answerGrid.PagesScrollView.GetComponent<BindingControl>();
        int prevSlotsNumber = answerGrid.GetPreviousSlotsNumber();
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            slots[i] = grid.transform.GetChild(i);
        }

        bool isAllRight = true;

        foreach (Transform slot in slots)
        {
            if (slot.childCount == 0) 
            {
                isAllRight = false;
                continue;
            }
            GameObject item = slot.GetChild(0).gameObject;
            int slotIndex = slot.GetSiblingIndex() + prevSlotsNumber;
            int itemIndex = Array.IndexOf(bc.itemsArray, item);

            // Debug.Log(slot.GetSiblingIndex() + " + " + prevSlotsNumber + " = " + itemIndex);

            if (slotIndex != itemIndex)
            {
                var matchingIndexes = bc.itemsArray
                    .Select((obj, index) => new { obj, index })
                    .Where(pair => pair.obj != null && pair.obj.name == item.name)
                    .Select(pair => pair.index)
                    .ToList();
                if (!matchingIndexes.Contains(slotIndex)) 
                {
                    isAllRight = false;
                    Image image = item.GetComponent<Image>();
                    image.color = new Color(0.96f, 0.439f, 0.439f, 1);
                }
            }
        }
        return isAllRight;
    }

    public void CheckStage(int num)
    {
        // Debug.Log("check button");
        if (num != currentStage + 1 || currentStage >= stages.Length) return;
        // Debug.Log("check button 2");
        if (Check(grids[num-1]))
        {
            currentStage = num;
            ChangeStage();
            wrongAnswerWarning.SetActive(false);
        }
        else wrongAnswerWarning.SetActive(true);
    }

    public void OpenHelpPage()
    {
        helpPage.gameObject.SetActive(true);
    }

    public void CloseHelpPage()
    {
        helpPage.gameObject.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < stages.Length; i++) 
        {
            stages[i].SetActive(false);
        }
        stages[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
