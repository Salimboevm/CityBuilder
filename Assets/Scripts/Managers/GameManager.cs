using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// building types
/// </summary>
public enum Types
{
    house,
    cafe,
    factory,
    shop,
    road,
    environment
}
public class GameManager : MonoBehaviour
{
    
    //variable to check if the game is over 
    public bool GameOver { 
        get => gameOver;//public accessor
        private set 
        {
            gameOver = value;//change game over value
            if(GameOver.Equals(true))//is game over
                gameOverAction.Invoke();//start game over action
        }
    }


    [SerializeField]
    private CommunicationValue connectionValues;//communication values between buildable objects to check
    public System.Action gameOverAction;//game over action
    private int allowedNumberOfButtonsLeft;//how many buttons are allowed
    private bool gameOver;//private varialbe to check game over
    #region properties
    public CommunicationValue ConnectionValues { get => connectionValues;private set => connectionValues = value; }
    public int AllowedNumberOfButtonsLeft { get => allowedNumberOfButtonsLeft;private set => allowedNumberOfButtonsLeft = value; }
    public int SetAllowedNumberOfButtonsLeft() => allowedNumberOfButtonsLeft--;
    #endregion
    /// <summary>
    /// get types of buildings by their values
    /// </summary>
    /// <param name="t">types of buildings</param>
    /// <returns></returns>
    public List<Types> GetTypesByValue(Types t)
    {
        List<Types> listOfTypes = new List<Types>();//list of types 
        //search for types
        ConnectionValues.comVs.Where(i => t.Equals(i.firstType) || t.Equals(i.secondtype)).ToList().ForEach(e => listOfTypes.Add(e.firstType == t ? e.secondtype: e.firstType));
        return listOfTypes;//ge ttypes
    }
    ScoreManager scoreManager;
    private void OnEnable()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }
    private void Start()
    {
        allowedNumberOfButtonsLeft = LevelManager.Instance.CurrentLevel.levelDatas.Count;//ge number of allowed building buttons
        GameOver = false;//game is not over
    }
    /// <summary>
    /// function to control 
    /// game over
    /// </summary>
    public void GameOverCheck()
    {
        //check for how many buttons left 
        if(allowedNumberOfButtonsLeft == 0)
        {
            //if user runs out of buttons
            //compare score for current level max bonus 
            if(scoreManager.Score < LevelManager.Instance.CurrentLevel.MaxBonusToOpenNewLevel)
            {
                //if it is less than max bonus 
                //game is over
                GameOver = true;
            }
        }
    }
}
