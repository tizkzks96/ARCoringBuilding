using GoogleARCore.Examples.HelloAR;
using GoogleARCore.Examples.ObjectManipulation;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum UIState
{
    NONE,
    HOME,
    ENVIRONMENT,
    BUILDING
}

public class ObjectPlaceUIManager : MonoBehaviour
{
    public static ObjectPlaceUIManager instance;

    public GameObject PlacePlane;

    [HideInInspector]
    public GameObject placePlane;

    public GameObject buildingSlot;

    [Header("Menu")]
    public GameObject mainUI;
    public GameObject evironmentUI;
    public GameObject buildingUI;

    [Space(10)]

    private GameObject spotSquare;

    public GameObject canvas;

    public GameObject temp;

    private GameObject m_currentMenu;

    public GameObject CurrentMenu { get => m_currentMenu;}

    public void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            InitPlace();
            //if not, set instance to this
            instance = this;

        }


        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    public void InitPlace()
    {
        if (placePlane == null)
        {
            //data class
            placePlane = Instantiate(PlacePlane);

            canvas = placePlane.transform.Find("Canvas").gameObject;

            //UI Link
            mainUI = canvas.transform.Find("MainUI").gameObject;

            evironmentUI = canvas.transform.Find("EnvironmentUI").gameObject;

            buildingUI = canvas.transform.Find("BuildingUI").gameObject;

            spotSquare = canvas.transform.Find("spotSquare").gameObject;

            #region UIState 이동 버튼
            //HomePanel 버튼 연결
            mainUI.transform.Find("Exit").GetComponent<Button>().onClick.AddListener(() => { ChangeState(UIState.NONE); });
            mainUI.transform.Find("Building").GetComponent<Button>().onClick.AddListener(() => { ChangeState(UIState.ENVIRONMENT); });
            mainUI.transform.Find("Enviroment").GetComponent<Button>().onClick.AddListener(() => { ChangeState(UIState.BUILDING); });

            //EnvironmentPanel 버튼 연결
            evironmentUI.transform.Find("Home").GetComponent<Button>().onClick.AddListener(() => { ChangeState(UIState.HOME); });

            //BuildingPanel 버튼 연결
            buildingUI.transform.Find("Home").GetComponent<Button>().onClick.AddListener(() => { ChangeState(UIState.HOME); });

            buildingUI.transform.Find("Add").GetComponent<Button>().onClick.AddListener(() => { SceanContorller.instance.ChangeScean(SceanState.AUGMENTEDIMAGE); });
            #endregion

            m_currentMenu = mainUI;

            for(int i = 1; i< evironmentUI.transform.childCount - 1; i++)
            {
                Transform button = evironmentUI.transform.GetChild(i);
                button.GetComponent<Button>().onClick.AddListener(() => {
                    HelloARController.instance.PlaceObject(placePlane, button.GetComponent<SlotInfo>().slotinfo.BuildingPrefab);
                    ChangeState(UIState.NONE);
                });
                Debug.Log("pe : " + button.GetComponent<SlotInfo>().slotinfo.BuildingPrefab);

            }
            InstantiateBuildingSlot(0);
            //InstantiateBuildingSlot(1);
            //InstantiateBuildingSlot(2);
            
                
            //캡슐화   
            mainUI.SetActive(false);
            evironmentUI.SetActive(false);
            buildingUI.SetActive(false);
            spotSquare.SetActive(false);
        }

    }

    public void StartPlace(TapGesture gesture)
    {
        if(m_currentMenu.activeSelf == true)
        {
            print("StartPlace true");
            return;
        }

        if (m_currentMenu.activeSelf == false)
        {
            print("StartPlace false");
            m_currentMenu.SetActive(true);
        }
        print("StartPlace run");
        placePlane.transform.SetParent(gesture.TargetObject.transform);
        HelloARController.instance.TapGesturePositionCorrection(gesture, placePlane.transform, 0.01f);
        placePlane.transform.localRotation = gesture.TargetObject.transform.localRotation;

        SetButtonPosition(0.5f);
        ChangeState(UIState.HOME);
        //MakeSlotPosition(20, 5);
    }

    public void SetButtonPosition(float radius)
    {
        if(CurrentMenu == null)
        {
            return;
        }

        int amount = CurrentMenu.transform.childCount;
        print("CurrnetMenu : " + CurrentMenu.transform + " : " + CurrentMenu.transform.childCount);
        for (int i = 0; i < amount; i++)
        {
            GameObject currnetOjbect = CurrentMenu.transform.GetChild(i).gameObject;
            float cornerAngle = 2f * Mathf.PI / (float)amount * i;
            //currnetOjbect.transform.SetParent(placePlane.transform);
            currnetOjbect.transform.localPosition = new Vector2(Mathf.Cos(cornerAngle) * radius,  Mathf.Sin(cornerAngle) * radius);
        }
    }

    public void MakeSlotPosition(float radius, int amount)
    {

        for (int i = 0; i < amount; i++)
        {
            float cornerAngle = 2f * Mathf.PI / (float)amount * i;
            GameObject slotTemp = Instantiate(temp);
            slotTemp.transform.SetParent(placePlane.transform);
            slotTemp.transform.localPosition = new Vector3(Mathf.Cos(cornerAngle) * radius, 0.01f, Mathf.Sin(cornerAngle) * radius);
        }
    }

    public void InstantiateBuildingSlot(int index, Texture2D texture = null, bool _default = true)
    {
        GameObject slot;

        //slot 생성
        if (_default)
        {
            slot = Instantiate(buildingSlot, evironmentUI.transform);
        }
        else
        {
            slot = Instantiate(buildingSlot, buildingUI.transform);
        }

        BuildingInfo slotInfo = BuildingDatabase.Instance.GetByID(index);
        if (texture != null)
        {
            slotInfo.BuildingPrefab.GetComponent<Renderer>().sharedMaterial.mainTexture = texture;
        }

        slotInfo.BuildingPrefab.transform.localScale = new Vector3(0.006f, 0.006f, 0.006f);

        slot.GetComponent<SlotInfo>().Slotinfo = slotInfo;

        slot.GetComponent<Button>().onClick.AddListener(() => {
            HelloARController.instance.PlaceObject(placePlane, slotInfo.BuildingPrefab);
            ChangeState(UIState.NONE);
            });

        SetButtonPosition(0.5f);
        //slot.transform.GetChild(0).GetComponent<Text>().text = "made : " + slotInfo.Name;
    }


    public void ChangeState(UIState uiState)
    {
        switch (uiState)
        {
            case UIState.NONE:
                //캡슐화
                StartCoroutine(Scale(false, 10, m_currentMenu));

                spotSquare.SetActive(false);

                m_currentMenu = mainUI;
                break;
            case UIState.HOME:
                m_currentMenu = mainUI;

                SetButtonPosition(0.5f);

                StartCoroutine(Scale(true, 10, m_currentMenu));

                spotSquare.SetActive(true);

                mainUI.SetActive(true);

                evironmentUI.SetActive(false);

                buildingUI.SetActive(false);
                break;
            case UIState.ENVIRONMENT:
                m_currentMenu = evironmentUI;

                SetButtonPosition(0.5f);

                StartCoroutine(Scale(true, 10, m_currentMenu));

                spotSquare.SetActive(true);

                mainUI.SetActive(false);

                evironmentUI.SetActive(true);

                buildingUI.SetActive(false);
                break;
            case UIState.BUILDING:
                m_currentMenu = buildingUI;

                SetButtonPosition(0.5f);

                StartCoroutine(Scale(true, 10, m_currentMenu));

                spotSquare.SetActive(true);

                mainUI.SetActive(false);

                evironmentUI.SetActive(false);

                buildingUI.SetActive(true);
                break;
            default:
                break;
        }
    }

    private IEnumerator Scale(bool state, float transitionSpeed, GameObject target)
    {
        RectTransform targetRect = target.GetComponent<RectTransform>();
        if(state == true)
        {
            targetRect.localScale = Vector3.zero;
        }

        while (true)
        {
            //true is open
            if (state == true)
            {

                targetRect.localScale = Vector3.Lerp(targetRect.transform.localScale, Vector3.one, Time.deltaTime * transitionSpeed);
                print("BB");
                if (Mathf.Abs((Vector2.one).sqrMagnitude - targetRect.transform.localScale.sqrMagnitude) < .5f)
                {
                    targetRect.localScale = Vector3.one;
                    break;
                }
            }

            //false is close
            else if (state == false)
            {
                targetRect.localScale = Vector3.Lerp(targetRect.transform.localScale, Vector2.zero, Time.deltaTime * transitionSpeed);
                if (targetRect.transform.localScale.x < .05f)
                {
                    targetRect.transform.localScale = Vector3.zero;
                    target.SetActive(false);
                    break;
                }
            }
            yield return null;
        }

        if(state == false)
        {
            targetRect.transform.localScale = Vector3.one;
        }
        yield return null;
    }
}
