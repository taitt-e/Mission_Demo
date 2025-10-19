using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartLevelButton : MonoBehaviour
{
    public Button setButton;
    //  Setup task listener for button on click
    public void Start()
    {
        Button btn = setButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        //If button is clicked reset level.
        MissionDemolition.RESET_LEVEL();
    }
}
