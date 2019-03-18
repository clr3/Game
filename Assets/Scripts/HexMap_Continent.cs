using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap_Continent : HexMap
{
    // Start is called before the first frame update
    public override void GenerateMap()
    {
        base.GenerateMap();

        //Uncomment to get same map
        //Random.InitState(0);
        // Hex midHex = new Hex(numColumns*3/4, numRows / 2);
        Hex midHex = new Hex(numColumns /2, numRows / 2);

        int numContinents = 20;
        int continentSpacing = numColumns / numContinents;

        for (int c = 0; c < numContinents; c++)
        {
            int numSplats = Random.Range(4, 8);
            for (int i = 0; i < numSplats; i++)
            {
                int range = Random.Range(5, 8);
                int y = Random.Range(range, numRows - range);
                int x = Random.Range(0, 10) - y / 2 + (c * continentSpacing);

                ElevateArea(x, y, range);
            }
        }

        //parameters for noise
        float noiseResolution = 0.1f;
        Vector2 noiseOffSet = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        
        float noiseScale = 3f; //bigger scale = smaller "continents"

        for (int column = 0; column<numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                Hex h = GetHexAt(column, row);
                float n =
                    Mathf.PerlinNoise(((float)column / numColumns / noiseResolution) + noiseOffSet.x,
                    ((float)row / numRows / noiseResolution) + noiseOffSet.y) % 0.5f
                    - 0.5f;  

                h.Elevation += n * noiseScale;

                if (Hex.Distance(h, midHex) <= numRows / 8) h.Elevation %= 0.4f;

                else if (Hex.Distance(h, midHex) <= numRows * 2 / 8)
                {
                    //h.Elevation = 2f;
                    if (h.Elevation < 0.2f) h.Elevation += 0.5f;
                }

                else
                {
                    h.Elevation = Mathf.Abs(h.Elevation) + 0.2f;
                }

            }
        }
        
        UpdateHexVisuals();
    }

    void ElevateArea(int q, int r, int range, float centreHeight = 0.5f)
    {

        Hex centreHex = GetHexAt(q, r);

        Hex[] areaHexes = GetHexesWithinRangeOf(centreHex, range);

        foreach(Hex h in areaHexes)
        {
           // if (h.Elevation < 0) h.Elevation = 0;
            h.Elevation = centreHeight * Mathf.Lerp(1f,0.0f, Mathf.Pow(Hex.Distance(centreHex, h)/range, 2));
        }
    }
}
