using UnityEngine;

public struct TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public TransformData(Transform t)
    {
        position = t.position;
        rotation = t.rotation;
        scale = t.localScale;
    }
}
