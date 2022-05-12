using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    int order;
    [TextArea]
    [SerializeField]
    string explanation;
    [SerializeField]
    protected bool containstAction;
    public int Order { get => order; }
    public string Explanation { get => explanation; }
    public bool ContainstAction { get => containstAction;}

    private void Start()
    {
        //TutorialManager.Instance.Tutorials.Add(this);
    }
    public int SetOrder() => order++;
    public bool SetContainsAction(bool v) => containstAction = v;
    public virtual void CheckOrder()
    {

    }
}
