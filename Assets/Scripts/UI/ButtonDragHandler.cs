using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDragHandler : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private BuildableObjects thisButtonsBuildableObject;//what should I need to build
    [SerializeField]
    private Text maxNumberAllowedToBuild;//how many objects of this type player needs to build
    private BuildingSystem buildingSystem;
    public System.Action<string> onMaxNumberChanged;//when ui number changed

    public Text MaxNumberAllowedToBuild { 
        get => maxNumberAllowedToBuild;
        private set 
        { 
            maxNumberAllowedToBuild = value;
            onMaxNumberChanged(maxNumberAllowedToBuild.text);//update text
        }
    }
    private void OnEnable()
    {
        buildingSystem = FindObjectOfType<BuildingSystem>();
    }
    /// <summary>
    /// setting buildable object for this button
    /// </summary>
    /// <param name="buildableObject">buildable object type</param>
    /// <returns></returns>
    public BuildableObjects SetCurrentBuildableObject(BuildableObjects buildableObject)
    {
        return thisButtonsBuildableObject = buildableObject;
    }
   
    /// <summary>
    /// when user presses button 
    /// make visual building object 
    /// for showcasing 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
    }
    /// <summary>
    /// when user moves their fingers 
    /// move previously instatiated object to their finger position
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
       buildingSystem._aOnDrag(thisButtonsBuildableObject);        
    }
    /// <summary>
    /// when user decides where to build visual object 
    /// ask if he really wants to build this object in to here
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
       buildingSystem._aOnEndDrag(thisButtonsBuildableObject);
    }
    
}
