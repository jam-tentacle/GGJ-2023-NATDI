using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetsCollection : Service
{
    public LineRenderer LinePrefab;
    public Projectile SporePrefab;
    public GameSettings Settings;
    public MushroomArea DefaultMushroomArea;
    public Mushroom DefaultMushroom;
    public List<MushroomAreaByTerrain> MushroomAreasByTerrains;

    [Serializable]
    public class MushroomAreaByTerrain
    {
        public TerrainLayerType Terrain;
        public MushroomArea MushroomArea;
        public Mushroom Mushroom;
    }

    public MushroomArea GetMushroomAreaByTerrain(TerrainLayerType terrain)
    {
        return MushroomAreasByTerrains.FirstOrDefault(m => m.Terrain == terrain)?.MushroomArea ?? DefaultMushroomArea;
    }

    public Mushroom GetMushroomByTerrain(TerrainLayerType terrain)
    {
        return MushroomAreasByTerrains.FirstOrDefault(m => m.Terrain == terrain)?.Mushroom ?? DefaultMushroom;
    }
}
