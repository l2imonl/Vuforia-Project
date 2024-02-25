using UnityEngine;

public class GridCell
{
    public bool IsWalkable;
    public Vector3 WorldPosition;
    public int GridX;
    public int GridY;

    public GridCell(bool isWalkable, Vector3 worldPos, int gridX, int gridY)
    {
        IsWalkable = isWalkable;
        WorldPosition = worldPos;
        GridX = gridX;
        GridY = gridY;
    }
}
