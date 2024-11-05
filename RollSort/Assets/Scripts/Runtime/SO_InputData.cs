using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = nameof(SO_InputData), menuName = "GAME/"+ nameof(SO_InputData), order = 0)]
    public class SO_InputData : ScriptableObject
    {
        public InputData Data;
    }
}