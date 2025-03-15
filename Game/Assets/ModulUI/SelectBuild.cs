using System.Collections;
using Build;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UIModul
{
    public class SelectBuild : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        [FormerlySerializedAs("build")] [SerializeField] private BuildData buildData;
        [FormerlySerializedAs("button")] [SerializeField] private Image buttonImage;

        private Color _defaultColor = Color.white;

        public void InitializeSelectBuildButtons()
        {
            if (uiManager == null) uiManager = GetComponentInParent<UIManager>();
            if (buttonImage == null) buttonImage = GetComponent<Image>();
        }
        
        public void Select()
        {
            if (buildData == null || buttonImage == null) return;

            uiManager.TakeObjectFroBuilding(buildData, this);
            buttonImage.color = Color.green;
        }

        public void NotSelected()
        {
            buttonImage.color = _defaultColor;
        }
        
    }
}