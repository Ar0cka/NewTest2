using UnityEngine;

namespace Build
{
    [CreateAssetMenu(fileName = "build", menuName = "ScriptableObject/Build", order = 0)]
    public class BuildData : ScriptableObject
    {
        [SerializeField] private string buildName;
        [SerializeField] private Vector2Int size;
        [SerializeField] private Sprite previewSprite;
        [SerializeField] private GameObject prefabBuild;

        public string BuildName => buildName;
        public Vector2Int Size => size;
        public Sprite PreviewSprite => previewSprite;
        public GameObject PrefabBuild => prefabBuild;
    }
}