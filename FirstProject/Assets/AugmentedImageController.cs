using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AugmentedImageController : MonoBehaviour
{
    public Button AugmentedImageSceanExitBtn;
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
