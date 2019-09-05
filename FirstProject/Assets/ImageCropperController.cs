using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.Examples.AugmentedImage;
using GoogleARCore;

namespace ImageCropperNamespace
{
	public class ImageCropperController : MonoBehaviour
	{
        public static ImageCropperController instance;

        public RawImage croppedImageHolder;

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

        public void CropTest()
        {
            // If image cropper is already open, do nothing
            if (ImageCropper.Instance.IsOpen)
                return;

            StartCoroutine(TakeScreenshotAndCrop());
        }

        public void Crop(AugmentedImage image)
		{
			// If image cropper is already open, do nothing
			if( ImageCropper.Instance.IsOpen )
				return;

			StartCoroutine( TakeScreenshotAndCrop(image) );
		}

		private IEnumerator TakeScreenshotAndCrop(AugmentedImage image = null)
		{
			yield return new WaitForEndOfFrame();

			float minAspectRatio, maxAspectRatio;
			minAspectRatio = 1f;
			maxAspectRatio = 1f;

			Texture2D screenshot = new Texture2D( Screen.width, Screen.height, TextureFormat.RGB24, false );
			screenshot.ReadPixels( new Rect( 0, 0, Screen.width, Screen.height ), 0, 0 );
			screenshot.Apply();

			ImageCropper.Instance.Show( screenshot, ( bool result, Texture originalImage, Texture2D croppedImage ) =>
			{
				// Destroy previously cropped texture (if any) to free memory
				Destroy( croppedImageHolder.texture, 5f );

				// If screenshot was cropped successfully
				if( result )
				{
					// Assign cropped texture to the RawImage
					croppedImageHolder.enabled = true;
					croppedImageHolder.texture = croppedImage;

					Vector2 size = croppedImageHolder.rectTransform.sizeDelta;
					if( croppedImage.height <= croppedImage.width )
						size = new Vector2( 400f, 400f * ( croppedImage.height / (float) croppedImage.width ) );
					else
						size = new Vector2( 400f * ( croppedImage.width / (float) croppedImage.height ), 400f );
					croppedImageHolder.rectTransform.sizeDelta = size;

				}
				else
				{
					croppedImageHolder.enabled = false;
				}

				// Destroy the screenshot as we no longer need it in this case
				Destroy( screenshot );
			},
			settings: new ImageCropper.Settings()
			{
				imageBackground = Color.clear, // transparent background
				selectionMinAspectRatio = minAspectRatio,
				selectionMaxAspectRatio = maxAspectRatio

			},
			croppedImageResizePolicy: ( ref int width, ref int height ) =>
			{
				// uncomment lines below to save cropped image at half resolution
				//width /= 2;
				//height /= 2;
			} );

#if UNITY_EDITOR
            ImageCropper.Instance.FlipVertical();

            ImageCropper.Instance.Selection.sizeDelta = new Vector2(365, 365);
            ImageCropper.Instance.Selection.anchoredPosition = new Vector2(0 - ImageCropper.Instance.Selection.sizeDelta.x/2, 0 - ImageCropper.Instance.Selection.sizeDelta.y/2);
#endif

#if !UNITY_EDITOR
            if (image != null)
            {
                Debug.Log("Unity Image + " + "image : " + image);
                Debug.Log("Unity Image + " + "image.ExtentX : " + image.ExtentX);
                Debug.Log("Unity Image + " + "ImageCropper.Instance.Selection.sizeDelta : " + ImageCropper.Instance.Selection.sizeDelta);
                Debug.Log("Unity Image + " + "ImageCropper.Instance.Selection.anchoredPosition : " + ImageCropper.Instance.Selection.anchoredPosition);
                Debug.Log("Unity Image + " + "image.CenterPose.position.x : " + image.CenterPose.position.x);

                //ImageCropper.Instance.FlipVertical();
                ImageCropper.Instance.Selection.sizeDelta = new Vector2(image.ExtentX, image.ExtentZ);
                ImageCropper.Instance.Selection.anchoredPosition = new Vector2(image.CenterPose.position.x - image.ExtentX / 2, image.CenterPose.position.z - image.ExtentZ / 2);
            }
            else
            {
                Debug.Log("Unity Image + " + "image : " + image);
                Debug.Log("Unity Image + " + "image.ExtentX : " + image.ExtentX);
                Debug.Log("Unity Image + " + "ImageCropper.Instance.Selection.sizeDelta : " + ImageCropper.Instance.Selection.sizeDelta);
                Debug.Log("Unity Image + " + "ImageCropper.Instance.Selection.anchoredPosition : " + ImageCropper.Instance.Selection.anchoredPosition);
                Debug.Log("Unity Image + " + "image.CenterPose.position.x : " + image.CenterPose.position.x);

                ImageCropper.Instance.FlipVertical();
                ImageCropper.Instance.SelectionGraphics.sizeDelta = new Vector2(365, 365);
                ImageCropper.Instance.SelectionGraphics.anchoredPosition = new Vector2(0 - ImageCropper.Instance.Selection.sizeDelta.x / 2, 0 - ImageCropper.Instance.Selection.sizeDelta.y / 2);
                ImageCropper.Instance.Selection.sizeDelta = new Vector2(365, 365);
                ImageCropper.Instance.Selection.anchoredPosition = new Vector2(0 - ImageCropper.Instance.Selection.sizeDelta.x / 2, 0 - ImageCropper.Instance.Selection.sizeDelta.y / 2);
            }
#endif


            //ImageCropper.Instance.Crop();

        }

    }


}