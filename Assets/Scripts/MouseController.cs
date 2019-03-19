using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Update_CurrentFunc = Update_DetectModeStart;

        hexMap = GameObject.FindObjectOfType<HexMap>();

        LineRenderer = transform.GetComponentInChildren<LineRenderer>();
    }
    //Generic Variables
    HexMap hexMap;
    Hex hexUnderMouse;
    Hex hexLastUnderMouse;
    Vector3 LastMousePosition;

    //Camera Dragging variabales 
    Vector3 LastMouseGroundPlanePosition;
    Vector3 cameraTargetOffset;

    Unit selectedUnit = null;
    Hex[] hexPath;
    LineRenderer LineRenderer;

    delegate void UpdateFunc();
    UpdateFunc Update_CurrentFunc;

    public LayerMask LayerIDForHexTiles;

    void Update()
    {
        hexUnderMouse = MouseToHex();
        
        if (Input.GetKeyDown(KeyCode.Escape)){
            Debug.Log("Cancelling camera drag");
            CancelUpdateFunc();
        }

        Update_CurrentFunc();
        // Always do camera zooms
        Update_ScrollZoom();

        LastMousePosition =  Input.mousePosition;
        hexLastUnderMouse = hexUnderMouse;

        if(selectedUnit != null)
        {
            DrawPath((hexPath != null) ? hexPath: selectedUnit.GetHexPath());
        }
        else
        {
            DrawPath(null);
        }
    }

    void DrawPath(Hex[] hexPath)
    {
        if (hexPath == null || hexPath.Length == 0)
        {
            LineRenderer.enabled = false;
            return;
        }
        LineRenderer.enabled = true;
        Vector3[] ps = new Vector3[hexPath.Length];
        for (int i = 0; i < hexPath.Length; i++)
        {
            GameObject hexGO = hexMap.GetHexGO(hexPath[i]);
            ps[i] = hexGO.transform.position + Vector3.up * 0.5f;
        }
        LineRenderer.positionCount = ps.Length;
        LineRenderer.SetPositions(ps);
        //DrawPath(hexPath);
    }

    void CancelUpdateFunc()
    {
        Update_CurrentFunc = Update_DetectModeStart;
        //Also do cleanup of ui associated with modes
        selectedUnit = null;

        hexPath = null;
    }
    void Update_DetectModeStart()
    {

        if (Input.GetMouseButtonDown(1))
        {
            //LMB went down;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Debug.Log("MOUSE UP");
            Unit[] us = hexUnderMouse.Units();

            //TODO: Cycling through multiple units in same tile
            if(us.Length > 0 )
            {
                selectedUnit = us[0];
            }
           
        }

        else if ( selectedUnit != null && Input.GetMouseButtonDown(0))
        {
            Update_CurrentFunc = Update_UnitMovement;

        }

        else if (Input.GetMouseButton(1) && Input.mousePosition != LastMousePosition)
        {
            //RMB held down and camera drag;
            Update_CurrentFunc = Update_CameraDrag;
            LastMouseGroundPlanePosition = MouseToGroundPlane(Input.mousePosition);
            Update_CurrentFunc();
        }
        else if (selectedUnit != null && Input.GetMouseButton(0))
        {
            //we have a selected unit, and holding down LMB. show path from unit to mouse position
        }
    }
    Hex MouseToHex()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        int layerMask = LayerIDForHexTiles.value;

        if(Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity,layerMask))
        {
            //Debug.Log(hitInfo.collider.name);

            // Collider is a child of the "correct" game object that we want.
            GameObject hexGO = hitInfo.rigidbody.gameObject;
            return hexMap.GetHexFromGameObject(hexGO);
        }
        Debug.Log("found nothing");

        return null;
    }

    Vector3 MouseToGroundPlane(Vector3 mousePos)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);

        float rayLength = mouseRay.origin.y / mouseRay.direction.y;
        return mouseRay.origin - (mouseRay.direction * rayLength);
    }

    void Update_UnitMovement()
    {
        if (Input.GetMouseButtonUp(0) || selectedUnit == null)
        {
            Debug.Log("complete unit movement");

            if(selectedUnit != null)
            {
                selectedUnit.SetHexPath(hexPath);
            }

            CancelUpdateFunc();
            return;
        }

        if (hexPath == null || hexUnderMouse != hexLastUnderMouse)
        {
            hexPath = QPath.QPath.FindPath<Hex>(hexMap, selectedUnit, selectedUnit.Hex, hexUnderMouse, Hex.CostEstimate);

           
        }

    }



    void Update_CameraDrag()
    {
        if (Input.GetMouseButtonUp(1))
        {
            CancelUpdateFunc();
            return;
        }

        Vector3 hitPos = MouseToGroundPlane(Input.mousePosition);

        Vector3 diff = LastMouseGroundPlanePosition - hitPos;
        Camera.main.transform.Translate(diff, Space.World);

        LastMouseGroundPlanePosition = hitPos = MouseToGroundPlane(Input.mousePosition);


    }

    void Update_ScrollZoom()
    {
        float scrollAmount = -Input.GetAxis("Mouse ScrollWheel");
        Vector3 hitPos = MouseToGroundPlane(Input.mousePosition);

        if (Mathf.Abs(scrollAmount) > 0.01f)
        {
            Vector3 dir = Camera.main.transform.position - hitPos;

            Camera.main.transform.Translate(dir * scrollAmount, Space.World);
        }
    }
}
