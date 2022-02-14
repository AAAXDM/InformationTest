using UnityEngine;

public struct TransformInfo
{
    public readonly Vector3 position;
    public readonly  Quaternion rotation;

    public TransformInfo(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }

    public bool Comparsion(TransformInfo transformInfo)
    {
        if (transformInfo.position == position && transformInfo.rotation == rotation) return true;
        return false;
    }
}
