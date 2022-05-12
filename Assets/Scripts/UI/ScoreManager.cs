using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    private int score;//score of player
    public System.Action<int> onScoreChanged;//when score changed update score text action
    //public accessor and action caller
    public int Score 
    {
        get 
        { 
            return score; 
        }
        private set
        {
            score = value;
            onScoreChanged(score);
        }
    }
    private void Start()
    {
        LevelManager.Instance.SetScoreManager(this);
    }
    /// <summary>
    /// function for adding score to our current score
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public int AddScore(int amount)
    {
        return Score += amount;
    }
}
