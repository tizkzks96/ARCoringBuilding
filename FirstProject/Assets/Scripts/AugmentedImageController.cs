using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AugmentedImageController : MonoBehaviour
{
    public Button AugmentedImageSceanExitBtn;

    private RectTransform m_captureArea;

    public RectTransform CaptureArea { get => m_captureArea; set => m_captureArea = value; }

    public static AugmentedImageController instance;

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
        AugmentedImageSceanExitBtn.onClick.AddListener(()=> { SceanContorller.instance.ChangeScean(SceanState.MAIN); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
