using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        oldPosition = this.transform.position;
    }

    Vector3 oldPosition;

    // Update is called once per frame
    void Update()
    {
        //TODO: Code to click and drag camera
        //      WSAD
        //      Zoom 
        checkIfCameraMoved();
    }

    public void PanToHex(Hex hex)
    {
        //TODO: Move camera to hex
    }

    HexComponent[] hexes;

    void checkIfCameraMoved()
    {
        
        if(oldPosition != this.transform.position)
        {
            oldPosition = this.transform.position;
            if(hexes == null)
                hexes = GameObject.FindObjectsOfType<HexComponent>();             
                
            foreach (HexComponent hex in hexes)
            {
                hex.UpdatePosition();
            }
        }
    }
}
