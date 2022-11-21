using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.SceneManagement.LoadSceneMode;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private Canvas menuCanvas;
    public string startScene;

    public CanvasGroup controlls;
    public CanvasGroup credits;

    public AudioSource whipSound;

    public TextMeshProUGUI text;
    
    private bool isMenuVisible
    {
        get => menuCanvas.gameObject.activeSelf;
        set => menuCanvas.gameObject.SetActive(value);
    }
    
    private void Start()
    {

        text.text = "Version - " + Application.version;
        
        #if UNITY_WEBGL
        Destroy(GameObject.Find("Exit"));
        #endif
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (credits)
            {
                credits.interactable = false;
                credits.blocksRaycasts = false;
                credits.alpha = 0F;
            }

            if (controlls)
            {
                controlls.interactable = false;
                controlls.blocksRaycasts = false;
                controlls.alpha = 0F;
            }
        }
    }

    public void StartGame()
    {
        isMenuVisible = false;
        SceneManager.LoadSceneAsync(startScene, LoadSceneMode.Single);
    }

    public void ShowCredits()
    {
        whipSound.Play();
        credits.interactable = true;
        credits.blocksRaycasts = true;
        credits.alpha = 1F;
    }

    public void ShowControls()
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
