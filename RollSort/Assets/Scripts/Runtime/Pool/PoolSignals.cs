using System;
using Pool;
using RollSort.Runtime.Utilities;
using UnityEngine;

namespace Signals
{
    public class PoolSignals : MonoSingleton<PoolSignals>
    {
        public Func<PoolTypes, Transform, GameObject> onGetPoolObject;
        public Func<PoolTypes, GameObject> onGetPoolObjectNoParent;
        public Action<PoolTypes, GameObject> onReleasePoolObject;
    }
}