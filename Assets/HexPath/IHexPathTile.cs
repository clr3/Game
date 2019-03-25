using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QPath
{
    public interface IHexPathTile
    {

        IHexPathTile[] GetNeighbours();

        float AggregateCostToEnter(float costSoFar, IHexPathTile sourceTile, IHexPathUnit theUnit);

    }
}

