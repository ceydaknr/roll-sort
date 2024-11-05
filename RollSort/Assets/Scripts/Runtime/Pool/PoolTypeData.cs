using System;
using UnityEngine;

namespace RollSort.Runtime.Pool
{
    [Serializable]
    public class PoolTypeData
    {
        [Range(0, 100)] public int ObjectLimit;
        public GameObject PooledGameObject;
    }
}