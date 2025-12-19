using System.IO;
using UnityEditor;
using UnityEngine;

public static class UiAnimationScriptableObjectFactory
{
    public static T Create<T>(string assetName = null, string path = "") where T : ScriptableObject
    {
        T instance = ScriptableObject.CreateInstance<T>();
        
        #if UNITY_EDITOR
        if (!string.IsNullOrEmpty(assetName))
        {
            path += assetName + ".asset";

            FolderUtils.EnsureDirectoryExists(path);

            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.SaveAssets();
        }
        #endif
        
        return instance;
    }

      
   
}