using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;//score text object
    [SerializeField]
    private GameObject askToBuild;//ask from user to build
    [SerializeField]
    private GameObject buttonDragPrefab;//prefab object of draggable button
    [SerializeField]
    private Transform parent;//parent object to create draggable buttons 
    [SerializeField]
    BuildingSystem buildingInstance;//instance of building system
    [SerializeField]
    private Button nextLevelOpenButton;//next level loader button
    [SerializeField]
    private Slider nextLevelButtonSlider;//its slider value
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject gameOverPanel;//game over
    #region properties
    public Text ScoreText { get => scoreText;private set => scoreText = value; }
    public GameObject AskToBuildObject { get => askToBuild;private set => askToBuild = value; }
    public GameObject ButtonDragPrefab { get => buttonDragPrefab;private set => buttonDragPrefab = value; }
    public Slider NextLevelButtonSlider { get => nextLevelButtonSlider;private set => nextLevelButtonSlider = value; }

    #endregion
    ScoreManager scoreManager;
    private void OnEnable()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }
    private void Start()
    {
        LevelManager.Instance.SetUIManager(this);
        scoreManager.onScoreChanged += UpdateScore;//attach update score into action
        print(NextLevelButtonSlider);
        ScoreText.text = " 0 ";//set initial value
        SetUIDragableButtons();//create building object buttons
        nextLevelButtonSlider.maxValue = LevelManager.Instance.CurrentLevel.MaxBonusToOpenNewLevel;//set slider value
        //LevelManager.Instance._aOpenNewLevelAction += ChangeOpenNextLevelButtonInteractable;//attach new level opening action
        gameManager.gameOverAction+=GameOver;//attach game over action
    }
    /// <summary>
    /// function for creating building object buttons
    /// </summary>
    void SetUIDragableButtons()
    {
        //get level manager button data
        List<LevelButtonData> levelManager = LevelManager.Instance.CurrentLevel.levelDatas;
        //get building button data
        List<BuildingData> buildingDatas = buildingInstance.BuildingDatas;
        //loop and count how many buttons should be created
        for (int i = 0; i < levelManager.Count; i++)
        {
            //create button with showing parent object
            ButtonDragHandler g = Instantiate(ButtonDragPrefab, parent).GetComponent<ButtonDragHandler>();
            
            // check for type 
            if(levelManager[i].type == buildingDatas[i].type)
            {
                //set this created object`s drag handler
                g.SetCurrentBuildableObject(buildingDatas[i].prefab);
             //change sprites and text value from level data
                g.GetComponent<Button>().image.sprite = levelManager[i].image;
                g.MaxNumberAllowedToBuild.text = levelManager[i].requiredAmount.ToString();
            }
            //add it to the list of building buttons
            buildingInstance.buildingButtons.Add(g.GetComponent<Button>());
        }
    }
    /// <summary>
    /// func for updating score
    /// </summary>
    /// <param name="score"></param>
    void UpdateScore(int score)
    {
        //update score
        ScoreText.text = score.ToString();
        //update slider value
        nextLevelButtonSlider.value = Mathf.Clamp(score,0,LevelManager.Instance.CurrentLevel.MaxBonusToOpenNewLevel);
    }
    /// <summary>
    /// control function for opening new level button
    /// </summary>
    /// <param name="t"></param>
    public void ChangeOpenNextLevelButtonInteractable()
    {
        //make open new level button pressable
        print(NextLevelButtonSlider);
        nextLevelButtonSlider.gameObject.SetActive(false);
        print(nextLevelOpenButton);
        nextLevelOpenButton.interactable = true;
        //start particle effects around the button
        
    }
    
    /// <summary>
    /// func to ask from the user
    /// if user really wants to build current object or not 
    /// </summary>
    /// <param name="worldPos">world position for panel object to activate on top of current buildable object</param>
    public void AskToBuild(Vector3 worldPos)
    {
        worldPos.z = 0f;
        AskToBuildObject.SetActive(true);
        
        buildingInstance.SetLastPosition(true);
    }
    /// <summary>
    /// when user presses yes 
    /// then build game object and call build func
    /// </summary>
    public void LetMeBuild()
    {
        bool temp = buildingInstance.CurrentBuildableObject.Build();
        
        if (buildingInstance.CurrentBuildableObject.CurrentStates == States.builded)
        {
            buildingInstance.SetInProgress(false);
            buildingInstance.SetLastPosition(false);
            buildingInstance.SetInteractable();
            buildingInstance.CurrentBuildableObject.Decrease();
            if (LevelManager.Instance.CurrentLevel.levelDatas[buildingInstance.CurrentBuildableObject.IdNum].requiredAmount > 0)
                buildingInstance.buildingButtons[buildingInstance.CurrentBuildableObject.IdNum].GetComponent<ButtonDragHandler>().MaxNumberAllowedToBuild.text = LevelManager.Instance.CurrentLevel.levelDatas[buildingInstance.CurrentBuildableObject.IdNum].requiredAmount.ToString();
            else
            {
                buildingInstance.CurrentBuildableObject.DeleteButton(buildingInstance.buildingButtons[buildingInstance.CurrentBuildableObject.IdNum].gameObject);
            }
            AskToBuildObject.SetActive(!temp);
            buildingInstance.SetCurrentBuildableObject(null);
        }
    }
    /// <summary>
    /// when user presses no 
    /// then just clean visual building type
    /// </summary>
    public void DoNotBuildIt()
    {
        buildingInstance.Clear();//clear everything and reset
        buildingInstance.SetLastPosition(false);//user is not building this object
        AskToBuildObject.SetActive(false);//deactivate asking
    }
    /// <summary>
    /// function for controlling game over panel
    /// </summary>
    private void GameOver()
    {
        //show game over
        gameOverPanel.SetActive(true);
    }
    /// <summary>
    /// restart function
    /// </summary>
    public void Restart()
    {
        //reload this scene
        LevelManager.Instance.Restart();
    }
    /// <summary>
    /// fucntion to open next lvl
    /// </summary>
    public void NextLevel()
    {
        LevelManager.Instance.NextLevel();
    }
}