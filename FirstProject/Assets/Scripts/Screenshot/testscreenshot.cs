using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class testscreenshot : MonoBehaviour
{
    // The "m_Display" is the GameObject whose Texture will be set to the captured image.
    public Image m_Display;

    public Sprite e;

    public void CaptureScreen()
    {
        //StartCoroutine(RecordFrame());
        Photo();
        Invoke("GetPhoto", 1f);
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

    public RawImage image;
    void Start()
    {
        
    }

    public void Photo()
    {
        ScreenCapture.CaptureScreenshot("a");
    }

    public void GetPhoto()
    {
         string url = Application.persistentDataPath +"/"+"a";
         var bytes = File.ReadAllBytes( url );
         Texture2D texture = new Texture2D( 73, 73 );
         texture.LoadImage( bytes );
         image.texture= texture ;
    }


}
