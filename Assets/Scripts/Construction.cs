using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// states of buildings
/// </summary>
public enum States
{
    //by these states game logic controller will control construction system
    canBuild,//player can build this building
    cannotBuild,//player cannot build this building
    builded//this building is already built
}
public class Construction : MonoBehaviour
{
    [SerializeField]
    private Types ownType;//what is the type of this building
    [SerializeField]
    private States currentStates = States.canBuild;//set current state
    [SerializeField]
    private TextMesh scoreTextMesh;//feedback to user about how much bonus points user gained
    [SerializeField]
    protected List<Types> types;//list of types to find own type from types 
    private GameManager gameManagerInstance;
    public Types OwnType { get => ownType; private set => ownType = value; }//public accessor
    public TextMesh ScoreTextMesh { get => scoreTextMesh; private set => scoreTextMesh = value; }//public accessor
    public States CurrentStates { get => currentStates;protected set => currentStates = value; }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        //check for layer of entering object
        if (other.gameObject.layer == 10)
        {
            Construction buildable = other.GetComponent<Construction>();//get entering object
            if (buildable == null)//check if entering object is constructable building type
                return;//if not early return
            //check for this building type
            if (types.Contains(buildable.OwnType))
            {
                //activate user feedback text
                scoreTextMesh.gameObject.SetActive(true);
                
                //show user about how much bonus is player getting from this building object
                ScoreTextMesh.text = gameManagerInstance.ConnectionValues.GetValueByTypes(OwnType, buildable.OwnType).ToString();
            }
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        //check for layer of entering object
        if (other.gameObject.layer == 10)
        {
            //get entering object
            Construction buildable = other.GetComponent<Construction>();
            if (buildable == null)//check if entering object is constructable building type
                return;//if not early return
            //check for this building type
            if (types.Contains(buildable.OwnType))
            {
                //deactivate user feedback text
                ScoreTextMesh.gameObject.SetActive(false);
            }
        }
    }
    protected virtual void Start()
    {
        gameManagerInstance = FindObjectOfType<GameManager>();
        types = gameManagerInstance.GetTypesByValue(OwnType);//get types by their value
    }
}
