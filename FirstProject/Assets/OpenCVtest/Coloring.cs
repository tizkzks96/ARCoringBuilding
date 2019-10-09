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
        int h = Screen.height; //스크린 가로, 세로 영역

        int sx = (int)(w * 0);
        int sy = (int)(h * 0);
        w = (int)(w * 1);
        h = (int)(h * 1);

        capRect = new UnityEngine.Rect(sx, sy, w, h); //CapRect 크기의 텍스처 이미지 생성
        capTexture = new Texture2D(w, h, TextureFormat.RGB24, false);
    }

    IEnumerator ImageProcessing()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForEndOfFrame();
        CreateImage(); //이미지 생성


        Point[] corners;
        FindRect(out corners); //사각형 영역 추출

        TransformImage(corners); //사각형 영역 Transform
        ShowImage(); //Image Visualization
        bgr.Release(); //메모리 해제
        bin.Release(); // 메모리 해제
        fitOverlay.SetActive(true); 

        // Scean Home 으로 변경
        //SceanContorller.instance.ChangeScean(SceanState.MAIN);
    }

    void TransformImage(Point[] corners)
    {
        if (corners == null) return;

        //이미지 정렬
        SortCorners(corners);

        Point2f[] input = { corners[0], corners[1],
                         corners[2], corners[3] };

        Point2f[] square =
                 { new Point2f(0, 0), new Point2f(0, 255),
                new Point2f(255, 255), new Point2f(255, 0) };

        //원근제거 matrix
        Mat transform = Cv2.GetPerspectiveTransform(input, square);

        //4개의 점을 이용하여 원근제거
        Cv2.WarpPerspective(bgr, bgr, transform, new Size(256, 256));
        int s = (int)(256 * 0.05f);
        int w = (int)(256 * 0.9f);

        //외각 라인 제거
        //x, y, width, height
        OpenCvSharp.Rect innerRect = new OpenCvSharp.Rect(s, s, w, w);
        bgr = bgr[innerRect];
    }

    //이미지 정렬
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

    //참고 : https://vovkos.github.io/doxyrest-showcase/opencv/sphinx_rtd_theme/enum_cv_ContourApproximationModes.html
    void FindRect(out Point[] corners)
    {
        corners = null;

        Point[][] contours;
        HierarchyIndex[] h;

        //연결 컴포넌트의 외곽선 탐지
        //외곽선 벡터, hierarchy, 
        //윤곽 검색 모드 설정(external) : 외곽선 
        //윤곽 근사방법(simple) : 수평, 수직 및 대각선 세그먼트를 압축하고 끝점만 남음. 예를 들어, 직각 직사각형 형상은 4 포인트로 인코딩됨.
        bin.FindContours(out contours, out h, RetrievalModes.External,
                             ContourApproximationModes.ApproxSimple);


        //참고 : https://datascienceschool.net/view-notebook/f9f8983941254a34bf0fee42c66c5539/
        // 가장 큰 사각형 추출
        double maxArea = 0;
        for (int i = 0; i < contours.Length; i++)
        {

            double length = Cv2.ArcLength(contours[i], true);

            //컨투어 추정
            //Douglas-Peucker 알고리즘을 이용해 컨투어 포인트의 수를 줄여 실제 컨투어 라인과 근사한 라인을 그릴 때 사용
            Point[] tmp = Cv2.ApproxPolyDP(contours[i], length * 0.1f, true);

            double area = Cv2.ContourArea(contours[i]);
            if (tmp.Length == 4 && area > maxArea)
            {
                maxArea = area;
                corners = tmp;
            }
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

    void CreateImage()
    {
        capTexture.ReadPixels(capRect, 0, 0);//이미지 캡쳐
        capTexture.Apply();//캡쳐 이미지 적용

        //texture 를 matrix 로 변환
        bgr = OpenCvSharp.Unity.TextureToMat(capTexture);

        //이미지 생상 그레이 스케일로 변환
        bin = bgr.CvtColor(ColorConversionCodes.BGR2GRAY);

        //이미지 이진화
        //임계값, 최대값, 임계값 종류
        //ThresholdTypes.Otsu : 단일 채널에서 사용 가능, 자동으로 임계값 검출
        bin = bin.Threshold(100, 255, ThresholdTypes.Otsu);

        //흑백이미지 반전
        Cv2.BitwiseNot(bin, bin);
    }

    void ShowImage()
    {
        if (colTexture != null) { DestroyImmediate(colTexture); }
        if (binTexture != null) { DestroyImmediate(binTexture); }

        colTexture = OpenCvSharp.Unity.MatToTexture(bgr);
        binTexture = OpenCvSharp.Unity.MatToTexture(bin);

        viewL.texture = colTexture;
    }

    public void CreatePrefab()
    {
        BuildingInfo createObject = BuildingDatabase.Instance.GetByName("Building_ApartmentLarge_Brown");

        ObjectPlaceUIManager.instance.InstantiateBuildingSlot(createObject.ID, colTexture, false);

        SceanContorller.instance.ChangeScean(SceanState.MAIN);
    }


    public void StartCV()
    {
        fitOverlay.SetActive(false);
        StartCoroutine(ImageProcessing()); //Calling coroutine. 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
