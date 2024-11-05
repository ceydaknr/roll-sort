using Data.ValueObject;
using Pool;
using UnityEngine;
using UnityEngine.Rendering;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "SO_Pool", menuName = "GAME/SO_Pool", order = 0)]
    public class SO_Pool : ScriptableObject
    {
        public SerializedDictionary<PoolTypes, PoolTypeData> PoolTypeDatas;
    }
}