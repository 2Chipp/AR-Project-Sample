using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : MonoBehaviour
{
    public event Action OnMainMenu;
    public event Action OnItemMenu;
    public event Action OnARPositionMenu;

    public static GameManager gameManager;

    private void Awake()
    {
        if (gameManager == null) gameManager = this;
        else Destroy(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        MainMenu();
    }

    public void MainMenu()
    {
        OnMainMenu?.Invoke();
        Debug.Log("MainMenu");
    }

    public void ItemsMenu()
    {
        OnItemMenu?.Invoke();
        Debug.Log("ItemMenu");
    }

    public void ARPositionMenu()
    {
        OnARPositionMenu?.Invoke();
        Debug.Log("ARPositionMenu");
    }

    public void CloseApp()
    {
        Application.Quit();
    }
}
