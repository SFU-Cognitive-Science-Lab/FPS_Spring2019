using UnityEngine;

class DFPositionRotation : DataFarmerObject
{
    Vector3 position;
    Quaternion rotation;
    float distance;

    public DFPositionRotation(Vector3 position, Quaternion rotation, float distanceTravelled) :
        base("posrot")
    {
        this.position = position;
        this.rotation = rotation;
        distance = distanceTravelled;
    }

    public override string Serialize()
    {
        return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\n",
            base.Serialize(), position.x, position.y, position.z,
            rotation.x, rotation.y, rotation.z, distance);
    }
}
