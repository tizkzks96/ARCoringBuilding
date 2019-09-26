using OpenCvSharp;
using OpenCvSharp.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coloring : Singleton<Coloring>
{

    //public MeshRenderer target; //Rendering Setting of Cube 
    public GameObject canvas; //Canvas which involves UI 
    public RawImage viewL, viewR; //Result viewer 
    UnityEngine.Rect capRect;//Region of screen shot 
    Texture2D capTexture; //Texture of screenshot image 
    Texture2D colTexture; //Result of image processing(color) 
    Texture2D binTexture; //Result of image processing(gray)

    Mat bgr, bin;

    public GameObject fitOverlay;

    // Start is called before the first frame update
    void Start()
    {
        int w = Screen.width;
        int h = Screen.height; //Definition of capture region as (0,0) to (w,h) 

        int sx = (int)(w * 0.1);
        int sy = (int)(h * 0.2);
        w = (int)(w * 1);
        h = (int)(h * 0.6);

        capRect = new UnityEngine.Rect(0, sy, w, h); //Creating texture image of the size of capRect
        capTexture = new Texture2D(w, h, TextureFormat.RGB24, false);
    }

    IEnumerator ImageProcessing()
    {
        //canvas.SetActive(false);//Making UIs invisible 
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForEndOfFrame();
        CreateImage(); //Image Creation

        Debug.Log("unity test 3asd");

        Point[] corners;
        FindRect(out corners);

        Debug.Log("unity test 4asd");
        TransformImage(corners);
        ShowImage(); //Image Visualization 
        Debug.Log("unity test 5asd");
        bgr.Release();
        bin.Release();
        fitOverlay.SetActive(true);
        Debug.Log("unity test 1asd");
        // Scean Home 으로 변경
        //SceanContorller.instance.ChangeScean(SceanState.MAIN);
        //canvas.SetActive(true);//Making UIs visible. 
    }

    void TransformImage(Point[] corners)
    {
        if (corners == null) return;

        SortCorners(corners);

        Point2f[] input = { corners[0], corners[1],
                         corners[2], corners[3] };

        Point2f[] square =
                 { new Point2f(0, 0), new Point2f(0, 255),
                new Point2f(255, 255), new Point2f(255, 0) };

        Mat transform = Cv2.GetPerspectiveTransform(input, square);

        Cv2.WarpPerspective(bgr, bgr, transform, new Size(256, 256));
        int s = (int)(256 * 0.05f);
        int w = (int)(256 * 0.9f);
        OpenCvSharp.Rect innerRect = new OpenCvSharp.Rect(s, s, w, w);
        bgr = bgr[innerRect];

    }

    void SortCorners(Point[] corners)
    {
        System.Array.Sort(corners, (a, b) => a.X.CompareTo(b.X));
        if (corners[0].Y > corners[1].Y)
        {
            corners.Swap(0, 1);
        }
        if (corners[3].Y > corners[2].Y)
        {
            corners.Swap(2, 3);
        }
    }

    void FindRect(out Point[] corners)
    {
        corners = null;

        Point[][] contours;
        HierarchyIndex[] h;

        bin.FindContours(out contours, out h, RetrievalModes.External,
                             ContourApproximationModes.ApproxSimple);

        double maxArea = 0;
        for (int i = 0; i < contours.Length; i++)
        {

            double length = Cv2.ArcLength(contours[i], true);

            Point[] tmp = Cv2.ApproxPolyDP(contours[i], length * 0.1f, true);

            double area = Cv2.ContourArea(contours[i]);
            if (tmp.Length == 4 && area > maxArea)
            {
                maxArea = area;
                corners = tmp;
            }
        }

        //if (corners != null)
        //{
        //    bgr.DrawContours(new Point[][] { corners }, 0, Scalar.Red, 5);
        //    for (int i = 0; i < corners.Length; i++)
        //    {
        //        bgr.Circle(corners[i], 20, Scalar.Blue, 5);
        //    }

        //}
    }

    void CreateImage()
    {
        capTexture.ReadPixels(capRect, 0, 0);//Starting capturing 
        capTexture.Apply();//Apply captured image. 

        //Conversion Texure2D to 
        bgr = OpenCvSharp.Unity.TextureToMat(capTexture);

        bin = bgr.CvtColor(ColorConversionCodes.BGR2GRAY);

        bin = bin.Threshold(100, 255, ThresholdTypes.Otsu);
        Cv2.BitwiseNot(bin, bin);

    }
    void ShowImage()
    {
        if (colTexture != null) { DestroyImmediate(colTexture); }
        if (binTexture != null) { DestroyImmediate(binTexture); }

        colTexture = OpenCvSharp.Unity.MatToTexture(bgr);
        binTexture = OpenCvSharp.Unity.MatToTexture(bin);

        viewL.texture = colTexture;
        //viewR.texture = binTexture;

        /*Setting texture on the coloring target object (cube)*/
        //target.material.mainTexture = colTexture;
        
    }

    public void CreatePrefab()
    {
        BuildingInfo createObject = BuildingDatabase.Instance.GetByName("Building_ApartmentLarge_Brown");
        BuildingUI.instance.InstantiateBuildingSlot(createObject.ID, colTexture, canvas.transform.Find("BottomUI/BuildingPanel"));

        SceanContorller.instance.ChangeScean(SceanState.MAIN);
    }


    public void StartCV()
    {
        Debug.Log("unity test 1asd");
        fitOverlay.SetActive(false);
        StartCoroutine(ImageProcessing()); //Calling coroutine. 
        Debug.Log("unity test 2asd");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
