using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTutorial : Tutorial
{
    //tutorial ketma ketligi 
    //actionla bilan 
    Utils.ActionsController actionsController = new Utils.ActionsController();
    public override void CheckOrder()
    {
        actionsController.CallOnlyOnce(TutorialManager.Instance.complete);
    }

}
