using System.Collections.Generic;
using UnityEngine;

namespace Build
{
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] private GridManager gridManager;

        private BuildData _buildData;
        private GameObject _previewObject;

        private bool _removeBuild;

        private GameControls _gameControls;
        private List<BuildingState> _buildingStates = new List<BuildingState>();
        public List<BuildingState> BuildingStates => _buildingStates;

       public void SetGameControls(GameControls gameControls)
        {
            _gameControls = gameControls;
            
            InitializeInput();
        }

        private void InitializeInput()
        {
            if (_gameControls != null)
            {
                _gameControls.Player.Enable();
                _gameControls.Player.LeftClick.performed += ctx => OnLeftClick();
                _gameControls.Player.RightClick.performed += ctx => OnRightClick();
            }
        }

        private void OnDisable()
        {
            _gameControls.Player.Disable();
            _gameControls.Player.LeftClick.performed -= ctx => OnLeftClick();
            _gameControls.Player.RightClick.performed -= ctx => OnRightClick();
        }

        void Update()
        {
            if (_buildData != null || _removeBuild)
            {
                Vector2 mouseScreenPos = _gameControls.Player.MousePosition.ReadValue<Vector2>();
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
                mousePos.z = 0;
                Vector3Int cellPos = gridManager.WorldToCell(mousePos);

                if (_buildData != null)
                {
                    UpdatePreview(cellPos);
                }
            }
        }

        private void OnLeftClick()
        {
            Vector2 mouseScreenPos = _gameControls.Player.MousePosition.ReadValue<Vector2>();
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            mousePos.z = 0;
            Vector3Int cellPos = gridManager.WorldToCell(mousePos);

            if (_buildData != null && CanPlaceBuilding(cellPos))
            {
                PlaceBuilding(cellPos);
                _buildingStates.Add(new BuildingState(_buildData.BuildName, cellPos));
                Destroy(_previewObject);
                _previewObject = null;
                _buildData = null;
            }
            else if (_removeBuild)
            {
                Collider2D hit = Physics2D.OverlapPoint(gridManager.CellToWorld(cellPos));
                if (hit != null && hit.CompareTag("Build"))
                {
                    _buildingStates.RemoveAll(b => b.gridPosition == cellPos);
                    Destroy(hit.gameObject);
                }
            }
        }

        private void OnRightClick()
        {
            if (_buildData != null)
            {
                Destroy(_previewObject);
                _previewObject = null;
                _buildData = null;
            }
            else if (_removeBuild)
            {
                _removeBuild = false;
            }
        }

        public void SelectBuilding(BuildData build)
        {
            _removeBuild = false;
            if (_previewObject != null) Destroy(_previewObject);
            _buildData = build;
            CreatePreview();
        }

        private void CreatePreview()
        {
            _previewObject = new GameObject("BuildingPreview");
            SpriteRenderer renderer = _previewObject.AddComponent<SpriteRenderer>();
            renderer.sprite = _buildData.PreviewSprite;
            renderer.color = new Color(1, 1, 1, 0.5f);
        }

        private void UpdatePreview(Vector3Int cellPos)
        {
            if (_previewObject != null)
            {
                Vector3 worldPos = gridManager.CellToWorld(cellPos);
                _previewObject.transform.position = worldPos;

                SpriteRenderer renderer = _previewObject.GetComponent<SpriteRenderer>();
                renderer.color = CanPlaceBuilding(cellPos)
                    ? new Color(0, 1, 0, 0.5f)
                    : new Color(1, 0, 0, 0.5f);
            }
        }

        private bool CanPlaceBuilding(Vector3Int cellPos)
        {
            Vector2Int size = _buildData.Size;
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3Int checkCell = cellPos + new Vector3Int(x, y, 0);
                    if (!gridManager.IsCellInGrid(checkCell) || gridManager.IsCellOccupied(checkCell))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void PlaceBuilding(Vector3Int cellPos)
        {
            Vector3 worldPos = gridManager.CellToWorld(cellPos);
            Instantiate(_buildData.PrefabBuild, worldPos, Quaternion.identity);
        }

        public void DeleteBuild()
        {
            _removeBuild = true;
        }
    }
}