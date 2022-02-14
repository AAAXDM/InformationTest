using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo 
{
    #region Fields
    List<TransformInfo> positions;
    int samePositionsCount;
    #endregion

    #region Properties
    public IReadOnlyList<TransformInfo> Positions => positions;
    public int SamePositionsCount => samePositionsCount;
    #endregion

    public ObjectInfo()
    {
        positions = new List<TransformInfo>();
    }

    #region Public Methods
    public void AddTransform(Vector3 position, Quaternion rotation)
    {
        TransformInfo transformInfo = new TransformInfo(position, rotation);
        positions.Add(transformInfo);
    }

    public void PlusSamePosition()
    {
        samePositionsCount++;
    }
    #endregion
}
