using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class GroundDatabaseEditor {


    private static string GetSavePath()
    {
        return EditorUtility.SaveFilePanelInProject("New ground database", "New ground database", "asset", "Create a new ground database.");
    }

    [MenuItem("Assets/Create/Databases/ground Database")]
    public static void CreateDatabase()
    {
        string assetPath = GetSavePath();
        GroundDatabase asset = ScriptableObject.CreateInstance("GroundDatabase") as GroundDatabase;  //scriptable object
        AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        AssetDatabase.Refresh();
    }
}
