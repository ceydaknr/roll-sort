namespace RollSort.Runtime.GridManagement
{
    public class GridController
    {
        private readonly GridView _view;

        public GridController(GridView view)
        {
            _view = view;

            _view.GenerateGrid();
        }
    }
}