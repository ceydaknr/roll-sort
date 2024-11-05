using System;
using RollSort.Runtime.Utilities;
using UnityEngine;

namespace RollSort.Runtime.Pool
{
    public class PoolSignals : MonoSingleton<PoolSignals>
    {
        public Func<PoolType, Transform, GameObject> onGetPoolObject;
        public Func<PoolType, GameObject> onGetPoolObjectNoParent;
        public Action<PoolType, GameObject> onReleasePoolObject;
    }
}