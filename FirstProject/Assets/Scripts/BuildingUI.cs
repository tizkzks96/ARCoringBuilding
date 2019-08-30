using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.Examples.HelloAR;
using GoogleARCore;
using GoogleARCoreInternal;
using System.IO;

public enum UIState{
    HOME,
    ENVIRONMENT,
    BUILDING
}

/// <summary>
/// 
/// 
/// 
/// 나중에 여기에 BuildingSlot 추가할 때 디비에서 가져온걸 for문 돌려서 추가해야함
/// </summary>
public class BuildingUI : MonoBehaviour
{
    public static BuildingUI instance;

    public GameObject HomePanel;

    public GameObject EnvironmentPanel;

    public GameObject BuildingPanel;

    public GameObject buildingSlot;

    public UIState UIState { get; set; } = UIState.HOME;

    public void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }


        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {


        #region UIState 이동 버튼
        //HomePanel 버튼 연결
        HomePanel.transform.Find("EnvironmentBtn").GetComponent<Button>().onClick.AddListener(() => { ChangeState(UIState.ENVIRONMENT); });
        HomePanel.transform.Find("BuildingsBtn").GetComponent<Button>().onClick.AddListener(() => { ChangeState(UIState.BUILDING); });

        //EnvironmentPanel 버튼 연결
        EnvironmentPanel.transform.Find("Home").GetComponent<Button>().onClick.AddListener(() => { ChangeState(UIState.HOME); });

        //BuildingPanel 버튼 연결
        BuildingPanel.transform.Find("Home").GetComponent<Button>().onClick.AddListener(() => { ChangeState(UIState.HOME); });

        BuildingPanel.transform.Find("Add").GetComponent<Button>().onClick.AddListener(() => { SceanContorller.instance.ChangeScean(SceanState.AUGMENTEDIMAGE); });
        #endregion



        //기본 빌딩슬롯 생성
        InstantiateBuildingSlot(0);
        InstantiateBuildingSlot(1);
        InstantiateBuildingSlot(2);

    }

    /// <summary>
    /// UI Panel 상태를 체인지한다.
    /// </summary>
    public void ChangeState(UIState uiState)
    {
        switch (uiState)
        {
            case UIState.HOME:
                HomePanel.SetActive(true);

                EnvironmentPanel.SetActive(false);

                BuildingPanel.SetActive(false);
                break;
            case UIState.ENVIRONMENT:
                HomePanel.SetActive(false);

                EnvironmentPanel.SetActive(true);

                BuildingPanel.SetActive(false);
                break;
            case UIState.BUILDING:
                HomePanel.SetActive(false);

                EnvironmentPanel.SetActive(false);

                BuildingPanel.SetActive(true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// BuildingSlot object 를 생성한다.
    /// </summary>
    /// <param name="index">BuildingDatabase에 저장된 index</param>
    public void InstantiateBuildingSlot(int index)
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas == null)
            return;

        //slot 생성
        GameObject slot = Instantiate(buildingSlot, canvas.transform.Find("BottomUI/EnvironmentPanel"));

        BuildingInfo slotInfo = BuildingDatabase.Instance.GetByID(index);
        
        slotInfo.BuildingPrefab.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        slot.GetComponent<SlotInfo>().Slotinfo = slotInfo;

        slot.transform.GetChild(0).GetComponent<Text>().text = slotInfo.Name;


        //slot buildingsystem 변수에 this 연결
        //slot.GetComponent<PreparationBlockSlotDragHandler>().BuildingSystem = transform.GetComponent<BuildingSystem>();

        print("slot : " + slot);
    }






    public void temp(GameObject buildingInfo)
    {
        print("temp : " + HelloARController.instance.HorizontalPlanePrefab);

        HelloARController.instance.HorizontalPlanePrefab = buildingInfo;
        print("temp1 : " + HelloARController.instance.HorizontalPlanePrefab);
    }


    private byte[] imageByte;   //스크린샷을 Byte로 저장.Texture2D use 
    Image pictureInScene;
    WaitForSeconds waitTime = new WaitForSeconds(0.1F);
    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

    public IEnumerator CaptureTest()
    {
        yield return waitTime;
        yield return frameEnd;

        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        //2d texture객체를 만드는대.. 스크린의 넓이, 높이를 선택하고 텍스쳐 포멧은 스크린샷을 찍기 위해서는 이렇게 해야 한다더군요. 

        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
        //말 그대로입니다. 현제 화면을 픽셀 단위로 읽어 드립니다. 

        tex.Apply();
        //읽어 들인것을 적용. 

        imageByte = tex.EncodeToPNG();
        //읽어 드린 픽셀을 Byte[] 에 PNG 형식으로 인코딩 해서 넣게 됩니다. 

        DestroyImmediate(tex);
        //Byte[]에 넣었으니 Texture 2D 객체는 바로 파괴시켜줍니다.(메모리관리) 

        pictureInScene.sprite = Sprite.Create(tex, new Rect(0, 0, 128, 128), new Vector2());//set the Rect with position and dimensions as you need
    }





}
