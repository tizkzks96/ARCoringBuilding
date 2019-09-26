using OpenCvSharp;
using OpenCvSharp.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coloring : MonoBehaviour
{

    //public MeshRenderer target; //Rendering Setting of Cube 
    //public GameObject canvas; //Canvas which involves UI 
    //public RawImage viewL, viewR; //Result viewer 
    UnityEngine.Rect capRect;//Region of screen shot 
    Texture2D capTexture; //Texture of screenshot image 
    Texture2D colTexture; //Result of image processing(color) 
    Texture2D binTexture; //Result of image processing(gray)

    Mat bgr, bin;

    // Start is called before the first frame update
    void Start()
    {
        int w = Screen.width;
        int h = Screen.height; //Definition of capture region as (0,0) to (w,h) 

        int sx = (int)(w * 0.2);
        int sy = (int)(h * 0.3);
        w = (int)(w * 0.6);
        h = (int)(h * 0.4);

        capRect = new UnityEngine.Rect(sx, sy, w, h); //Creating texture image of the size of capRect
        capTexture = new Texture2D(w, h, TextureFormat.RGB24, false);
    }

    IEnumerator ImageProcessing(MeshRenderer target)
    {
        //canvas.SetActive(false);//Making UIs invisible 
        yield return new WaitForEndOfFrame();
        CreateImage(); //Image Creation

        Point[] corners;
        FindRect(out corners);


        TransformImage(corners);
        ShowImage(target); //Image Visualization 

        bgr.Release();
        bin.Release();

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

        Cv2.WarpPerspective(bgr, bgr, transform, new Size(512, 512));
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

            Point[] tmp = Cv2.ApproxPolyDP(contours[i], length * 0.01f, true);

            double area = Cv2.ContourArea(contours[i]);
            if (tmp.Length == 4 && area > maxArea)
            {
                maxArea = area;
                corners = tmp;
            }
        }

        if (corners != null)
        {
            bgr.DrawContours(new Point[][] { corners }, 0, Scalar.Red, 5);
            //各頂点の位置に円を描画
            for (int i = 0; i < corners.Length; i++)
            {
                bgr.Circle(corners[i], 20, Scalar.Blue, 5);
            }

        }
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
    void ShowImage(MeshRenderer target)
    {
        if (colTexture != null) { DestroyImmediate(colTexture); }
        if (binTexture != null) { DestroyImmediate(binTexture); }

        colTexture = OpenCvSharp.Unity.MatToTexture(bgr);
        binTexture = OpenCvSharp.Unity.MatToTexture(bin);

        //viewL.texture = colTexture;
        //viewR.texture = binTexture;

        /*Setting texture on the coloring target object (cube)*/
        target.material.mainTexture = colTexture;

    }


    public void StartCV(MeshRenderer target)
    {
        StartCoroutine(ImageProcessing(target)); //Calling coroutine. 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
