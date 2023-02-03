using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Painter : MonoBehaviour
{
    //place these where you would normally declare variables
    Terrain targetTerrain; //The terrain obj you want to edit
    float[,] terrainHeightMap;  //a 2d array of floats to store
    int terrainHeightMapWidth; //Used to calculate click position
    int terrainHeightMapHeight;
    float[,] heights; //a variable to store the new heights
    TerrainData targetTerrainData; // stores the terrains terrain data
    public enum EffectType
    {
        raise,
        lower,
        flatten,
        smooth,
        paint,
    };
    public Texture2D[] brushIMG; // This will allow you to switch brushes
    float[,] brush; // this stores the brush.png pixel data
    public int brushSelection; // current selected brush
    public int areaOfEffectSizeX = 15; // size of the brush
    public int areaOfEffectSizeZ = 15; // size of the brush
    [FormerlySerializedAs("strength")]
    [Range(0.01f,1f)] // you can remove this if you want
    public float _strength; // brush strength
    public float flattenHeight = 0; // the height to which the flatten mode will go
    public EffectType effectType;
    public TerrainLayer[] paints;// a list containing all of the paints
    public int paint; // variable to select paint
    float[,,] splat; // A splat map is what unity uses to overlay all of your paints on to the terrain

    void Awake()
    {
        brush = GenerateBrush(brushIMG[brushSelection], areaOfEffectSizeZ, areaOfEffectSizeX); // This will take the brush image from our array and will resize it to the area of effect
        targetTerrain = FindObjectOfType<Terrain>(); // this will find terrain in your scene, alternatively, if you know you will only have one terrain, you can make it a public variable and assign it that way

        targetTerrain.terrainData = TerrainDataCloner.Clone(targetTerrain.terrainData);
        targetTerrain.GetComponent<TerrainCollider>().terrainData = targetTerrain.terrainData; // Don't forget to update the TerrainCollider as well
        SetEditValues(targetTerrain);

        // int bufferX = areaOfEffectSizeX;
        // int bufferZ = areaOfEffectSizeZ;
        //
        // areaOfEffectSizeX = terrainHeightMapWidth;
        // areaOfEffectSizeZ = terrainHeightMapHeight;
        // ModifyTerrain(terrainHeightMapWidth / 2, terrainHeightMapHeight / 2, 0);
        //
        // areaOfEffectSizeX = bufferX;
        // areaOfEffectSizeZ = bufferZ;
    }

    public Terrain GetTerrainAtObject(GameObject gameObject)
    {
        return GetComponent<Terrain>();
        if (gameObject.GetComponent<Terrain>())
        {
            //This will return the Terrain component of an object (if present)
            return gameObject.GetComponent<Terrain>();
        }
        return default(Terrain);
    }

    public TerrainData GetCurrentTerrainData()
    {
        if (targetTerrain)
        {
            return targetTerrain.terrainData;
        }
        return default(TerrainData);
    }

    public Terrain GetCurrentTerrain()
    {
        if (targetTerrain)
        {
            return targetTerrain;
        }
        return default(Terrain);
    }

    public void SetEditValues(Terrain terrain)
    {
        targetTerrainData = GetCurrentTerrainData();
        terrainHeightMap = GetCurrentTerrainHeightMap();
        terrainHeightMapWidth = GetCurrentTerrainWidth();
        terrainHeightMapHeight = GetCurrentTerrainHeight();
    }

    private void GetTerrainCoordinates(RaycastHit hit, out int x,out int z)
    {
        int offset = areaOfEffectSizeX / 2; //This offsets the hit position to account for the size of the brush which gets drawn from the corner out
        //World Position Offset Coords, these can differ from the terrain coords if the terrain object is not at (0,0,0)
        Vector3 tempTerrainCoodinates = hit.point - targetTerrain.transform.position;
        //This takes the world coords and makes them relative to the terrain
        Vector3 terrainSize = GetTerrainSize();
        Vector3 terrainCoordinates = new Vector3(
            tempTerrainCoodinates.x / terrainSize.x,
            tempTerrainCoodinates.y / terrainSize.y,
            tempTerrainCoodinates.z / terrainSize.z);
        // This will take the coords relative to the terrain and make them relative to the height map(which often has different dimensions)
        Vector3 locationInTerrain = new Vector3
        (
            terrainCoordinates.x * terrainHeightMapWidth,
            0,
            terrainCoordinates.z * terrainHeightMapHeight
        );
        //Finally, this will spit out the X Y values for use in other parts of the code
        x = (int)locationInTerrain.x - offset;
        z = (int)locationInTerrain.z - offset;
    }

    private float GetSurroundingHeights(float[,] height,int x, int z)
    {
        float value; // this will temporarily hold the value at each point
        float avg = height[x, z]; // we will add all the heights to this and divide by int num bellow to get the average height
        int num = 1;
        for (int i = 0; i < 4; i++) //this will loop us through the possible surrounding spots
        {
            try // This will try to run the code bellow, and if one of the coords is not on the terrain(ie we are at an edge) it will pass the exception to the Catch{} below
            {
                // These give us the values surrounding the point
                if (i == 0)
                {value = height[x + 1, z];}
                else if (i == 1)
                {value = height[x - 1, z];}
                else if (i == 2)
                {value = height[x, z + 1];}
                else
                {value = height[x, z - 1];}
                num++; // keeps track of how many iterations were successful
                avg += value;
            }
            catch (System.Exception)
            {
            }
        }
        avg = avg / num;
        return avg;
    }

    public Vector3 GetTerrainSize()
    {
        if (targetTerrain)
        {
            return targetTerrain.terrainData.size;
        }
        return Vector3.zero;
    }

    public float[,] GetCurrentTerrainHeightMap()
    {
        if (targetTerrain)
        {
            // the first 2 0's indicate the coords where we start, the next values indicate how far we extend the area, so what we are saying here is I want the heights starting at the Origin and extending the entire width and height of the terrain
            return targetTerrain.terrainData.GetHeights(0, 0,
                targetTerrain.terrainData.heightmapResolution,
                targetTerrain.terrainData.heightmapResolution);
        }
        return default(float[,]);
    }

    public int GetCurrentTerrainWidth()
    {
        if (targetTerrain)
        {
            return targetTerrain.terrainData.heightmapResolution;
        }
        return 0;
    }
    public int GetCurrentTerrainHeight()
    {
        if (targetTerrain)
        {
            return targetTerrain.terrainData.heightmapResolution;
        }
        return 0;
        //test2.GetComponent<MeshRenderer>().material.mainTexture = texture;
    }

    public float[,] GenerateBrush(Texture2D texture, int sizeX, int sizeZ)
    {
        float[,] heightMap = new float[sizeX,sizeZ];//creates a 2d array which will store our brush
        Texture2D scaledBrush = ResizeBrush(texture,sizeX,sizeZ); // this calls a function which we will write next, and resizes the brush image
        //This will iterate over the entire re-scaled image and convert the pixel color into a value between 0 and 1
        for (int x = 0; x < sizeX; x++)
        {
            for(int y = 0; y < sizeZ; y++)
            {
                Color pixelValue = scaledBrush.GetPixel(x, y);
                heightMap[x, y] = pixelValue.grayscale / 255;
            }
        }

        return heightMap;
    }

    public static Texture2D ResizeBrush(Texture2D src, int width, int height, FilterMode mode = FilterMode.Trilinear)
    {
        Rect texR = new Rect(0, 0, width, height);
        _gpu_scale(src, width, height, mode);
        //Get rendered data back to a new texture
        Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, true);
        result.Reinitialize(width, height);
        result.ReadPixels(texR, 0, 0, true);
        return result;
    }
    static void _gpu_scale(Texture2D src, int width, int height, FilterMode fmode)
    {
        //We need the source texture in VRAM because we render with it
        src.filterMode = fmode;
        src.Apply(true);
        //Using RTT for best quality and performance. Thanks, Unity 5
        RenderTexture rtt = new RenderTexture(width, height, 32);
        //Set the RTT in order to render to it
        Graphics.SetRenderTarget(rtt);
        //Setup 2D matrix in range 0..1, so nobody needs to care about sized
        GL.LoadPixelMatrix(0, 1, 1, 0);
        //Then clear & draw the texture to fill the entire RTT.
        GL.Clear(true, true, new Color(0, 0, 0, 0));
        Graphics.DrawTexture(new Rect(0, 0, 1, 1), src);
    }

    public void SetPaint(int num)
    {
        paint = num;
    }
    public void SetLayers(TerrainData t)
    {
        t.terrainLayers = paints;
    }
    public void SetBrushSize(int value)//adds int value to brush size(make negative to shrink)
    {
        areaOfEffectSizeX += value;
        if (areaOfEffectSizeX > 50)
        { areaOfEffectSizeX = 50; }
        else if(areaOfEffectSizeX < 1)
        { areaOfEffectSizeX = 1; }
        brush = GenerateBrush(brushIMG[brushSelection], areaOfEffectSizeX, areaOfEffectSizeZ); // regenerates the brush with new size
    }
    public void SetBrushStrength(float value)//same idea as SetBrushSize()
    {
        _strength += value;
        if (_strength > 1)
        { _strength = 1; }
        else if (_strength < 0.01f)
        { _strength = 0.01f; }
    }
    public void SetBrush(int num)
    {
        brushSelection = num;
        brush = GenerateBrush(brushIMG[brushSelection], areaOfEffectSizeX, areaOfEffectSizeZ);
        // RMC.SetIndicators();
    }

    void ModifyTerrain(int x, int z, float strength)
 {
     //These AreaOfEffectModifier variables below will help us if we are modifying terrain that goes over the edge, you will see in a bit that I use Xmod for the the z(or Y) values, which was because I did not realize at first that the terrain X and world X is not the same so I had to flip them around and was too lazy to correct the names, so don't get thrown off by that.
     int AOExMod = 0;
     int AOEzMod = 0;
     int AOExMod1 = 0;
     int AOEzMod1 = 0;
     if (x < 0) // if the brush goes off the negative end of the x axis we set the mod == to it to offset the edited area
     {
         AOExMod = x;
     }
     else if (x + areaOfEffectSizeX > terrainHeightMapWidth)// if the brush goes off the posative end of the x axis we set the mod == to this
     {
         AOExMod1 = x + areaOfEffectSizeX - terrainHeightMapWidth;
     }

     if (z < 0)//same as with x
     {
         AOEzMod = z;
     }
     else if (z + areaOfEffectSizeZ > terrainHeightMapHeight)
     {
         AOEzMod1 = z + areaOfEffectSizeZ - terrainHeightMapHeight;
     }
     if (effectType != EffectType.paint) // the following code will apply the terrain height modifications
     {
         heights = targetTerrainData.GetHeights(x - AOExMod, z - AOEzMod, areaOfEffectSizeX + AOExMod - AOExMod1, areaOfEffectSizeX + AOEzMod - AOEzMod1); // this grabs the heightmap values within the brushes area of effect
     }
     ///Raise Terrain
     /*if (effectType == EffectType.raise)
     {
         for (int xx = 0; xx < areaOfEffectSizeX + AOEzMod - AOEzMod1; xx++)
         {
             for (int yy = 0; yy < areaOfEffectSizeX + AOExMod - AOExMod1; yy++)
             {
                 heights[xx, yy] += brush[xx-AOEzMod, yy-AOExMod] * strength; //for each point we raise the value  by the value of brush at the coords * the strength modifier
             }
         }
         targetTerrainData.SetHeights(x - AOExMod, z - AOEzMod, heights); // This bit of code will save the change to the Terrain data file, this means that the changes will persist out of play mode into the edit mode
     }
     ///Lower Terrain, just the reverse of raise terrain
     else if (effectType == EffectType.lower)
     {
         for (int xx = 0; xx < areaOfEffectSizeX + AOEzMod; xx++)
         {
             for (int yy = 0; yy < areaOfEffectSizeX + AOExMod; yy++)
             {
                 heights[xx, yy] -= brush[xx - AOEzMod, yy - AOExMod] * strength;
             }
         }
         targetTerrainData.SetHeights(x - AOExMod, z - AOEzMod, heights);
     }
     //this moves the current value towards our target value to flatten terrain
     else if (effectType == EffectType.flatten)
     {
         for (int xx = 0; xx < areaOfEffectSizeX + AOEzMod; xx++)
         {
             for (int yy = 0; yy < areaOfEffectSizeX + AOExMod; yy++)
             {
                 heights[xx, yy] = Mathf.MoveTowards(heights[xx, yy],  flattenHeight/600, brush[xx - AOEzMod, yy - AOExMod] * strength);
             }
         }
         targetTerrainData.SetHeights(x - AOExMod, z - AOEzMod, heights);
     }
     //Takes the average of surrounding points and moves the point towards that height
     else if (effectType == EffectType.smooth)
     {
         float[,] heightAvg = new float[heights.GetLength(0), heights.GetLength(1)];
         for (int xx = 0; xx < areaOfEffectSizeX + AOEzMod; xx++)
         {
             for (int yy = 0; yy < areaOfEffectSizeX + AOExMod; yy++)
             {
                 heightAvg[xx, yy] = GetSurroundingHeights(heights, xx, yy); // calculates the value we want each point to move towards
             }
         }
         for (int xx1 = 0; xx1 < areaOfEffectSizeX + AOEzMod; xx1++)
         {
             for (int yy1 = 0; yy1 < areaOfEffectSizeX + AOExMod; yy1++)
             {
                 heights[xx1, yy1] = Mathf.MoveTowards(heights[xx1, yy1], heightAvg[xx1, yy1], brush[xx1 - AOEzMod, yy1 - AOExMod] * strength); // moves the points towards their targets
             }
         }
         targetTerrainData.SetHeights(x - AOExMod, z - AOEzMod, heights);
     }*/
     //This is where we do the painting, sorry its buried so far in here
     /*else*/
     if (effectType == EffectType.paint)
     {
         splat = targetTerrain.terrainData.GetAlphamaps(x - AOExMod, z - AOEzMod, areaOfEffectSizeX + AOExMod, areaOfEffectSizeZ + AOEzMod); //grabs the splat map data for our brush area
         for (int xx = 0; xx < areaOfEffectSizeZ + AOEzMod; xx++)
         {
             for (int yy = 0; yy < areaOfEffectSizeX + AOExMod; yy++)
             {
                 float[] weights = new float[targetTerrain.terrainData.alphamapLayers]; //creates a float array and sets the size to be the number of paints your terrain has
                 for (int zz = 0; zz < splat.GetLength(2); zz++)
                 {
                     weights[zz] = splat[xx, yy, zz];//grabs the weights from the terrains splat map
                 }
                 weights[paint] += brush[xx - AOEzMod, yy - AOExMod] * strength *2000; // adds weight to the paint currently selected with the int paint variable
                 //this next bit normalizes all the weights so that they will add up to 1
                 float sum = weights.Sum();
                 for (int ww = 0; ww < weights.Length; ww++)
                 {
                     weights[ww] /= sum;
                     splat[xx, yy, ww] = weights[ww];
                 }
             }
         }
         //applies the changes to the terrain, they will also persist
         targetTerrain.terrainData.SetAlphamaps(x - AOExMod, z - AOEzMod, splat);
         targetTerrain.Flush();
     }
 }

    // void Update()
    // {
    //     if (Input.GetMouseButton(0))
    //     {
    //         Transform cam = Camera.main.transform;
    //         Ray ray = new Ray(cam.position, cam.forward);
    //         RaycastHit hit;
    //         if(Physics.Raycast (ray,out hit, 500))
    //         {
    //             targetTerrain = GetTerrainAtObject(hit.transform.gameObject);
    //             SetEditValues(targetTerrain);
    //             Debug.Log(hit.point);
    //             GetTerrainCoordinates(hit, out int terX, out int terZ);
    //             Debug.Log($"{terX} {terZ}");
    //
    //             ModifyTerrain(200, 210);
    //             // Debug.Log("painting");
    //         }
    //     }
    // }

    public void Modify(Vector3 position)
    {
        SetEditValues(targetTerrain);
        Vector3 pos = position - targetTerrain.transform.position;
        Vector3 size = GetTerrainSize();
        Vector3 pos1 = new Vector3(pos.x / size.x, 0, pos.z / size.z);
        Vector3 locationInTerrain = new Vector3
        (
            pos1.x * terrainHeightMapWidth,
            0,
            pos1.z * terrainHeightMapHeight
        );
        int offset = areaOfEffectSizeX / 2; //This offsets the hit position to account for the size of the brush which gets drawn from the corner out



        ModifyTerrain((int) locationInTerrain.x - areaOfEffectSizeX / 2, (int) locationInTerrain.z - areaOfEffectSizeZ / 2, _strength);

    }

    void OnApplicationQuit()
    {
        targetTerrainData.terrainLayers[paint] = null;
    }
}
