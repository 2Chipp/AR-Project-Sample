using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class UIManager : MonoBehaviour
{
    private enum MenuType { MainMenu, ItemMenu, ARPositionMenu }

    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject itemMenuCanvas;
    [SerializeField] private GameObject aRPositionMenuCanvas;
    // Start is called before the first frame update
    void Start()
    {
     
    }
    private void Awake()
    {
        Init();
    }
    void Init()
    {
        GameManager.gameManager.OnMainMenu += ActivateMainMenu;
        GameManager.gameManager.OnItemMenu += ActivateItemMenu;
        GameManager.gameManager.OnARPositionMenu += ActivateARPositionMenu;
    }


    public void ActivateMainMenu()
    {
        mainMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(1, 1, 1), 0.3f);
        mainMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(1, 1, 1), 0.3f);
        mainMenuCanvas.transform.GetChild(2).transform.DOScale(new Vector3(1, 1, 1), 0.3f);

        DisableMenu(MenuType.ItemMenu);
        DisableMenu(MenuType.ARPositionMenu);
    }

    private void ActivateItemMenu()
    {
        itemMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(1, 1, 1), 0.3f);
        itemMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(1, 1, 1), 0.3f);

        DisableMenu(MenuType.MainMenu);
        DisableMenu(MenuType.ARPositionMenu);
    }

    private void ActivateARPositionMenu()
    {
        aRPositionMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(1, 1, 1), 0.3f);
        aRPositionMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(1, 1, 1), 0.3f);

        DisableMenu(MenuType.ItemMenu);
        DisableMenu(MenuType.MainMenu);
    }

    private void DisableMenu(MenuType menuType)
    {
        switch (menuType)
        {
            case MenuType.MainMenu:
                mainMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(0,0,0), 0.5f);
                mainMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(0,0,0), 0.5f);
                mainMenuCanvas.transform.GetChild(2).transform.DOScale(new Vector3(0,0,0), 0.5f);
                break;

            case MenuType.ItemMenu:
                itemMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(0, 0, 0), 0.5f);
                itemMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(0, 0, 0), 0.5f);
                break;

            case MenuType.ARPositionMenu:
                aRPositionMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(0, 0, 0), 0.5f);
                aRPositionMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(0, 0, 0), 0.5f);
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        GameManager.gameManager.OnMainMenu -= ActivateMainMenu;
        GameManager.gameManager.OnItemMenu -= ActivateItemMenu;
        GameManager.gameManager.OnARPositionMenu -= ActivateARPositionMenu;
    }
}
