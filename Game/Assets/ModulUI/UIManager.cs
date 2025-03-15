using Build;
using UnityEngine;

namespace UIModul
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private BuildingSystem buildingSystem;

        private BuildData builderObject;
        private SelectBuild _currentSelectBuild;
        
        public void TakeObjectFroBuilding(BuildData buildData, SelectBuild selectBuild)
        {
            if (_currentSelectBuild != selectBuild && _currentSelectBuild != null)
            {
                _currentSelectBuild.NotSelected();
            }
            
            _currentSelectBuild = selectBuild;
            builderObject = buildData;
        }

        public void PlaceBuild()
        {
            buildingSystem.SelectBuilding(builderObject);
            _currentSelectBuild.NotSelected();
        }

        public void DeleteBuild()
        {
            buildingSystem.DeleteBuild();
        }
    }
}