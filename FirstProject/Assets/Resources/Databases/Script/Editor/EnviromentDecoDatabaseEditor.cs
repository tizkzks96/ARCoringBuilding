using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnviromentDecoDatabaseEditor
{
    private static string GetSavePath()
    {
        return EditorUtility.SaveFilePanelInProject("New enviroment deco database", "New enviroment deco database", "asset", "Create a new enviroment deco database.");
    }

    [MenuItem("Assets/Create/Databases/enviroment deco Database")]
    public static void CreateDatabase()
    {
        string assetPath = GetSavePath();
        EnviromentDecoDatabase asset = ScriptableObject.CreateInstance("EnviromentDecoDatabase") as EnviromentDecoDatabase;  //scriptable object
        AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        AssetDatabase.Refresh();
    }
}
