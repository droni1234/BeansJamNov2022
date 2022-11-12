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

    public void StartGame()
    {
        isMenuVisible = false;
        SceneManager.LoadSceneAsync(startScene, Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
