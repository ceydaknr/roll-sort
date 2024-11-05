using UnityEngine;

namespace RollSort.Runtime.InputManagement
{
    [CreateAssetMenu(fileName = "SO_InputData", menuName = "Data/SO_InputData")]
    public class SO_InputData : ScriptableObject
    {
        public InputData Data;
    }
}