using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QPath
{


    public static class QPath
    {
        public static T[] FindPath<T>(
            IHexPathWorld world,
            IHexPathUnit unit,
            T startTile,
            T endTile,
            CostEstimateDelegate costEstimateFunc
            ) where T : IHexPathTile
        {
            Debug.Log("QPath::FindPath");
            if (world == null || unit == null || startTile == null || endTile == null)
            {
                Debug.LogError("null values passed to QPath::FindPath");
                return null;
            }

            // Call on path solver
            QPath_AStar<T> resolver = new QPath_AStar<T>(world, unit, startTile, endTile, costEstimateFunc);

            resolver.DoWork();

            return resolver.GetList();
        }
    }
    public delegate float CostEstimateDelegate(IHexPathTile a, IHexPathTile b);
}

