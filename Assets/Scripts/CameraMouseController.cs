using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool isDraggingCamera = false;
    Vector3 lastMousePosition;
    // Update is called once per frame
    void Update()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        float rayLength = mouseRay.origin.y / mouseRay.direction.y;
        Vector3 hitPos = mouseRay.origin - (mouseRay.direction * rayLength);

        if (Input.GetMouseButtonDown(1))
        {

            isDraggingCamera = true;
            lastMousePosition = hitPos;
        }     
        else if (Input.GetMouseButtonUp(1))
        {
            isDraggingCamera = false;
        }

        if (isDraggingCamera)
        {
            Vector3 diff = lastMousePosition - hitPos;
            Camera.main.transform.Translate(diff, Space.World);
            
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            rayLength = mouseRay.origin.y / mouseRay.direction.y;
            lastMousePosition = mouseRay.origin - (mouseRay.direction * rayLength);
        }

        float scrollAmount = -Input.GetAxis("Mouse ScrollWheel");
        if(Mathf.Abs(scrollAmount) > 0.01f)
        {
            Vector3 dir = Camera.main.transform.position - hitPos;

            Camera.main.transform.Translate(dir * scrollAmount, Space.World);
        }
    }
}
