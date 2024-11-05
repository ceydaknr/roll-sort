using UnityEngine;
using UnityEngine.Rendering;

namespace RollSort.Runtime.Pool
{
    [CreateAssetMenu(fileName = "SO_Pool", menuName = "Data/SO_Pool")]
    public class SO_Pool : ScriptableObject
    {
        public SerializedDictionary<PoolType, PoolTypeData> PoolTypeDatas;
    }
}