using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RollSort.Runtime.GridManagement
{
    public class GridView : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        private GridController _controller;
        public SO_GridData GridData { get; private set; }

        private async void Awake()
        {
            AsyncOperationHandle<SO_GridData> handle = Addressables.LoadAssetAsync<SO_GridData>("SO_GridData");
            GridData = await handle.ToUniTask();

            _controller = new GridController(this);
        }

        public void GenerateGrid()
        {
            for (int row = 0; row < GridData.Height; row++)
            for (int col = 0; col < GridData.Width; col++)
            {
                GameObject cellObject = Instantiate(cellPrefab, new Vector3(col, 0, row), Quaternion.identity);
                cellObject.gameObject.name = $"Cell ({row} {col})";
            }
        }
    }
}