using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine;

public class BuildableObjects : Construction
{
    private BuildingSystem buildingSystem;
    private GameManager gameManager;
    [SerializeField]
    GameObject materialObject;//user feedback material game object
    [SerializeField]
    private ParticleSystem buildAffect;//user feedback building particles
    [SerializeField]
    private int initialPoints;//this building`s points 

    private int calculatedPoints;//overall calculated points
    [SerializeField]
    protected List<Construction> nearbyObjects;//list of objects next to this building(objects will be added by trigger entering)
    protected CommunicationValue comunicationValue;//with which types is building connected
   
    [SerializeField]
    private GameObject isOccupied;//gameobject to check is building place occupied
    private Color tempColor;//temporary color 
    [SerializeField]
    private Color notAllowedToBuildColor;//is occupied color

    [SerializeField]
    private int idNum;//building object`s id num for buttons
    #region properties
    public int IdNum { get => idNum; private set => idNum = value; }
    public ParticleSystem BuildAffect { get => buildAffect;private set => buildAffect = value; }
    public int DecreaseIDNum() => idNum--;
    #endregion
    ScoreManager scoreManager;
    private void OnEnable()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }
    /// <summary>
    /// function for building this game object
    /// </summary>
    /// <returns></returns>
    public bool Build()
    {
        //check for state of building
        if (CurrentStates == States.canBuild)
        {
            AudioManager._instance.PlaySFX("18365_1464637302");//play sfx
            BuildAffect.Play();//give user feedback about this buildable object is built now 
            gameObject.GetComponent<BoxCollider>().enabled = true;//enable parent object`s collider
            Destroy(gameObject.GetComponent<SphereCollider>());//destroy distance feedback object
            //add to score
            scoreManager.AddScore(calculatedPoints);
            //deactivate material object
            materialObject.SetActive(false);
            //change its state
            CurrentStates = States.builded;
            //check is current score enough to open new level
            LevelManager.Instance.CalculateScoreToOpenNewLevel();
            //destroy occupied check gameobject
            Destroy(isOccupied);
            //hide nearby objects text meshes
            DeactivateNearbyObjectTextMeshes();
            //clear nearby objects list
            nearbyObjects.Clear();
            //add rigidbody component for further checkings
            gameObject.AddComponent<Rigidbody>().isKinematic = true;
            return true;
        }
        return false;

    }
    List<LevelButtonData> data;//buildable object button datas
    /// <summary>
    /// this function is for 
    /// deducting amount of allowed building buttons 
    /// </summary>
    /// <returns></returns>
    public int Decrease()
    {
        //get data from level manager
        data = LevelManager.Instance.CurrentLevel.levelDatas;
        //compare to parent type
        if (OwnType == data[IdNum].type)
        {
            //deduct required amount of buttons in this type of building 
            return data[IdNum].requiredAmount--;
            
        }
        return 0;
    }
    /// <summary>    
    /// this function is for 
    /// deactivating building buttons 
    /// </summary>
    /// <param name="g"></param>
    public void DeleteButton(GameObject g)
    {
        //get data
        data = LevelManager.Instance.CurrentLevel.levelDatas;
        //compare left amount of building buttons to 0
        if (data[IdNum].requiredAmount <= 0)
        {
            //deduct allowed number of buttons
            gameManager.SetAllowedNumberOfButtonsLeft();
            //deactivate this button
            g.SetActive(false);
            //check for game over
            gameManager.GameOverCheck();
        }
    }
    /// <summary>
    /// this function is for 
    /// deactivating text mesh components 
    /// </summary>
    void DeactivateNearbyObjectTextMeshes()
    {
        //check current state
        if (CurrentStates == States.builded)
        {
            //search for  nearby objects
            for (int i = 0; i < nearbyObjects.Count; i++)
            {
                //make font size smaller
                nearbyObjects[i].ScoreTextMesh.fontSize = 50;
                //deactivate text mesh object
                nearbyObjects[i].ScoreTextMesh.gameObject.SetActive(false);
            }
            //deactivate text mesh object 
            ScoreTextMesh.gameObject.SetActive(false);
            //make font size smaller
            ScoreTextMesh.fontSize = 50;
        }

    }
    protected override void Start()
    {
        base.Start();//check for parent class
        //get starting color
        tempColor = materialObject.GetComponent<MeshRenderer>().material.color;
        //set score text
        ScoreTextMesh.text = initialPoints.ToString();
        buildingSystem = FindObjectOfType<BuildingSystem>();
        gameManager = FindObjectOfType<GameManager>();
    }
    /// <summary>
    /// when object collides with another object
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerEnter(Collider other)
    {
        //check for layer
        if (other.gameObject.layer == 10)
        {
            //get buildable object from entering object
            Construction buildable = other.GetComponent<Construction>();
            if (buildable == null)//check if it is not null
                return;//early return, if there is no construction object
            //check for states
            if (CurrentStates == States.canBuild||CurrentStates.Equals(States.cannotBuild))
            {
                //check types list contains owner type
                if (types.Contains(buildable.OwnType))
                {
                    nearbyObjects.Add(buildable);//add to nearby objects list
                    Calculate();//calculate their score
                }
            }
            //check for states
            else if (CurrentStates == States.builded)
            {
                //activate text mesh object
                ScoreTextMesh.gameObject.SetActive(true);
                //update text mesh object 
                ScoreTextMesh.text = gameManager.ConnectionValues.GetValueByTypes(OwnType, buildable.OwnType).ToString();
            }

        }
    }
    /// <summary>
    /// when object leaves another collision object 
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerExit(Collider other)
    {
        //check for layer
        if (other.gameObject.layer == 10)
        {
            //get buildable object
            Construction buildable = other.GetComponent<Construction>();
            if (buildable == null)//check for buildable object
                return;//early return
            //check for states
            if (CurrentStates == States.canBuild||CurrentStates.Equals(States.cannotBuild))
            {
                //check for types
                if (types.Contains(buildable.OwnType))
                {
                    //remove from the list 
                    nearbyObjects.Remove(buildable);
                    //calculate bonus points
                    Calculate();
                }


            }
            //check for state
            else if (CurrentStates == States.builded)
            {
                //deactivate score text mesh object
                ScoreTextMesh.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// function for calculating
    /// bonus points
    /// </summary>
    /// <returns></returns>
    int Calculate()
    {
        calculatedPoints=0;//initial value
        //search for nearby objects
        for (int i = 0; i < nearbyObjects.Count; i++)
        {
            //calculate nearby objects value
            calculatedPoints += gameManager.ConnectionValues.GetValueByTypes(OwnType, nearbyObjects[i].OwnType);
        }
        //add initial bonus points
        calculatedPoints += initialPoints;
        //update score text mesh
        ScoreTextMesh.text = calculatedPoints.ToString();
        //return score
        return calculatedPoints;
    }
    /// <summary>
    /// function for setting 
    /// state of this current buildable object
    /// </summary>
    /// <param name="state">state to change</param>
    public void SetState(States state)
    {
        CurrentStates = state;//set current state to new state
        if (CurrentStates == States.cannotBuild)//check for state
            materialObject.GetComponent<MeshRenderer>().material.color = notAllowedToBuildColor;//chenge material color
        else
            materialObject.GetComponent<MeshRenderer>().material.color = tempColor;//change material color
    }
    /// <summary>
    /// function runs
    /// when user drags this object
    /// </summary>
    private void OnMouseDrag()
    {
        //check user is dragging this object
        if (buildingSystem.CurrentBuildableObject != null && buildingSystem.CurrentBuildableObject == this)
        {
            //start calling on drag action
            buildingSystem._aOnDrag(this);
        }
    }
    /// <summary>
    /// function runs
    /// when user ends dragging
    /// </summary>
    private void OnMouseUp()
    {
        //start calling end drag action
        buildingSystem._aOnEndDrag(this);
    }
}

