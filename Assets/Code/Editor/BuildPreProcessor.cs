using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildPreprocessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    
    public void OnPreprocessBuild(BuildReport report)
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs have been reset before build.");
        
        string savePath = Application.persistentDataPath + "/saveData.json";
        if (System.IO.File.Exists(savePath))
        {
            System.IO.File.Delete(savePath);
            Debug.Log("Save file deleted before build: " + savePath);
        }
    }
}