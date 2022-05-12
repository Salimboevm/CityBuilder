using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region singleton
    private static LevelManager instance;

    public static LevelManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    #endregion
    //accessor and action caller
    public bool OpenNewLevel { get => openNewLevel;private set 
        {
            openNewLevel = value;
            //_aOpenNewLevelAction.Invoke();
        }
    }
    #region properties
    public Level CurrentLevel { get => currentLevel;private set => currentLevel = value; }
    public static int CurrentLevelId { get => currentLevelId;private set => currentLevelId = Mathf.Clamp(value,0,lvlCount); }
    public List<Level> Levels { get => levels;private set => levels = value; }
    public bool TutorialIsGoing { get => tutorialIsGoing;private set => tutorialIsGoing = value; }
    public bool SetTutorialIsGoing(bool v) => tutorialIsGoing = v;
    public ScoreManager SetScoreManager(ScoreManager s) => scoreManager = s;
    public UIManager SetUIManager(UIManager u) => uIManager = u;
    #endregion
    [SerializeField]
    private string nameOfMusic;//name of bg music
    private void Awake()
    {
        if (instance == null) 
        { 
            instance = this;//set singleton
        }
        lvlCount = levels.Count;//set levels count
        SceneManager.sceneLoaded += OnSceneLoaded;//set level scene
    }
    private static int currentLevelId=0;//current level id
    static int lvlCount;//levels amount
    private bool openNewLevel;//new level open check
    [SerializeField]
    private List<Level> levels;//list of levels
    private Level currentLevel;//current active level
    private bool tutorialIsGoing = false;
    //public Action _aOpenNewLevelAction;//new level open check action
    private ScoreManager scoreManager;
    private UIManager uIManager;

    private void Start()
    {
        AudioManager._instance.PlayMusic(nameOfMusic);//start bg music
    }
    /// <summary>
    /// when scene loaded
    /// create level
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sceneMode"></param>
    void OnSceneLoaded(Scene id,LoadSceneMode sceneMode)
    {
        //set current level to work with
        currentLevel = Instantiate(Levels[CurrentLevelId].gameObject).GetComponent<Level>();
    }
    /// <summary>
    /// create next level
    /// </summary>
    public void NextLevel()
    {
        CurrentLevelId++;//increase current level id
        
        SceneManager.LoadScene(1);//load scene
    }
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
    /// <summary>
    /// score calculator
    /// to open new level
    /// </summary>
    public void CalculateScoreToOpenNewLevel()
    {
        //check score
        if (scoreManager.Score >= Levels[CurrentLevelId].MaxBonusToOpenNewLevel)
        {
            OpenNewLevel = true;//open new level
            uIManager.ChangeOpenNextLevelButtonInteractable();
        } 
    }
    //o`rniga max scoredan o`tsa affect bervorish
    //start knopka qo`shib qo`yish
    //tips tutorial system+
}
[System.Serializable]
public class LevelButtonData
{
    public Types type;//buildin type
    public int requiredAmount;//how many building button should be in the level
    public Sprite image;//sprite image of the object
}