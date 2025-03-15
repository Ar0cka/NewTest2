using System.Collections;
using System.Collections.Generic;
using Build;
using UIModul;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private List<SelectBuild> selectBuilds;
    [SerializeField] private BuildingSystem buildingSystem;
    [SerializeField] private BuilderSaver builderSaver;
    
    private void Awake()
    {
        GameControls gameControls = new GameControls();
        buildingSystem.SetGameControls(gameControls);
        
        if (selectBuilds != null)
        {
            foreach (var item in selectBuilds)
            {
                item.InitializeSelectBuildButtons();
            }
        }
        
        builderSaver.LoadBuildings();
    }
}
