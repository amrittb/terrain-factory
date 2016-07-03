using UnityEngine;

namespace TerrainFactory {

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class HeightMapTerrain : MonoBehaviour {
        /// <summary>
        /// 
        /// TerrainData object for the terrain
        /// 
        /// </summary>
        public TerrainData terrainData;
    }
}