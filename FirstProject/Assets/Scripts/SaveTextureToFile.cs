using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveTextureToFile : MonoBehaviour
{
    public Texture2D txtToSave;
    public string dirName = "Saved2D_Textures";
    public string fileName = "textToPng";

    //  THIS CAN BE DELETED ITS JUST SHOWING FOLDER LOCATION 
    //  Delete Zone Start

    [Header("Copy and paste into a folder address bar to get to the location of where the textures are saving to")]
    public string fileLoc;

    public SaveTextureToFile(Texture2D txtToSave, string dirName, string fileName)
    {
        this.txtToSave = txtToSave;
        this.dirName = dirName;
        this.fileName = fileName;
    }

    public void OnDrawGizmos()
    {
        fileLoc = Application.persistentDataPath + "/" + dirName + "/";
    }

    //  Delete Zone Finish


    public void SaveTxt2D(Texture2D savedTexture, string dirPath, string fileName)
    {
        if (txtToSave != null)
        {
            print("1 : " + txtToSave);
            print("1 : " + dirName);
            print("1 : " + fileName);
            StartCoroutine(STTF(txtToSave, dirName, fileName));
        }
    }

    public int DirCount(DirectoryInfo dir)
    {
        int i = 0;
        // Add file sizes.
        FileInfo[] fiArr = dir.GetFiles();
        foreach (FileInfo fi in fiArr)
        {
            i++;
        }
        return i;
    }

    IEnumerator STTF(Texture2D savedTexture, string dirPath, string fileName)
    {
        print("2 : " + txtToSave);
        print("2 : " + dirName);
        print("2 : " + fileName);
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        DirectoryInfo di = new DirectoryInfo(dirPath);
        // this is to give Unique name so we dont overWrite providing no files have deleted 
        int dc = DirCount(di);
        fileName = fileName + dc;
        Byte[] bytes = savedTexture.EncodeToPNG();
        string fileLocation = Application.persistentDataPath + "/" + dirPath + "/" + fileName;
        File.WriteAllBytes(fileLocation, bytes);
        yield return null;

        print("3 : " + txtToSave);
        print("3 : " + dirName);
        print("3 : " + fileName);
    }
}
