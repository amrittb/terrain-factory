using UnityEngine;

namespace TerrainFactory {

    public struct TerrainData {

        /// <summary>
        /// 
        /// Number of tiles in x-axis
        /// 
        /// </summary>
        public int numTilesX;

        /// <summary>
        /// 
        /// Number of tiles in z-axis
        /// 
        /// </summary>
        public int numTilesZ;

        /// <summary>
        /// 
        /// Each tile size
        /// 
        /// </summary>
        public float tileSize;

        /// <summary>
        /// 
        /// Height multipfier
        /// 
        /// </summary>
        public float heightStrength;

        /// <summary>
        /// 
        /// Heightmap texture
        /// 
        /// </summary>
        public Texture2D heightMap;

        /// <summary>
        /// 
        /// Terrain Material
        /// 
        /// </summary>
        public Material terrainMaterial;

        /// <summary>
        /// 
        /// Creates a TerrainData object.
        /// 
        /// </summary>
        /// <param name="numTilesX">Number of tiles in x-axis</param>
        /// <param name="numTilesZ">Number of tiles in z-axis</param>
        /// <param name="tileSize">Each tile size</param>
        /// <param name="heightStrength">Height multipfier</param>
        /// <param name="heightMap">Heightmap texture</param>
        /// <param name="terrainMaterial">Terrain Material</param>
        public TerrainData(
                            int numTilesX,
                            int numTilesZ, 
                            float tileSize,
                            float heightStrength,
                            Texture2D heightMap,
                            Material terrainMaterial
            ) {
            this.numTilesX = numTilesX;
            this.numTilesZ = numTilesZ;
            this.tileSize = tileSize;
            this.heightStrength = heightStrength;
            this.heightMap = heightMap;
            this.terrainMaterial = terrainMaterial;
        }

        /// <summary>
        /// 
        /// Checks if two terrain data object are equivalent
        /// 
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if two objects are equivalent</returns>
        public override bool Equals(object obj) {
            if(typeof(TerrainData) != obj.GetType()) {
                return false;
            }

            TerrainData data = (TerrainData) obj;

            return (numTilesX == data.numTilesX 
                            && 
                    numTilesZ == data.numTilesZ 
                            && 
                    tileSize == data.tileSize 
                            && 
                    heightStrength == data.heightStrength 
                            && 
                    heightMap == data.heightMap 
                            && 
                    terrainMaterial == data.terrainMaterial);
        }

        /// <summary>
        /// 
        /// Checks if the height data is changed but other data are same.
        /// 
        /// </summary>
        /// <param name="data">TerrainData object to compare</param>
        /// <returns>True if the the height is updateable</returns>
        public bool IsHeightUpdatable(TerrainData data) {
            return (numTilesX == data.numTilesX && numTilesZ == data.numTilesZ && tileSize == data.tileSize);
        }
    }
}