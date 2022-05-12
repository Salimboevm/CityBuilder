using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// class can be converted and used as a scriptable object
/// </summary>
public class Level : MonoBehaviour
{

    [SerializeField]
    private int maxBonusToOpenNewLevel;//maximum amount of bonus to open new lvl
    public List<LevelButtonData> levelDatas = new List<LevelButtonData>();//list of level data buttons
    [SerializeField]
    private Vector2 maxLimitForCamera;//maximum moving limit to camera
    #region properties
    public int MaxBonusToOpenNewLevel { get => maxBonusToOpenNewLevel; private set => maxBonusToOpenNewLevel = value; }
    public Vector2 MaxLimitForCamera { get => maxLimitForCamera;private set => maxLimitForCamera = value; }
    #endregion
}
