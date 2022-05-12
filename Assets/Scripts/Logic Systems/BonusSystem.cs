using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSystem : MonoBehaviour
{
    ScoreManager scoreManager;
    private void OnEnable()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }
    /// <summary>
    /// give bonus point
    /// </summary>
    /// <param name="amount"></param>
    public void GiveBonus(int amount)
    {
        scoreManager.AddScore(amount);
    }
}
