using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils
{
    public static class UITouchCheck
    {

        //Check if mouse or touch is pressed over UI
        public static bool IsPointerOverUI(float x, float y)
        {
            //(x,y) Screen touch or mouse down positions
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = new Vector2(x, y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    } 
}
