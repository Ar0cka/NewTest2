using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Build
{
    [Serializable]
    public class BuildingState
    {
        [FormerlySerializedAs("BuildingID")] public string buildingID;
        [FormerlySerializedAs("GridPosition")] public Vector3Int gridPosition;

        public BuildingState(string id, Vector3Int pos)
        {
            buildingID = id;
            gridPosition = pos;
        }
    }
}