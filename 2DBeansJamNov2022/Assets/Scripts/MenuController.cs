using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.LoadSceneMode;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private Canvas menuCanvas;
    public int startScene = 0;

    public CanvasGroup controlls;
    public CanvasGroup credits;
    
    private bool isMenuVisible
    {
        get => menuCanvas.gameObject.activeSelf;
        set => menuCanvas.gameObject.SetActive(value);
    }
    
    private void Start()
    {
        
        #if UNITY_WEBGL
        Destroy(GameObject.Find("Exit"));
        #endif
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            credits.interactable = false;
            credits.blocksRaycasts = false;
            credits.alpha = 0F;
            controlls.interactable = false;
            controlls.blocksRaycasts = false;
            controlls.alpha = 0F;
        }
    }

    public void StartGame()
    {
        isMenuVisible = false;
        SceneManager.LoadSceneAsync(startScene, LoadSceneMode.Single);
    }

    public void ShowCredits()
    {
        credits.interactable = true;
        credits.blocksRaycasts = true;
        credits.alpha = 1F;
    }

    public void Controls()
    {
        controlls.interactable = true;
        controlls.blocksRaycasts = true;
        controlls.alpha = 1F;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
