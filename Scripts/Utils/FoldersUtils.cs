#if UNITY_EDITOR
using UnityEditor;
#endif

using System.IO;
using UnityEngine;

public static class FolderUtils
{
#if UNITY_EDITOR
    public static void EnsureDirectoryExists(string filePath)
    {
        try
        {
            // Get the directory path (remove the filename)
            string directoryPath = Path.GetDirectoryName(filePath);

            // Check if directory exists
            if (!Directory.Exists(directoryPath))
            {
                Debug.Log($"Creating directory: {directoryPath}");

                // Create all necessary directories
                Directory.CreateDirectory(directoryPath);

                // Refresh AssetDatabase to recognize new folders
                AssetDatabase.Refresh();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to create directory for path: {filePath}\nError: {ex.Message}");
        }
    }
#endif
}