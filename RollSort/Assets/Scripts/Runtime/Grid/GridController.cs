using UnityEngine;

namespace RollSort.Runtime.GridManagement
{
    public class GridController
    {
        private readonly GridView _view;
        private readonly SO_GridData _gridData;

        public GridController(GridView view, SO_GridData gridData)
        {
            _view = view;
            _gridData = gridData;
            
            GenerateGrid();
        }
        
        private void GenerateGrid()
        {
            for (int row = 0; row < _gridData.Height; row++)
            for (int col = 0; col < _gridData.Width; col++)
            {
                // GameObject cellObject = in(cellPrefab, new Vector3(col, 0, row), Quaternion.identity);
                // cellObject.gameObject.name = $"Cell ({row} {col})";
                _view.AddContainer(row, col);
            }
        }
    }
}