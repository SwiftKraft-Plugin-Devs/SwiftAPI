using UnityEngine;

namespace SwiftAPI.Utility.Misc
{
    public static class GridSnapper
    {
        public static Vector3 SnapToGrid(Vector3 originalPosition, Vector3 scale, Vector3 gridSize) => new Vector3(Mathf.Round(originalPosition.x / gridSize.x) * gridSize.x + (scale.x % (scale.x * 2) != 0 ? gridSize.x / 2f : 0f), Mathf.Round(originalPosition.y / gridSize.y) * gridSize.y + (scale.y % (scale.y / 2f) == 0 ? gridSize.y / 2f : 0f), Mathf.Round(originalPosition.z / gridSize.z) * gridSize.z + (scale.z % (gridSize.z / 2f) == 0 ? gridSize.z / 2f : 0f));
    }
}
