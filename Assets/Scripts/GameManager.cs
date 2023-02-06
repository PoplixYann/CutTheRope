using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameManager");
                GameManager gm = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
                instance = gm;
            }

            return instance;
        }
    }

    LevelManager curLevelManager = null;
    public LevelManager CurLevelManager
    {
        get
        {
            if (curLevelManager == null)
            {
                CreateLevelManager();
            }
            return curLevelManager;
        }
    }

    public List<Level> levelList = new List<Level>();
    public int curLevel = 0;

    public bool isPause = false;
    bool isMenu = true;

    string levelsSavePath;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);

        Application.targetFrameRate = 60;

        levelsSavePath = Path.Combine(Application.persistentDataPath, "save.txt");

        LoadLevels();
    }

    void Start()
    {
        SoundPlayer sp = GetComponent<SoundPlayer>();
        sp.PlaySound("MainMusic");
    }

    void Update()
    {
        if (!SoundManager.Instance.MusicIsPlaying())
        {
            SoundPlayer sp = GetComponent<SoundPlayer>();
            if (isMenu)
            {
                sp.PlaySound("MainMusic");
            }
            else
            {
                sp.PlaySound("LevelMusic");
            }
        }
    }

    void CreateLevelManager()
    {
        GameObject go = new GameObject("LevelManager");
        LevelManager lm = go.AddComponent<LevelManager>();
        curLevelManager = lm;
    }

    public void PlayLevel(int id)
    {
        curLevel = id;

        isMenu = false;
        SoundPlayer sp = GetComponent<SoundPlayer>();
        sp.PlaySound("LevelMusic");

        Transition.Instance.PlayTransition(levelList[id].scene);
    }

    public void ReplayLevel()
    {
        SoundPlayer sp = GetComponent<SoundPlayer>();
        sp.PlaySound("FailLevel");

        PlayLevel(curLevel);
    }

    public void PlayNextLevel()
    {
        SoundPlayer sp = GetComponent<SoundPlayer>();
        sp.PlaySound("EndLevel");

        if (curLevel + 1 >= levelList.Count)
        {
            GoToPlayMenu();
            return;
        }

        PlayLevel(curLevel + 1);
    }

    public void GoToPlayMenu()
    {
        SoundPlayer sp = GetComponent<SoundPlayer>();
        sp.PlaySound("MainMusic");
        isMenu = true;
        Transition.Instance.PlayTransition("PlayMenu");
    }

    public void GoToMainMenu()
    {
        SoundPlayer sp = GetComponent<SoundPlayer>();
        sp.PlaySound("MainMusic");
        isMenu = true;
        Transition.Instance.PlayTransition("MainMenu");
    }

    public void GoToOptionMenu()
    {
        Transition.Instance.PlayTransition("OptionMenu");
    }

    public void UnlockNextLevel()
    {
        if (curLevelManager.StarsCount > levelList[curLevel].nbStars)
        {
            levelList[curLevel].nbStars = curLevelManager.StarsCount;
        }

        if (curLevel + 1 < levelList.Count)
        {
            levelList[curLevel + 1].isUnlock = true;
        }

        SaveLevels();
    }

    void SaveLevels()
    {
        string path = levelsSavePath;
        StreamWriter streamWriter = new StreamWriter(path);
        if (streamWriter == null)
        {
            Debug.Log("Error : Can't write in " + path);
            return;
        }

        for (int i = 0; i < levelList.Count; i++)
        {
            string line = i.ToString() + "," + levelList[i].isUnlock.ToString() + "," + levelList[i].nbStars.ToString();
            streamWriter.WriteLine(line);
        }

        streamWriter.Close();
    }

    void LoadLevels()
    {
        string path = levelsSavePath;
        if (!File.Exists(path))
        {
            return;
        }
        StreamReader streamReader = new StreamReader(path);
        if (streamReader == null)
        {
            Debug.Log("Error : Can't read in " + path);
            return;
        }

        for (int i = 0; i < levelList.Count; i++)
        {
            string line = streamReader.ReadLine();

            string[] info = line.Split(',');

            if (int.Parse(info[0]) == i)
            {
                levelList[i].isUnlock = bool.Parse(info[1]);
                levelList[i].nbStars = int.Parse(info[2]);
            }

            if (streamReader.EndOfStream)
            {
                break;
            }
        }

        streamReader.Close();
    }

    public void DeleteLevelsSave()
    {
        string path = levelsSavePath;
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        levelList[0].isUnlock = true;
        levelList[0].nbStars = 0;
        for (int i = 1; i < levelList.Count; i++)
        {
            levelList[i].isUnlock = false;
            levelList[i].nbStars = 0;
        }
    }
}
