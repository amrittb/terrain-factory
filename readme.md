# Terrain Factory

Terrain Factory is an Unity package to generate procedural mesh from height maps.

## Usage

* Create `Factory` object anywhere where you need.
* Create `TerrainData` object with number of tiles in each axis, each tile size, height map, height multiplier and terrain material
* Pass factory the data to build the mesh.

```cs

TerrainFactory.Factory factory = new TerrainFactory.Factory();

TerrainFactory.TerrainData data = new TerrainFactory.TerrainData(
                                    numTilesX,
                                    numTilesZ,
                                    tileSize,
                                    heightStrength,
                                    firstHeightMap,
                                    firstTerrainMaterial
                                    );

factory.BuildMesh(data);

```

## Demo

The demo scene is in demo folder. See the demo scene for more usage options
