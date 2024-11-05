using UnityEngine;

namespace RollSort.Test
{
    [CreateAssetMenu(fileName = "SO_GridData", menuName = "Data/SO_GridData")]
    public class SO_GridData : ScriptableObject
    {
        public int CellSize;
        public int CellSpacing;
    }
}