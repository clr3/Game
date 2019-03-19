using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QPath
{
    /// <summary>
    /// 
    ///     Tile[] ourPath = QPath.FindPath( ourWorld, theUnit, startTile, endTile );
    ///     
    ///     theUnit is the object that is trying to path between tiles. maybe special logic based on its movement type and type of tiles moved through
    /// 
    ///     our tiles need to be able to return:
    ///         1) List of Neighbours
    ///         2 The aggregate cost to enter this tile from another tile
    /// </summary>

    public static class QPath
    {
        public static T[] FindPath<T>(
            IQPathWorld world,
            IQPathUnit unit,
            T startTile,
            T endTile,
            CostEstimateDelegate costEstimateFunc
            ) where T : IQPathTile
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
    public delegate float CostEstimateDelegate(IQPathTile a, IQPathTile b);
}

