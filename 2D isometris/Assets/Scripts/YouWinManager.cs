using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouWinManager : MonoBehaviour
{
   
    public GameObject youWinUI; 

    public static YouWinManager singleton;

    private void Awake() {
        singleton = this;
    }
    public void TriggerYouWin()
    {
        youWinUI.SetActive(true);
        Time.timeScale = 0;
    }
}
