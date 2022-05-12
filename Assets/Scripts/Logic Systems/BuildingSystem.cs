using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{

    private BuildableObjects currentBuildableObject;//current building object type
    public List<UnityEngine.UI.Button> buildingButtons;//building buttons
    private bool lastPosition=false;//is user decided to build this object
    [SerializeField]
    private List<BuildingData> buildingDatas = new List<BuildingData>();//types of buildings 
    private bool inProgress = false;//is user building
    Transform visual;//current building type`s visual gameobject 
    [SerializeField]
    private int accuracy;//building moving accuracy
    [SerializeField]
    private UIManager uIManager;
    public Action <BuildableObjects> _aOnDrag, _aOnEndDrag;//actions for button presses
    #region properties
    public List<BuildingData> BuildingDatas { get => buildingDatas;private set => buildingDatas = value; }
    public bool LastPosition { get => lastPosition;private set => lastPosition = value; }
    public BuildableObjects CurrentBuildableObject { get => currentBuildableObject;private set => currentBuildableObject = value; }
    public bool InProgress { get => inProgress; }
    public bool SetInProgress(bool v) => inProgress = v;
    public bool SetLastPosition(bool v) => lastPosition = v;
    public BuildableObjects SetCurrentBuildableObject(BuildableObjects v) => currentBuildableObject = v;
    #endregion
    private void Start()
    {
        _aOnDrag += OnButtonDragMoveObject;//connect action
        _aOnEndDrag += OnEndDragButton;//connect action
    }
    /// <summary>
    /// function to convert user input position to world position
    /// </summary>
    /// <returns></returns>
    public Vector3 MouseToWorld()
    {
        Vector2 mousePosition = Input.mousePosition;//user`s input position
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);//convert input position to ray 
        //raycast and check
        if (Physics.Raycast(ray, out RaycastHit hit, 9999999, LayerMask.GetMask("ground")) && Utils.UITouchCheck.IsPointerOverUI(mousePosition.x, mousePosition.y))
        {
            //user hit ground
            return new Vector3(((int)(hit.point.x / accuracy)) * accuracy, ((int)hit.point.y/accuracy) * accuracy, ((int)hit.point.z/accuracy)*accuracy);
        }
        else
        {
            //user did not hit ground
            return Vector3.zero;
        }
    }
    /// <summary>
    /// refresh visual 
    /// create visual object for current building object
    /// </summary>
    /// <param name="prefab">type of building </param>
    /// <returns></returns>
    public BuildableObjects RefreshVisual(BuildableObjects prefab)
    {
        Vector3 hitPoint = MouseToWorld();//hit position point
        if(hitPoint.Equals(Vector3.zero))//check hit point is actually hitting ground
        {
            //if not early return
            return null;
        }
        //create visual object
        visual = Instantiate(prefab, MouseToWorld(), Quaternion.identity).transform;
        //return created visual object
        return currentBuildableObject = visual.gameObject.GetComponent<BuildableObjects>();
    }
    /// <summary>
    /// function to clear current buildable object
    /// </summary>
    public void Clear()
    {
        //destroy visual 
        Destroy(visual.gameObject);
        //player is not building anything
        inProgress = false;
        //there is no current buildable object
        CurrentBuildableObject = null;
        //set buttons interactable
        SetInteractable();
    }
    /// <summary>
    /// function to control building buttons interactibility
    /// </summary>
    public void SetInteractable()
    {
        //loop through building type buttons
        for (int i = 0; i < buildingButtons.Count; i++)
        {
            //set control their click function
            buildingButtons[i].interactable = !inProgress;
        }
    }
    /// <summary>
    /// function will be called
    /// when user drags object from button
    /// </summary>
    /// <param name="buildableObject">what is the type of this button`s buildable object</param>
    public void OnButtonDragMoveObject(BuildableObjects buildableObject)
    {
        //convert input position to world
        Vector3 mouseToWorld = MouseToWorld();
        if (!mouseToWorld.Equals(Vector3.zero))//check if we are hitting ground
        {
            if (currentBuildableObject == null)//check player does not have buildable object
            {
                //create visual object
                currentBuildableObject = RefreshVisual(buildableObject);
                //player is building 
                inProgress = true;
                //make buttons not pressable
                SetInteractable();
            }
            else//check if player does have buildable object
            {
                if (LastPosition == true)//check this position is not last position
                {
                    if (Utils.UITouchCheck.IsPointerOverUI(mouseToWorld.x, mouseToWorld.z))//check player is not pressing ui 
                        return;
                }
                mouseToWorld.y = 0.5f;//set world position higher than ground
                //move visual buildable object
                currentBuildableObject.gameObject.transform.position = mouseToWorld;
                if (LevelManager.Instance.TutorialIsGoing.Equals(true))
                {
                    if (TutorialManager.Instance.IsTextWritingOver.Equals(true))
                    {
                        if (TutorialManager.Instance.Tutorials[4])
                        {
                            TutorialManager.Instance.Tutorials[4].SetContainsAction(false);
                            TutorialManager.Instance.Tutorials[4].CheckOrder();
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// function will be called
    /// when user ends drag
    /// </summary>
    /// <param name="buildable"></param>
    private void OnEndDragButton(BuildableObjects buildable)
    {
        //check player can build this visual buildable object
        if (currentBuildableObject.CurrentStates == States.canBuild)
            uIManager.AskToBuild(currentBuildableObject.transform.position);//ask from user to build or not
    }
}
[Serializable]
public class BuildingData
{
    public Types type;//type of building
    public BuildableObjects prefab;//prefab object for building
    
}
