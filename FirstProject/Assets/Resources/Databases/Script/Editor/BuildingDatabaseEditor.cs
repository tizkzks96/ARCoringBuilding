using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class BuildingDatabaseEditor {


    private static string GetSavePath()
    {
        return EditorUtility.SaveFilePanelInProject("New building database", "New building database", "asset", "Create a new building database.");
    }

    [MenuItem("Assets/Create/Databases/building Database")]
    public static void CreateDatabase()
    {
        string assetPath = GetSavePath();
        BuildingDatabase asset = ScriptableObject.CreateInstance("BuildingDatabase") as BuildingDatabase;  //scriptable object
        AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        AssetDatabase.Refresh();
    }
}
