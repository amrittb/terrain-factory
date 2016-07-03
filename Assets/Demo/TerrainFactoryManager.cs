using UnityEngine;
using TerrainFactory;

public class TerrainFactoryManager : MonoBehaviour {

    /// <summary>
    /// 
    /// Number of tiles in x-axis of the terrain
    /// 
    /// </summary>
    public int numTilesX = 254;

    /// <summary>
    /// 
    /// Number of tiles in z-axis of the terrain
    /// 
    /// </summary>
    public int numTilesZ = 254;

    /// <summary>
    /// 
    /// Each tile size
    /// 
    /// </summary>
    public float tileSize = 20f;

    /// <summary>
    /// 
    /// Height strength
    /// 
    /// </summary>
    public float heightStrength = 1400f;

    /// <summary>
    /// 
    /// First Heightmap
    /// 
    /// </summary>
    public Texture2D firstHeightMap;

    /// <summary>
    /// 
    /// Second HeightMap
    /// 
    /// </summary>
    public Texture2D secondHeightMap;

    /// <summary>
    /// 
    /// First TerrainMaterial
    /// 
    /// </summary>
    public Material firstTerrainMaterial;

    /// <summary>
    /// 
    /// Second TerrainMaterial
    /// 
    /// </summary>
    public Material secondTerrainMaterial;

    /// <summary>
    /// 
    /// TerrainFactory instance
    /// 
    /// </summary>
    private Factory factory;

    /// <summary>
    /// 
    /// Terrain data instance
    /// 
    /// </summary>
    private TerrainFactory.TerrainData data;
    
    /// <summary>
    /// 
    /// Actual HeightMap terrain
    /// 
    /// </summary>
    private HeightMapTerrain terrain;

    // Use this for initialization
    void Start () {
        // Instanciating the terrain factory.
        factory = new Factory();

        // Instanciating the terrain data
        data = new TerrainFactory.TerrainData(numTilesX, numTilesZ, tileSize, heightStrength, firstHeightMap, firstTerrainMaterial);

        // Building terrain mesh
        terrain = factory.BuildMesh(data);
	}
	
	// Update is called once per frame
	void OnGUI () {
        if (GUI.Button(new Rect(10,20,100,50),"Update")) {
            data.heightMap = secondHeightMap;
            data.terrainMaterial = secondTerrainMaterial;
            factory.UpdateMesh(data, terrain);
        }
	}
}
