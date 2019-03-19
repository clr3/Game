using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using QPath;

//Defines grid position, world space position, size, neighbours of a hex tile. Does not interact with unity directly.
public class Hex : IQPathTile {

    public Hex(HexMap hexMap, int c, int r)
    {

        this.HexMap = hexMap;
        this.C = c;
        this.R = r;
        this.S = -(c + r);

    }
    // C + S + R = 0
    // S = -( C + R )

    public readonly int C;
    public readonly int R;
    public readonly int S;

    // Data for map generation & in game effects?
    public float Elevation;
    //TODO: This is just a temp public value
    public int MovementCost = 1;
    //TODO: Property to track hex type
    //TODO: Property to track hex detail

    public readonly HexMap HexMap;

    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

    float radius = 1f;

    HashSet<Unit> units;

    public override string ToString()
    {
        return C + "," + R;
    }

    bool allowWrapEastWest = true;
    bool allowWrapNorthSouth = false;

    public Vector3 Position()
    {
        return new Vector3(
            HexHorizontalSpacing() * (this.C + this.R/2f),
            0, 
            HexVerticalSpacing() * this.R
            );
    }

    public float HexHeight()
    {
        return radius * 2;
    }

    public float HexWidth()
    {
        return WIDTH_MULTIPLIER * HexHeight();
    }

    public float HexVerticalSpacing()
    {
        return HexHeight() * 0.75f;
    }

    public float HexHorizontalSpacing()
    {
        return HexWidth();
    }

    public Vector3 PositionFromCamera()
    {
        return HexMap.GetHexPosition(this);
    }

    public Vector3 PositionFromCamera(Vector3 cameraPosition, float numRows, float numColumns)
    {
        float mapHeight = numRows * HexVerticalSpacing();
        float mapWidth = numColumns * HexHorizontalSpacing();

        Vector3 position = Position();

        if (allowWrapEastWest)
        {
            float howManyWidthsFromCamera = (position.x - cameraPosition.x) / mapWidth;
            // We want howManyWidthsFromCamera to be between -0.5 to 0.5


            if (howManyWidthsFromCamera > 0) howManyWidthsFromCamera += 0.5f;

            else howManyWidthsFromCamera -= 0.5f;

            int howManyWidthsToFix = (int)howManyWidthsFromCamera;

            position.x -= howManyWidthsToFix * mapWidth;

        }

        if (allowWrapNorthSouth)
        {
            float howManyHeightsFromCamera = (position.z - cameraPosition.z) / mapHeight;
            // We want howManyWidthsFromCamera to be between -0.5 to 0.5


            if (howManyHeightsFromCamera > 0) howManyHeightsFromCamera += 0.5f;

            else howManyHeightsFromCamera -= 0.5f;

            int howManyHeightsToFix = (int)howManyHeightsFromCamera;

            position.z -= howManyHeightsToFix * mapHeight;

        }
        return position;
    }
    
    public static float CostEstimate(IQPathTile aa, IQPathTile bb)
    {
        return Distance((Hex)aa, (Hex)bb);
    }
    public static float Distance(Hex a, Hex b)
    {
        //FIX : Wrapping
        return Mathf.Max(Mathf.Abs(a.C - b.C), Mathf.Abs(a.R - b.R), Mathf.Abs(a.S - b.S));
    }

    public static float DistanceWrapEastWest(Hex a, Hex b, int NumColumns)
    {
        return Mathf.Max(Mathf.Abs(a.C - b.C), Mathf.Abs(a.R - b.R), Mathf.Abs(a.S - b.S));
    }

    public void AddUnit (Unit unit)
    {
        if(units == null)
        {
            units = new HashSet<Unit>();
        }
        units.Add(unit);
    }

    public void RemoveUnit (Unit unit)
    {
        if(units != null)
        {
            units.Remove(unit);
        }
    }

    public Unit[] Units()
    {
        return units.ToArray();
    }

    public int BaseMovementCost()
    {
        //TODO: Factor in terrain type & features
        return MovementCost;
    }

    Hex[] neighbours; 

    public IQPathTile[] GetNeighbours()
    {
        if(this.neighbours != null)
        {
            return this.neighbours;
        }
        List<Hex> neighbours = new List<Hex>();

        neighbours.Add(HexMap.GetHexAt( C + 1, R + 0));
        neighbours.Add(HexMap.GetHexAt( C + -1, R + 0));
        neighbours.Add(HexMap.GetHexAt( C + 0, R + 1));
        neighbours.Add(HexMap.GetHexAt( C + 0, R + -1));
        neighbours.Add(HexMap.GetHexAt( C + 1, R + -1));
        neighbours.Add(HexMap.GetHexAt( C + -1, R + 1));

        List<Hex> neighbours2 = new List<Hex>();

        foreach(Hex h in neighbours)
        {
            if(h != null)
            {
                neighbours2.Add(h);
            }
        }
        this.neighbours = neighbours2.ToArray();
        return this.neighbours;
    }

    public float AggregateCostToEnter(float costSoFar, IQPathTile sourceTile, IQPathUnit theUnit)
    {
        //TODO : Ignoring source tile right now, change when we have rivers
        return ((Unit)theUnit).AggregateTurnsToEnterHex(this,costSoFar);
    }
}
