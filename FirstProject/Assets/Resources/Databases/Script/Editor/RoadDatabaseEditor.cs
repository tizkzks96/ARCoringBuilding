using System.Collections;
using UnityEditor;
using UnityEngine;

public class RoadDatabaseEditor
{
    private static string GetSavePath()
    {
        return EditorUtility.SaveFilePanelInProject("New road database", "New road database", "asset", "Create a new road database.");
    }

    [MenuItem("Assets/Create/Databases/road Database")]
    public static void CreateDatabase()
    {
        string assetPath = GetSavePath();
        RoadDatabase asset = ScriptableObject.CreateInstance("RoadDatabase") as RoadDatabase;  //scriptable object
        AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        AssetDatabase.Refresh();
    }
}
