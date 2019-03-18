using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitView : MonoBehaviour
{

    Vector3 oldPosition;
    Vector3 newPosition;
    public void OnUnitMoved(Hex old, Hex newHex)
    {
        // This GameObject is supposed to be a child of the hex we are standing in
        //This ensures that we are in the correct place in the hierarchy.
        //Our correct position when we aren't moving, is to be at 0,0 local position 
        // relative to our parent.


        oldPosition = old.Position();
        newPosition = newHex.Position();
    }
}
