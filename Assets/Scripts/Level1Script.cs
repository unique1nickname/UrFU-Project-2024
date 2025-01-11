using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Script : MonoBehaviour
{
    private int currentStage = 0;

    [SerializeField] GameObject[] grids;
    [SerializeField] GameObject[] stages;

    [SerializeField] GameObject helpPage;

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
        Debug.Log("start checking");
        Transform[] slots = new Transform[grid.transform.childCount];
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            slots[i] = grid.transform.GetChild(i);
        }
        foreach (Transform slot in slots)
        {
            if (slot.childCount == 0) return false;
        }
        Debug.Log("hello there!");
        return true;
    }

    public void CheckStage(int num)
    {
        Debug.Log("check button");
        if (num != currentStage + 1 || currentStage >= stages.Length) return;
        Debug.Log("check button 2");
        if (Check(grids[num-1]))
        {
            currentStage = num;
            ChangeStage();
        };
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
