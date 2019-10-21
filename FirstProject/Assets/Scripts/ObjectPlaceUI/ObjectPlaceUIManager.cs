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

    public int maxSlotCount = 4;

    [HideInInspector]
    public GameObject placePlane;

    public GameObject buildingSlot;
    public GameObject emptyBuildingSlot;

    [Header("Menu")]
    public GameObject placeUI;
    public GameObject mainUI;
    public GameObject evironmentUI;
    public GameObject buildingUI;

    [Space(10)]


    public GameObject canvas;

    public GameObject temp;

    public static int BUILDING_SLOT_COUNT = 0;

    private AnimationClip scaleUpclip;
    private AnimationClip scaleDownclip;


    private float uiSize = 400;

    private GameObject m_currentMenu;

    private GameObject spotSquare;

    public GameObject CurrentMenu { get => m_currentMenu;}

    public Sprite buildingSprite;

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
            mainUI = placeUI.transform.Find("MainUI").gameObject;

            evironmentUI = placeUI.transform.Find("EnvironmentUI").gameObject;

            buildingUI = placeUI.transform.Find("BuildingUI").gameObject;

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

            buildingUI.transform.Find("Add").GetComponent<Button>().onClick.AddListener(() => { InstantiateEmptyBuildingSlot(); });
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

            InstantiateBuildingSlot(BuildingDatabase.Instance.GetByID(0));
            //InstantiateBuildingSlot(1);
            //InstantiateBuildingSlot(2);

            //Animation 초기화
            InitAnimationClip();

            //캡슐화   
            mainUI.SetActive(false);
            evironmentUI.SetActive(false);
            buildingUI.SetActive(false);
            spotSquare.SetActive(false);
        }

    }

    public void InitAnimationClip()
    {
        ScaleUpAnimationClip(400, out scaleUpclip);
        //ScaleDownAnimationClip(400, out scaleDownclip);

        print("scaleUpclip : " + scaleUpclip);
        print("scaleDownclip : " + scaleDownclip);
    }

    public void StartPlace(TapGesture gesture)
    {
        if(spotSquare.activeSelf == true)
        {
            return;
        }

        if (spotSquare.activeSelf == false)
        {
            m_currentMenu.SetActive(true);
        }
        placePlane.transform.SetParent(gesture.TargetObject.transform);
        HelloARController.instance.TapGesturePositionCorrection(gesture, placePlane.transform, 0.01f);
        placePlane.transform.localRotation = Quaternion.Euler(0,0,0);
        //spotSquare.transform.GetChild(1).gameObject.SetActive(true);


        SetButtonPosition(0.5f);
        ChangeState(UIState.HOME);
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

    public void InstantiateEmptyBuildingSlot()
    {
        if (BUILDING_SLOT_COUNT >= maxSlotCount)
        {
            Destroy(buildingUI.transform.Find("Add").gameObject);
            SetButtonPosition(0.5f);
        }

        GameObject slot = Instantiate(emptyBuildingSlot, buildingUI.transform);

        slot.GetComponent<Button>().onClick.AddListener(() => {
            SetEditingSlotInfo(slot);
            SceanContorller.instance.ChangeScean(SceanState.AUGMENTEDIMAGE);
        });

        slot.GetComponent<SlotInfo>().Slotinfo.ID = BUILDING_SLOT_COUNT;

        SetButtonPosition(0.5f);

        BUILDING_SLOT_COUNT++;
    }

    public void SetEditingSlotInfo(GameObject slot)
    {
        SceanContorller.instance.EdtingSlot = slot;
        print("coloring instant edi : " + slot);
    }

    public void InstantiateBuildingSlot(BuildingInfo buildingInfo, Material mat = null,bool _default = true)
    {
        //슬롯생성
        GameObject slot;

        if (_default)
        {
            slot = Instantiate(buildingSlot, evironmentUI.transform);
        }
        else
        {
            slot = SceanContorller.instance.EdtingSlot;
            //slot = Instantiate(buildingSlot, buildingUI.transform);
        }
        print("asdfasdf : " + slot);
        //슬롯정보
        BuildingInfo slotInfo = slot.GetComponent<SlotInfo>().Slotinfo;

        //슬롯에 넣을 빌딩 생성
        GameObject building = Instantiate(buildingInfo.BuildingPrefab);

        building.transform.localScale = new Vector3(0.006f, 0.006f, 0.006f);

        //building.SetActive(false);

        //슬롯정보에 머티리얼 할당
        if (mat != null)
        {
            building.GetComponent<Renderer>().material = mat;

            //slotInfo.BuildingPrefab.GetComponent<Renderer>().sharedMaterial.mainTexture = texture;
        }

        //슬롯에 빌딩 할당
        slotInfo.BuildingPrefab = building;

        slot.GetComponent<Image>().sprite = buildingSprite; 

        //임시 빌딩 해제
        //Destroy(building);

        //slot.GetComponent<Image>().sprite = building.GetComponent<Renderer>().material.GetTexture;
        slot.GetComponent<Button>().onClick.RemoveAllListeners();
            
        slot.GetComponent<Button>().onClick.AddListener(() => {
            HelloARController.instance.PlaceObject(placePlane, slotInfo.BuildingPrefab);
            ChangeState(UIState.NONE);
            });

        SetButtonPosition(0.5f);

        building.SetActive(false);
        //Coloring.Instance.EdtingSlot = null;
        //slot.transform.GetChild(0).GetComponent<Text>().text = "made : " + slotInfo.Name;
    }


    public void ChangeState(UIState uiState)
    {
        switch (uiState)
        {
            case UIState.NONE:
                //캡슐화
                //StartCoroutine(Scale(false, 10, m_currentMenu));
                PlayAnimation(m_currentMenu.GetComponent<Animation>(), "ScaleDown");

                spotSquare.SetActive(false);

                m_currentMenu = mainUI;
                print("qweqwe : " + m_currentMenu);

                break;
            case UIState.HOME:
                m_currentMenu = mainUI;

                SetButtonPosition(0.5f);


                //StartCoroutine(Scale(true, 10, m_currentMenu));

                spotSquare.SetActive(true);

                mainUI.SetActive(true);

                evironmentUI.SetActive(false);

                buildingUI.SetActive(false);

                PlayAnimation(m_currentMenu.GetComponent<Animation>(), "ScaleUp");

                break;
            case UIState.ENVIRONMENT:
                m_currentMenu = evironmentUI;

                SetButtonPosition(0.5f);


                //StartCoroutine(Scale(true, 10, m_currentMenu));

                spotSquare.SetActive(true);

                mainUI.SetActive(false);

                evironmentUI.SetActive(true);

                buildingUI.SetActive(false);

                PlayAnimation(m_currentMenu.GetComponent<Animation>(), "ScaleUp");

                break;
            case UIState.BUILDING:
                m_currentMenu = buildingUI;

                SetButtonPosition(0.5f);


                //StartCoroutine(Scale(true, 10, m_currentMenu));

                spotSquare.SetActive(true);

                mainUI.SetActive(false);

                evironmentUI.SetActive(false);

                buildingUI.SetActive(true);

                PlayAnimation(m_currentMenu.GetComponent<Animation>(), "ScaleUp");

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

                targetRect.localScale = Vector3.Lerp(targetRect.transform.localScale, Vector3.one * uiSize, Time.deltaTime * transitionSpeed);

                if (Mathf.Abs((Vector2.one).sqrMagnitude - targetRect.transform.localScale.sqrMagnitude) < 5f)
                {
                    targetRect.localScale = Vector3.one * uiSize;
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
                    break;
                }
            }
            yield return null;
        }

        if(state == false)
        {
            targetRect.transform.localScale = Vector3.one * uiSize;
        }
        yield return null;
    }

    public void ScaleUpAnimationClip(int size, out AnimationClip clip)
    {
        AnimationCurve curve;

        // create a new AnimationClip
        clip = new AnimationClip();
        clip.legacy = true;

        // create a curve to move the GameObject and assign to the clip
        Keyframe[] keys;

        keys = new Keyframe[2];

        curve = new AnimationCurve(keys);

        keys[0] = new Keyframe(0.0f, 0);
        keys[1] = new Keyframe(.5f, size);
        clip.SetCurve("", typeof(RectTransform), "sizeDelta.x", curve);

        keys[0] = new Keyframe(0.0f, 0);
        keys[1] = new Keyframe(.5f, size);
        clip.SetCurve("", typeof(RectTransform), "scale.y", curve);

        keys[0] = new Keyframe(0.0f, 0);
        keys[1] = new Keyframe(.5f, size);
        clip.SetCurve("", typeof(RectTransform), "scale.z", curve);
    }

    public void ScaleDownAnimationClip(int size, out AnimationClip clip)
    {
        AnimationCurve curve;

        // create a new AnimationClip
        clip = new AnimationClip
        {
            legacy = true
        };

        // create a curve to move the GameObject and assign to the clip
        Keyframe[] keys;

        keys = new Keyframe[2];

        curve = new AnimationCurve(keys);

        keys[0] = new Keyframe(0.0f, size);
        keys[1] = new Keyframe(.5f, 0);
        clip.SetCurve("", typeof(RectTransform), "localScale.x", curve);

        keys[0] = new Keyframe(0.0f, size);
        keys[1] = new Keyframe(.5f, 0);
        clip.SetCurve("", typeof(RectTransform), "localScale.y", curve);

        keys[0] = new Keyframe(0.0f, size);
        keys[1] = new Keyframe(.5f, 0);
        clip.SetCurve("", typeof(RectTransform), "localScale.z", curve);
    }

    public void PlayAnimation(Animation anim, string clipName)
    {
        // update the clip to a change the red color
        //curve = AnimationCurve.Linear(0.0f, 1.0f, 2.0f, 0.0f);
        //clip.SetCurve("", typeof(Material), "_Color.r", curve);
        //clip.wrapMode = WrapMode.Loop;
        // now animate the GameObject
        //anim.AddClip(clip, clip.name);
        print("qweqweqweweqwe : " + m_currentMenu);

        anim.clip = anim.GetClip(clipName);
        anim.Play();
    }
}
