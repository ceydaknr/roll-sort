using UnityEngine;

namespace RollSort
{
    public class HexGrid : MonoBehaviour
    {
        public GameObject hexCellPrefab; // Assign your hexagonal cell prefab here
        public int gridWidth = 5; // Number of cells horizontally
        public int gridHeight = 5; // Number of cells vertically
        public float hexRadius = 1f; // Radius of each hexagonal cell

        private void Start()
        {
            GenerateHexGrid();
        }

        private void GenerateHexGrid()
        {
            float hexWidth = hexRadius * 2;
            float hexHeight = Mathf.Sqrt(3) * hexRadius;

            for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
            {
                // Calculate offset for hexagonal layout
                float xOffset = y % 2 == 0 ? 0 : hexWidth * 0.5f;
                Vector3 cellPosition = new(
                    x * hexWidth * 0.75f + xOffset,
                    0,
                    y * hexHeight * 0.5f
                );

                // Instantiate the hex cell prefab at calculated position
                Instantiate(hexCellPrefab, cellPosition, Quaternion.identity, transform);
            }
        }
    }
}