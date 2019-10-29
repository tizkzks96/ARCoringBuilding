using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAnotherApp : Singleton<OpenAnotherApp>
{
    string temp = "com.Yong.Card";
    public void OpenUrl(string packageName)
    {
#if UNITY_ANDROID
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject pManager = unityActivity.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject intent = null;
        try
        {
            intent = pManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", packageName);
            unityActivity.Call("startActivity", intent);
            //return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to Opeen App: " + e.Message);
            //Open with Browser
            string link = "https://play.google.com/store/apps/details?id=" + packageName + "&hl=en";

            Application.OpenURL(link);
            //return false;
        }

#endif
    }
}
