using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(units != null)
            {
                foreach(Unit u in units)
                {
                    u.DoTurn();
                }
            }
        }
    }
    public GameObject HexPrefab;

    public Mesh MeshMountain;
    public Mesh MeshHill;
    public Mesh MeshLand;
    public Mesh MeshFlat;
    public Mesh MeshWater;
    public Mesh MeshSwamp;

    public GameObject CityPrefab;
    public GameObject GrassPrefab;

    public Material MatMountains;
    public Material MatLand;
    public Material MatSwamp;
    public Material MatPlains;
    public Material MatGrasslands;  
    public Material MatRiver;
    public Material MatHome;

    public GameObject UnitDwarfPrehab;

    [System.NonSerialized] public float HeightMountain = 1f;
    [System.NonSerialized] public float HeightLand = 0.7f;
    [System.NonSerialized] public float HeightSwamp = 0.4f;
    [System.NonSerialized] public float HeightPlains = 0.2f;
    [System.NonSerialized] public float HeightGrasslands = -0.5f;
    [System.NonSerialized] public float HeightRiver = -1f;

    //  private int mapsize = 30;
    [System.NonSerialized] public int numColumns = 30;
    [System.NonSerialized] public int numRows = 30;


    //TODO link up with Hex.cs version of these variables
    [System.NonSerialized] bool allowWrapEastWest = true;
    [System.NonSerialized] bool allowWrapNorthSouth = false;

    private Hex[,] hexes;
    private Dictionary<Hex, GameObject> hexToGameObjectMap;

    private HashSet<Unit> units;
    private Dictionary<Unit, GameObject> unitToGameObjectMap;

    public Hex GetHexAt(int x, int y)
    {
        if (hexes == null)
        {
            Debug.LogError("Hexes array not yet instantiated");
            return null;
        }

        if (allowWrapEastWest)
        {
            x = x % numColumns;
            if (x < 0) x += numColumns;
        }
        if (allowWrapNorthSouth)
        {
            y = y % numRows;
            if (y < 0) y += numRows;
        }
        return hexes[x, y];
    }
    virtual public void GenerateMap()
    {

        //Generate map filled with land

        hexes = new Hex[numColumns, numRows];
        hexToGameObjectMap = new Dictionary<Hex, GameObject>();

        for (int column = 0; column < numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                //Instantiate hex
                Hex h = new Hex(column, row);
                h.Elevation = 0f;

                hexes[column, row] = h;

                Vector3 pos = h.PositionFromCamera(
                    Camera.main.transform.position,
                    numRows,
                    numColumns
                    ); 

                GameObject hexGO = (GameObject)Instantiate(HexPrefab, pos, Quaternion.identity, this.transform);

                hexToGameObjectMap[h] = hexGO;

                hexGO.name = string.Format("HEX: {0},{1}", column, row);
                hexGO.GetComponent<HexComponent>().Hex = h;
                hexGO.GetComponent<HexComponent>().HexMap = this;

                hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);


            }
        }

        

        UpdateHexVisuals();

        Unit unit = new Unit();
        SpawnUnitAt(unit, UnitDwarfPrehab, numColumns / 2, numRows / 2);

    }
    public void UpdateHexVisuals()
    {
        for (int column = 0; column < numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                Hex h = hexes[column, row];
                GameObject hexGO = hexToGameObjectMap[h];

                MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
                MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();
                
                if (h.Elevation >= HeightMountain)
                {
                    mr.material = MatMountains;
                    mf.mesh = MeshMountain;
                }
                else if (h.Elevation >= HeightLand)
                {
                    mr.material = MatLand;
                    mf.mesh = MeshLand;
                }
                else if (h.Elevation >= HeightSwamp)
                {
                    mr.material = MatSwamp;
                    mf.mesh = MeshSwamp;
                }
                else if (h.Elevation >= HeightPlains)
                {
                    mr.material = MatPlains ;
                    mf.mesh = MeshHill;
                }
                else if (h.Elevation >= HeightGrasslands)
                {
                    mr.material = MatGrasslands;
                    mf.mesh = MeshFlat;
                   // GameObject.Instantiate(GrassPrefab, hexGO.transform.position, Quaternion.identity, hexGO.transform);
                }
                else
                {
                    mr.material = MatRiver;
                }

                if (column == numColumns/2 && row == numRows/2)
                {
                    mr.material = MatHome;
                    // spawn city
                    GameObject.Instantiate(CityPrefab, hexGO.transform.position, Quaternion.identity, hexGO.transform);
                    
                }

                //HexMaterials[Random.Range(0, HexMaterials.Length)];
             //   hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0}", h.Elevation);
            }
        }
    }

    public Hex[] GetHexesWithinRangeOf(Hex centreHex, int range)
    {
        List<Hex> results = new List<Hex>();
        for (int dx = -range; dx < range-1; dx++)
        {
            for (int dy = Mathf.Max(-range+1,-dx-range);dy < Mathf.Min(range, -dx + range-1); dy++)
            {
                results.Add(GetHexAt(centreHex.C + dx, centreHex.R + dy));

            }
        }
        return results.ToArray();
    }

    public void SpawnUnitAt(Unit unit, GameObject prehab, int q, int r)
    {
        if(units == null){
            units = new HashSet<Unit>();
            unitToGameObjectMap = new Dictionary<Unit, GameObject>();
        }

        GameObject myHexGO = hexToGameObjectMap[GetHexAt(q,r)];
        unit.setHex(GetHexAt(q, r));
        GameObject unitGO = Instantiate(prehab, myHexGO.transform.position, Quaternion.identity, myHexGO.transform);

        units.Add(unit);
        unitToGameObjectMap[unit] = unitGO;
    }
}
