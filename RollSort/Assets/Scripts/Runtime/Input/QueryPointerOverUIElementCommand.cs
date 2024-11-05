using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RollSort.Runtime.InputManagement
{
    public class QueryPointerOverUIElementCommand
    {
        public bool Execute()
        {
            PointerEventData eventData = new(EventSystem.current)
            {
                position = Input.mousePosition
            };
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}