﻿using QPath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class HexMap : MonoBehaviour, IHexPathWorld
    {
        // Start is called before the first frame update
        void Start()
        {
            GenerateMap();
        }

        public bool animationIsPlaying = false;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (Unit u in units) u.RefreshMovement();
                StartCoroutine(DoAllUnitMoves());
            }
        }

        IEnumerator DoAllUnitMoves()
        {
            if (units != null)
            {
                foreach (Unit u in units)
                {
                    yield return DoUnitMoves(u);

                }
            }
        }

        public IEnumerator DoUnitMoves(Unit u)
        {
            while (u.DoMove())
            {
                Debug.Log("DoMove returned true");
                //TODO: Check to see if animation is playing, if so wait for it to finish.
                while (animationIsPlaying)
                {
                    yield return null;
                }
            }
        }
        public void EndTurn()
        {

            foreach (Unit u in units)
            {
                u.RefreshMovement();
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
        private Dictionary<GameObject, Hex> gameObjectToHexMap;


        public HashSet<Unit> units;
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
            try
            {
                return hexes[x, y];
            }
            catch
            {
                Debug.LogError("GetHexAt:" + x + "," + y);
                return null;
            }

        }

        public Hex GetHexFromGameObject(GameObject hexGO)
        {
            if (gameObjectToHexMap.ContainsKey(hexGO))
            {
                return gameObjectToHexMap[hexGO];
            }
            return null;
        }

        public GameObject GetHexGO(Hex h)
        {
            if (hexToGameObjectMap.ContainsKey(h))
            {
                return hexToGameObjectMap[h];
            }
            return null;
        }
        public Vector3 GetHexPosition(int q, int r)
        {
            Hex hex = GetHexAt(q, r);

            return GetHexPosition(hex);
        }

        public Vector3 GetHexPosition(Hex hex)
        {
            return hex.PositionFromCamera(Camera.main.transform.position, numRows, numColumns);
        }

        virtual public void GenerateMap()
        {

            //Generate map filled with land

            hexes = new Hex[numColumns, numRows];
            hexToGameObjectMap = new Dictionary<Hex, GameObject>();
            gameObjectToHexMap = new Dictionary<GameObject, Hex>();

            for (int column = 0; column < numColumns; column++)
            {
                for (int row = 0; row < numRows; row++)
                {
                    //Instantiate hex
                    Hex h = new Hex(this, column, row);
                    h.Difficulty = 0f;

                    hexes[column, row] = h;

                    Vector3 pos = h.PositionFromCamera(
                        Camera.main.transform.position,
                        numRows,
                        numColumns
                        );

                    GameObject hexGO = (GameObject)Instantiate(HexPrefab, pos, Quaternion.identity, this.transform);

                    hexToGameObjectMap[h] = hexGO;
                    gameObjectToHexMap[hexGO] = h;

                    hexGO.name = string.Format("");
                    hexGO.GetComponent<HexComponent>().Hex = h;
                    hexGO.GetComponent<HexComponent>().HexMap = this;




                }
            }



            UpdateHexvisuals();

            Unit unit = new Unit();
            SpawnUnitAt(unit, UnitDwarfPrehab, numColumns / 2, numRows / 2);

        }
        public void UpdateHexvisuals()
        {
            for (int column = 0; column < numColumns; column++)
            {
                for (int row = 0; row < numRows; row++)
                {
                    Hex h = hexes[column, row];
                    GameObject hexGO = hexToGameObjectMap[h];

                    MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
                    MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();
                    if (h.Difficulty >= HeightMountain)
                    {
                        mr.material = MatMountains;
                        mf.mesh = MeshMountain;
                        h.DifficultyType = Hex.ELEVATION_TYPE.MOUNTAIN;
                    }
                    else if (h.Difficulty >= HeightLand)
                    {
                        mr.material = MatLand;
                        mf.mesh = MeshLand;
                        h.DifficultyType = Hex.ELEVATION_TYPE.DESERT;
                    }
                    else if (h.Difficulty >= HeightSwamp)
                    {
                        mr.material = MatSwamp;
                        mf.mesh = MeshSwamp;
                        h.DifficultyType = Hex.ELEVATION_TYPE.SWAMP;
                    }
                    else if (h.Difficulty >= HeightPlains)
                    {
                        mr.material = MatPlains;
                        mf.mesh = MeshHill;
                        h.DifficultyType = Hex.ELEVATION_TYPE.PLAINS;
                    }
                    else if (h.Difficulty >= HeightGrasslands)
                    {
                        mr.material = MatGrasslands;
                        mf.mesh = MeshFlat;
                        h.DifficultyType = Hex.ELEVATION_TYPE.GRASSLANDS;
                        // GameObject.Instantiate(GrassPrefab, hexGO.transform.position, Quaternion.identity, hexGO.transform);
                    }
                    else
                    {
                        mr.material = MatRiver;
                        h.DifficultyType = Hex.ELEVATION_TYPE.RIVER;
                    }

                    if (column == numColumns / 2 && row == numRows / 2)
                    {
                        mr.material = MatHome;
                        // spawn city
                        GameObject.Instantiate(CityPrefab, hexGO.transform.position, Quaternion.identity, hexGO.transform);

                    }
                    //hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1},\n{2}", column, row, h.BaseMovementCost());

                    //hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0}", h.MovementCost);

                    //HexMaterials[Random.Range(0, HexMaterials.Length)];
                    //   hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0}", h.Difficulty);
                }
            }
        }

        public Hex[] GetHexesWithinRangeOf(Hex centreHex, int range)
        {
            List<Hex> results = new List<Hex>();
            for (int dx = -range; dx < range - 1; dx++)
            {
                for (int dy = Mathf.Max(-range + 1, -dx - range); dy < Mathf.Min(range, -dx + range - 1); dy++)
                {
                    results.Add(GetHexAt(centreHex.C + dx, centreHex.R + dy));

                }
            }
            return results.ToArray();
        }

        public void SpawnUnitAt(Unit unit, GameObject prehab, int q, int r)
        {
            if (units == null)
            {
                units = new HashSet<Unit>();
                unitToGameObjectMap = new Dictionary<Unit, GameObject>();
            }

            Hex myHex = GetHexAt(q, r);
            GameObject myHexGO = hexToGameObjectMap[GetHexAt(q, r)];
            unit.SetHex(myHex);

            GameObject unitGO = (GameObject)Instantiate(prehab, myHexGO.transform.position, Quaternion.identity, myHexGO.transform);
            unit.OnUnitMoved += unitGO.GetComponent<UnitView>().OnUnitMoved;

            units.Add(unit);
            unitToGameObjectMap[unit] = unitGO;
        }
    }
