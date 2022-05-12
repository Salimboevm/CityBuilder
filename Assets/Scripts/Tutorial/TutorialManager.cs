using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    private static TutorialManager instance;
    public static TutorialManager Instance
    {
        get => instance;
    }
    public System.Action complete;
    [SerializeField]
    private List<Tutorial> tutorials = new List<Tutorial>();

    [SerializeField]
    private Text expText;
    [SerializeField]
    private Tutorial currentTutorial;
    [SerializeField]
    TextWriter textWriter;
    private bool isTextWritingOver;
    int id = 0;
    public List<Tutorial> Tutorials { get => tutorials; }
    public int Id { 
        get => id;
        private set 
        { 
            id = value;
        }
    }

    public bool IsTextWritingOver { get => isTextWritingOver;private set => isTextWritingOver = value; }
    public bool SetTextWritingOver(bool v) => isTextWritingOver = v;
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
    private void Start()
    {
        //id = 0;
        //Id++;
        complete += CompletedTutorial;
        SetNextTutorial(id);
        LevelManager.Instance.SetTutorialIsGoing(true);
    }

    public void CompletedTutorial()
    {
        if (tutorials[id].ContainstAction.Equals(false))
        {
            if (isTextWritingOver.Equals(true))
            {
                Id++;
                SetNextTutorial(id);
            }
        }
    }
    public void SetNextTutorial(int currentOrder)
    {
        currentTutorial = GetTutorialByOrder(currentOrder);
        if (!currentTutorial)
        {
            Completed();
            return;
        }

        textWriter.AddWriter(expText,currentTutorial.Explanation,0.05f);
    }
    public void Completed()
    {
        //textWriter.AddWriter(expText, currentTutorial.Explanation, 0.05f);
        LevelManager.Instance.SetTutorialIsGoing(false);
        LevelManager.Instance.Restart();
        //do the logic when game finishes
        //all of the buildings are built and upgraded to max

    }
    public Tutorial GetTutorialByOrder(int order)
    {
        for (int i = 0; i < tutorials.Count; i++)
        {
            if(tutorials[i].Order == order)
            {
                return tutorials[i];
            }
        }
        return null;
    }
}
