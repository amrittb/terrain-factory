using UnityEngine;

namespace TerrainFactory {

    public class Factory {

        /// <summary>
        /// 
        /// Old terrain data cache.
        /// 
        /// </summary>
        private TerrainData data;

        /// <summary>
        /// 
        /// Builds Mesh out of terrain data
        /// 
        /// </summary>
        /// <param name="data">Terrain data from which terrain is to be built.</param>
        /// <param name="terrainObject">Terrain Object in which the terrain is to be built</param>
        /// <returns>Built terrain object</returns>
        public HeightMapTerrain BuildMesh(TerrainData data, HeightMapTerrain terrainObject = null) {
            this.data = data;

            if (terrainObject == null) {
                terrainObject = BuildTerrainGO();
            } else {
                if(terrainObject.GetComponent<MeshFilter>() == null || terrainObject.GetComponent<MeshRenderer>() == null) {
                    Debug.LogError("Terrain GameObject must have both MeshFilter and MeshReneder Component");
                    return terrainObject;
                }
            }

            terrainObject.terrainData = data;
            terrainObject.GetComponent<MeshFilter>().mesh = CreateMesh(data);
            terrainObject.GetComponent<MeshRenderer>().material = data.terrainMaterial;

            return terrainObject;
        }

        /// <summary>
        /// 
        /// Builds Terrain Game Object with required components
        /// 
        /// </summary>
        /// <returns>Built terrain game object</returns>
        private HeightMapTerrain BuildTerrainGO() {
            GameObject terrain = new GameObject();

            terrain.transform.position = Vector3.zero;

            HeightMapTerrain heightMapTerrainComp = terrain.AddComponent<HeightMapTerrain>();

            return heightMapTerrainComp;
        }

        /// <summary>
        /// 
        /// Updates terrain mesh
        /// 
        /// </summary>
        /// <param name="data">Data for which the mesh is to be modified</param>
        /// <param name="terrainObject">Terrain Object which is to be updated</param>
        public void UpdateMesh(TerrainData data,HeightMapTerrain terrainObject) {
            if (!terrainObject.terrainData.Equals(data)) {
                if(terrainObject.terrainData.IsHeightUpdatable(data)) {
                    UpdateMeshHeightMap(data, terrainObject);

                    if(terrainObject.terrainData.terrainMaterial != data.terrainMaterial) {
                        terrainObject.gameObject.GetComponent<MeshRenderer>().material = data.terrainMaterial;
                    }
                } else {
                    BuildMesh(data, terrainObject);
                }

                terrainObject.terrainData = data;
            }
        }

        /// <summary>
        /// 
        /// Updates Terrain mesh's height via heightmap
        /// 
        /// </summary>
        /// <param name="data">Data for which the height is to be updated</param>
        /// <param name="terrainObject">Terrain object which updates</param>
        private void UpdateMeshHeightMap(TerrainData data, HeightMapTerrain terrainObject) {
            int vSizeX = terrainObject.terrainData.numTilesX + 1;
            int vSizeZ = terrainObject.terrainData.numTilesZ + 1;

            Mesh mesh = terrainObject.gameObject.GetComponent<MeshFilter>().mesh;

            Vector3[] meshVerts = mesh.vertices;
            Vector3[] vertices = new Vector3[mesh.vertexCount];


            for (int z = 0; z < vSizeZ; z++) {
                for (int x = 0; x < vSizeX; x++) {
                    int index = z * vSizeX + x;
                    float height = GetHeightFromMap(data.heightMap, data.heightStrength, vSizeX, vSizeZ, z, x);

                    meshVerts[index].y = height;
                    vertices[index] = meshVerts[index];
                }
            }

            mesh.vertices = vertices;
            mesh.Optimize();
            mesh.RecalculateNormals();
        }

        /// <summary>
        /// 
        /// Creates Mesh from the terrain data.
        /// 
        /// </summary>
        /// <param name="data">Terrain Data from which mesh is to be created.</param>
        /// <returns>Mesh object that was created</returns>
        private Mesh CreateMesh(TerrainData data) {
            int numTiles = data.numTilesX * data.numTilesZ;
            int numTris = 2 * numTiles;

            int vSizeX = data.numTilesX + 1;
            int vSizeZ = data.numTilesZ + 1;
            int numVertices = vSizeX * vSizeZ;

            Vector2[] uvs = new Vector2[numVertices];
            Vector3[] points = new Vector3[numVertices];

            int[] triangles = new int[3 * numTris];

            for (int z = 0; z < vSizeZ; z++) {
                for (int x = 0; x < vSizeX; x++) {
                    float height = GetHeightFromMap(data.heightMap, data.heightStrength, vSizeX, vSizeZ, z, x);

                    points[z * vSizeX + x] = new Vector3(x * data.tileSize, height, z * data.tileSize);
                    uvs[z * vSizeX + x] = new Vector2((float)x / data.numTilesX, (float)z / data.numTilesZ);
                }
            }

            for (int z = 0; z < data.numTilesZ; z++) {
                for (int x = 0; x < data.numTilesX; x++) {
                    int squareIndex = z * data.numTilesX + x;
                    int triangleOffset = squareIndex * 6;

                    triangles[triangleOffset + 0] = z * vSizeX + x +          0;
                    triangles[triangleOffset + 1] = z * vSizeX + x + vSizeX + 0;
                    triangles[triangleOffset + 2] = z * vSizeX + x + vSizeX + 1;

                    triangles[triangleOffset + 3] = z * vSizeX + x +          0;
                    triangles[triangleOffset + 4] = z * vSizeX + x + vSizeX + 1;
                    triangles[triangleOffset + 5] = z * vSizeX + x +          1;
                }
            }

            Mesh mesh = new Mesh();

            mesh.Clear();
            mesh.vertices = points;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            mesh.MarkDynamic();

            return mesh;
        }

        /// <summary>
        /// 
        /// Returns height from the heightmap
        /// 
        /// </summary>
        /// <param name="heightMap">Actual Heightmap</param>
        /// <param name="heightStrength">Height Multiplier</param>
        /// <param name="vSizeX">Number of vertices in X-axis</param>
        /// <param name="vSizeZ">Number of vertices in Z-axis</param>
        /// <param name="z">Current z position</param>
        /// <param name="x">Current x position</param>
        /// <returns>Height value from the heightmap</returns>
        private float GetHeightFromMap(Texture2D heightMap, float heightStrength, int vSizeX, int vSizeZ, int z, int x) {
            float colorX = ((float)x / vSizeX) * heightMap.width;
            float colorZ = ((float)z / vSizeZ) * heightMap.height;

            float heightPixel = heightMap.GetPixel((int)colorX, (int)colorZ).grayscale;
            float height = heightPixel * heightStrength;

            return height;
        }
    }
}