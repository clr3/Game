using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Update_CurrentFunc = Update_DetectModeStart;
    }
    //Generic Variables
    Vector3 LastMousePosition;

    //Camera Dragging variabales 
    Vector3 LastMouseGroundPlanePosition;
    Vector3 cameraTargetOffset;

    Unit selectedUnit = null;

    delegate void UpdateFunc();
    UpdateFunc Update_CurrentFunc;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)){
            Debug.Log("Cancelling camera drag");
            CancelUpdateFunc();
        }

        Update_CurrentFunc();
        // Always do camera zooms
        Update_ScrollZoom();

        LastMousePosition =  Input.mousePosition;
    }
    
    void CancelUpdateFunc()
    {
        Update_CurrentFunc = Update_DetectModeStart;
        //Also do cleanup of ui associated with modes
    }
    void Update_DetectModeStart()
    {

        if (Input.GetMouseButtonDown(1))
        {
            //RMB went down;
        }
        else if (Input.GetMouseButtonUp(1))
        {
           // Debug.Log("MOUSE UP");
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

    Vector3 MouseToGroundPlane(Vector3 mousePos)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);

        float rayLength = mouseRay.origin.y / mouseRay.direction.y;
        return mouseRay.origin - (mouseRay.direction * rayLength);
    }
    void Update_UnitMovement()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("complete unit movement");
            //TODO: pathfinding

            CancelUpdateFunc();
            return;
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
