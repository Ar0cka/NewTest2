using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Build
{
    public class BuilderSaver : MonoBehaviour
    {
        [SerializeField] private BuildingSystem buildingSystem;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private List<BuildData> availableBuildings;

        public void SaveBuildings()
        {
            GridSaveData saveData = new GridSaveData
            {
                buildingStates = buildingSystem.BuildingStates
            };

            string json = JsonUtility.ToJson(saveData, true);
            string path = Application.persistentDataPath + "/buildings.json";
            File.WriteAllText(path, json);
        }

        public void LoadBuildings()
        {
            string path = Application.persistentDataPath + "/buildings.json";
            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    GridSaveData saveData = JsonUtility.FromJson<GridSaveData>(json);

                    foreach (var building in buildingSystem.BuildingStates)
                    {
                        Collider2D hit = Physics2D.OverlapPoint(gridManager.CellToWorld(building.gridPosition));
                        if (hit != null && hit.CompareTag("Build"))
                        {
                            Destroy(hit.gameObject);
                        }
                    }

                    buildingSystem.BuildingStates.Clear();

                    foreach (var building in saveData.buildingStates)
                    {
                        BuildData buildData = GetBuildDataById(building.buildingID);
                        Debug.Log(buildData.BuildName);
                        if (buildData != null)
                        {
                            Vector3 worldPos = gridManager.CellToWorld(building.gridPosition);
                            Instantiate(buildData.PrefabBuild, worldPos, Quaternion.identity);
                            buildingSystem.BuildingStates.Add(building);
                        }
                        else
                        {
                            Debug.LogWarning($"Building with ID {building.buildingID} not found!");
                        }
                    }

                    Debug.Log("Buildings loaded from: " + path);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Failed to load: " + e.Message);
                }
            }
            else
            {
                Debug.Log("No save file found at: " + path);
            }
        }

        private BuildData GetBuildDataById(string id)
        {
            return availableBuildings.Find(build => build.BuildName == id);
        }

        private void OnApplicationQuit()
        {
            SaveBuildings();
        }
    }
}