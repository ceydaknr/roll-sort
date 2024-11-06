using RollSort.Runtime.Container;
using RollSort.Runtime.EventHandler;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RollSort.Runtime.GridManagement
{
    public class GridView : MonoBehaviour
    {
        private GridController _controller;

        private IContainer[,] _grid;
        private SO_GridData _gridData;

        private async void Awake()
        {
            AsyncOperationHandle<SO_GridData> handle = Addressables.LoadAssetAsync<SO_GridData>("SO_GridData");

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _gridData = handle.Result;

                _grid = new IContainer[_gridData.Height, _gridData.Width];
                _controller = new GridController(this, _gridData);
                EventManager.AddListener(GameEvent.OnLevelStarted, _controller.GenerateGrid);
            }
            else
            {
                Debug.LogError("SO_GridData load failed!");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) EventManager.Broadcast(GameEvent.OnLevelStarted);
        }

        private void OnEnable()
        {
            if (_controller != null) EventManager.AddListener(GameEvent.OnLevelStarted, _controller.GenerateGrid);
        }

        private void OnDisable()
        {
            if (_controller != null) EventManager.RemoveListener(GameEvent.OnLevelStarted, _controller.GenerateGrid);
        }

        public void AddContainer(int row, int column, IContainer container)
        {
            _grid[row, column] = container;
            Debug.Log($"Adding {container} at {row}, {column}");
        }

        public void RemoveContainer(int row, int column)
        {
            Debug.Log($"Removing {_grid[row, column]} at {row}, {column}");
            _grid[row, column] = null;
        }
    }
}