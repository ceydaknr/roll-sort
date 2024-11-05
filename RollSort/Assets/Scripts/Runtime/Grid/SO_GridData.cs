using UnityEngine;

namespace RollSort.Runtime.GridManagement
{
    [CreateAssetMenu(fileName = "SO_GridData", menuName = "Data/SO_GridData")]
    public class SO_GridData : ScriptableObject
    {
        public int Width;
        public int Height;
        public int CellSize;
        public int CellSpacing;
    }
}