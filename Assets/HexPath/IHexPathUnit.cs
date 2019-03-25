using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QPath
{
    public interface IHexPathUnit
    {
        float CostToEnterHex(IHexPathTile sourceTile, IHexPathTile destinationTile);
    }
}

