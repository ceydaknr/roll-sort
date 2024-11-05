using Cysharp.Threading.Tasks;
using RollSort.Runtime.Container;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RollSort.Runtime.GridManagement
{
    public class GridView : MonoBehaviour
    {
        private GridController _controller;
        private SO_GridData _gridData;

        private IContainer[,] _grid;
        
        private async void Awake()
        {
            AsyncOperationHandle<SO_GridData> handle = Addressables.LoadAssetAsync<SO_GridData>("SO_GridData");
            _gridData = await handle.ToUniTask();

            _controller = new GridController(this, _gridData);
        }

        public void AddContainer(int row, int column) //TODO: Add game object parameter.
        {
            // _grid[row, column] = ;
            Debug.Log($"Adding container at {row}, {column}");
        }
        
        public void RemoveContainer(int row, int column) //TODO: Add game object parameter.
        {
            // _grid[row, column] = ;
            Debug.Log($"Removing container at {row}, {column}");
        }
        
    }
}