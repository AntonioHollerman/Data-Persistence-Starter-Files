
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataFlowManager : MonoBehaviour
{
    [System.Serializable]
    public class HighScore
    {
        public string Name { get; private set; }
        public int Score { get; private set; }

        public HighScore(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }

    public static DataFlowManager Instance { get; private set; }
    
    public HighScore CurrentHighScore { get; private set; }
    public string PlayerName { get; private set; }
    
    public GameObject nameEntryGameObject;
    private InputField _nameField;

    private void Start()
    {
        _nameField = nameEntryGameObject.GetComponent<InputField>();
    }

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
    }

    public void SaveHighScore(string playerName, int newScore)
    {
        if (CurrentHighScore != null && CurrentHighScore.Score > newScore)
        {
            return;
        }

        HighScore data = new HighScore(playerName, newScore);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        CurrentHighScore = data;
    }

    private void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScore data = JsonUtility.FromJson<HighScore>(json);

            CurrentHighScore = data;
        }
        else
        {
            CurrentHighScore = new HighScore("Null", 0);
        }
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
                Application.Quit(); // original code to quit Unity player
        #endif
    }

    public void Play()
    {
        string playerName = _nameField.text;
        if (playerName.Length == 0)
        {
            _nameField.text = "Enter Name";
            return;
        }

        PlayerName = playerName;
        SceneManager.LoadScene(1);
    }
}
