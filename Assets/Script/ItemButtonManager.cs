using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButtonManager : MonoBehaviour
{
    public string ItemName { get; set; }
    public Sprite ItemImage { get; set; }
    public string ItemDescription { get; set; }
    public GameObject ItemModelPrefab { get; set; }

    private ARInteractionManager aRInteractionManager;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    
    void Init()
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ItemName;
        transform.GetChild(1).GetComponent<RawImage>().texture = ItemImage.texture;
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ItemDescription;

        var button = GetComponent<Button>();
        button.onClick.AddListener(GameManager.gameManager.ARPositionMenu);
        button.onClick.AddListener(Create3DModel);

        aRInteractionManager = FindObjectOfType<ARInteractionManager>();
    }

    private void Create3DModel()
    {
        aRInteractionManager.ItemModel = Instantiate(ItemModelPrefab);
    }
}
