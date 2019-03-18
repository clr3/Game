using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit{

    public string Name = "Dwarf";
    public int HitPoints = 100;
    public int Strength = 8;
    public int Speed = 3;
    public int SpeedRemaining = 3;

    public Hex Hex { get; protected set; }

    public void setHex(Hex hex)
    {
        if(hex != null)
        {
            Hex.RemoveUnit(this);
        }

        Hex = hex;

        Hex.AddUnit(this);

    }

    public void DoTurn()
    {
        // do queued move?
    }
}
