using RollSort.Runtime.Container;
using RollSort.Runtime.Pool;
using UnityEngine;

namespace RollSort.Runtime.GridManagement
{
    public class GridController
    {
        private readonly SO_GridData _gridData;
        private readonly GridView _view;

        public GridController(GridView view, SO_GridData gridData)
        {
            _view = view;
            _gridData = gridData;
        }

        public void GenerateGrid()
        {
            for (int row = 0; row < _gridData.Height; row++)
            for (int col = 0; col < _gridData.Width; col++)
            {
                GameObject containerObject =
                    PoolSignals.Instance.onGetPoolObject?.Invoke(PoolType.Container, _view.transform);
                if (containerObject == null) continue;

                containerObject.transform.localPosition = new Vector3(col, 0, row);
                containerObject.name = $"Cell ({row} {col})";

                if (containerObject.TryGetComponent(out IContainer container))
                    _view.AddContainer(row, col, container);
            }
        }
    }
}