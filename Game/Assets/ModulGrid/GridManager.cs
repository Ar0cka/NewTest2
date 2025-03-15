
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : UnityEngine.MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap tilemap;

    public Grid Grid => grid;

    private void InitializeGrid()
    {
        if (grid == null) grid = GetComponent<Grid>();
        if (tilemap == null) tilemap = GetComponentInChildren<Tilemap>();
    }

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        return grid.WorldToCell(worldPosition);
    }

    public Vector3 CellToWorld(Vector3Int cellPosition)
    {
        return grid.CellToWorld(cellPosition);
    }

    public bool IsCellOccupied(Vector3Int cell)
    {
        Vector3 worldPos = CellToWorld(cell);
        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        return hit != null;
    }
    
    public bool IsCellInGrid(Vector3Int cell)
    {
        return tilemap.HasTile(cell); // Есть тайл — ячейка в сетке
    }
    
}