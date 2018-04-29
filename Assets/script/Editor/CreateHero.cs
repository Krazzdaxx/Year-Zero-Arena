using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateHero : MonoBehaviour {

    [MenuItem("Assets/Create/HERO STATS")]
    public static void CreateMyAsset()
    {
        CharacterStats asset = ScriptableObject.CreateInstance<CharacterStats>();

        AssetDatabase.CreateAsset(asset, "Assets/NewHeroStats.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
