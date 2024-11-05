using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RollSort.Test
{
    public class GridView : MonoBehaviour
    {
        private GridController _controller;
        public SO_GridData GridData { get; private set; }

        private async void Awake()
        {
            AsyncOperationHandle<SO_GridData> handle = Addressables.LoadAssetAsync<SO_GridData>("SO_GridData");
            GridData = await handle.ToUniTask();

            _controller = new GridController(this);
        }
    }
}