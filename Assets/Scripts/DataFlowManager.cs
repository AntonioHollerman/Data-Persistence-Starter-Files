
using System.IO;
using UnityEngine;
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
    public HighScore highScore;
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
        if (highScore != null && highScore.Score > newScore)
        {
            return;
        }

        HighScore data = new HighScore(playerName, newScore);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        highScore = data;
    }

    private void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScore data = JsonUtility.FromJson<HighScore>(json);

            highScore = data;
        }
    }
}
