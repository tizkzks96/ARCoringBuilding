using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using GoogleARCore;

public class Screenshot : MonoBehaviour
{
    public static Screenshot instance;
    // The "m_Display" is the GameObject whose Texture will be set to the captured image.
    public Image m_Display;

    public Sprite e;

    public RawImage image;

    /// <summary>
    /// The Unity Awake() method.
    /// </summary>
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

    public void CaptureScreen(AugmentedImage image)
    {

        //StartCoroutine(RecordFrame());
        Photo();
        StartCoroutine(GetPhoto(image));
        
    }

    IEnumerator RecordFrame()
    {
        yield return new WaitForEndOfFrame();
        var texture = ScreenCapture.CaptureScreenshotAsTexture();
        ScreenCapture.CaptureScreenshot("asd");
        Debug.Log("Debug.Log(Application.persistentDataPath);" + Application.persistentDataPath);
        Rect rect = new Rect(0, 0, texture.width, texture.height);

        m_Display.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

        print("AA");
        // cleanup
        Object.Destroy(texture);

        yield return null;
    }


    public void Photo()
    {
        #if UNITY_EDITOR
            ScreenCapture.CaptureScreenshot("./Assets/temp.png");
        #endif

        #if !UNITY_EDITOR
            ScreenCapture.CaptureScreenshot("temp.png");
        #endif
    }

    IEnumerator GetPhoto(AugmentedImage aImage)
    {
        yield return new WaitForSeconds(1.0f);
        print("Application.persistentDataPath : " + "./" + Application.dataPath);
        #if UNITY_EDITOR
            string url = Application.dataPath + "/"+"temp.png";
        #endif
        #if !UNITY_EDITOR
            string url = Application.persistentDataPath +"/"+"temp.png";
        #endif



        var bytes = File.ReadAllBytes(url);
        Texture2D texture = new Texture2D(73, 73);
        texture.LoadImage(bytes);

        //AugmentedImageController.instance.CaptureArea = texture;
        AugmentedImageController.instance.Capture((int)aImage.ExtentX, (int)aImage.ExtentZ);

        image.texture = texture;
    }
}
