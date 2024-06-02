using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class ARInteractionManager : MonoBehaviour
{
    [SerializeField] private Camera aRCamera;
    private ARRaycastManager aRRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject aRPointer;
    private GameObject itemModel;
    private GameObject itemSelected;

    private bool isInitialPosition;
    private bool isOverUI;
    private bool isOverModel;

    private Vector2 initialTouchPosition;

    public GameObject ItemModel { set {
            itemModel = value;
            aRPointer.SetActive(true);
            itemModel.transform.position = this.transform.position;
            itemModel.transform.parent = this.transform;
            isInitialPosition = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        aRPointer = transform.GetChild(0).gameObject;
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        GameManager.gameManager.OnMainMenu += SetItemPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInitialPosition)
        {
            Vector2 middlePointScreen = new Vector2(Screen.width / 2, Screen.height / 2);
            aRRaycastManager.Raycast(middlePointScreen, hits, TrackableType.Planes);
            if (hits.Count > 0)
            {
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;
                isInitialPosition = false;
            }
        }

        if (Input.touchCount > 0)
        {
            Touch touchOne = Input.GetTouch(0);
            if(touchOne.phase == TouchPhase.Began)
            {
                var touchPosition = touchOne.position;
                isOverUI = isTapOverUI(touchPosition);
                isOverModel = isTapOverModel(touchPosition);
            }

            if(touchOne.phase == TouchPhase.Moved)
            {
                if(aRRaycastManager.Raycast(touchOne.position, hits, TrackableType.Planes))
                {
                    Pose hitPoise = hits[0].pose;
                    if (!isOverUI && isOverModel)
                    {
                        transform.position = hitPoise.position;
                    }
                }
            }

            if(Input.touchCount == 2)
            {
                Touch touchTwo = Input.GetTouch(1);
                if(touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began)
                {
                    initialTouchPosition = touchTwo.position - touchOne.position;
                }

                if(touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved)
                {
                    Vector2 currentTouchPos = touchTwo.position - touchOne.position;
                    float angle = Vector2.SignedAngle(initialTouchPosition, currentTouchPos);
                    itemModel.transform.rotation = Quaternion.Euler(0, itemModel.transform.eulerAngles.y - angle, 0);
                    initialTouchPosition = currentTouchPos;
                }
            }

            if(isOverModel && itemModel == null && !isOverUI)
            {
                GameManager.gameManager.ARPositionMenu();
                itemModel = itemSelected;
                itemSelected = null;
                aRPointer.SetActive(true);
                transform.position = itemModel.transform.position;
                itemModel.transform.parent = this.transform;
            }
        }
    }

    private bool isTapOverModel(Vector2 touchPosition)
    {
        Ray ray = aRCamera.ScreenPointToRay(touchPosition);
        if(Physics.Raycast(ray, out RaycastHit modelHit))
        {
            if (modelHit.collider.CompareTag("Item"))
            {
                itemSelected = modelHit.transform.gameObject;
                return true;
            }
        }
        return false;
    }

    private bool isTapOverUI(Vector2 touchPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touchPosition.x, touchPosition.y);

        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);

        return result.Count > 0;
    }

    private void SetItemPosition()
    {
        if(itemModel != null)
        {
            itemModel.transform.parent = null;
            aRPointer.SetActive(false);
            itemModel = null;
        }
    }

    public void DeleteItem()
    {
        Destroy(itemModel);
        aRPointer.SetActive(false);
        GameManager.gameManager.MainMenu();
    }
}
