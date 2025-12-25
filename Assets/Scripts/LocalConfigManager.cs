using System.IO;
using UnityEngine;

public class LocalConfigManager : MonoBehaviour
{
    public void SaveLocalCopy(string configData, string fileName = "config_backup.txt")
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        
        try
        {
            File.WriteAllText(filePath, configData);
            Debug.Log($"Config saved: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error while saving: {e.Message}");
        }
    }
    
    public string LoadLocalCopy(string fileName = "config_backup.txt")
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        
        if (File.Exists(filePath))
        {
            try
            {
                string configData = File.ReadAllText(filePath);
                Debug.Log($"Config loaded from: {filePath}");
                return configData;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error while loading: {e.Message}");
                return null;
            }
        }
        else
        {
            Debug.Log($"Local config not found: {filePath}");
            return null;
        }
    }
    
    public void DeleteLocalCopy(string fileName = "config_backup.txt")
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"Config Deleter: {filePath}");
        }
    }
    
    public bool HasLocalCopy(string fileName = "config_backup.txt")
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        return File.Exists(filePath);
    }
}
