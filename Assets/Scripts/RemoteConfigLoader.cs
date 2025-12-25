using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class RemoteConfigLoader : MonoBehaviour
{
    private readonly int _timeout = 10;
    private string _errorLog = "";
    
    public LocalConfigManager lcm;
    public string docURL =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vSiMgW_enegx5QBgZZKSuAFZzEtrloild_T4--g01a81ufSuvRyAIQFwOl6PShbbskdcyhNzuRE3vnq/pub?gid=0&single=true&output=csv";
    public TextMeshProUGUI[] text;
    public TextMeshProUGUI errorText;

    private void Awake()
    {
        errorText.text = "";
    }

    public void OnLoadConfigButtonClicked()
    {
        StartCoroutine(LoadConfig());
    }

    public void OnDeleteConfigButtonClicked()
    {
        lcm.DeleteLocalCopy();
        UpdateUI("Local config deleted\n");
    }

    IEnumerator LoadConfig()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(docURL))
        {
            www.timeout = _timeout;
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                UpdateUI("Error while getting CSV config, trying to load local version...\n" + www.error + "\n");
                LocalConfigLoader();
                yield break;
            }
            
            string csv = www.downloadHandler.text;

            ApplyConfigCsv(csv);
            UpdateUI("CSV config loaded successfully!\n");
            
            if (lcm.HasLocalCopy())
            {
                lcm.DeleteLocalCopy();
            }
            
            lcm.SaveLocalCopy(csv);
            UpdateUI("Local config saved successfully!\n");
        }
    }

    void UpdateUI(string message)
    {
        _errorLog += message;
        errorText.text = _errorLog;
    }

    void LocalConfigLoader()
    {
        if (lcm.HasLocalCopy())
        {
            string data = lcm.LoadLocalCopy();
            ApplyConfigCsv(data);
            UpdateUI("Local config loaded successfully!\n");
        }
        else
        {
            UpdateUI($"Error while loading local config\n" + "Using default values...\n");
        }
    }

    void ApplyConfigCsv(string config)
    {
        if (config != null)
        {
            int index = 0;
            foreach (string line in config.Split('\n'))
            {
                text[index].text = string.Join(": ", line.Split(','));
                index += 1;
            }
        }
    }
}