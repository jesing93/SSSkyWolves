using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Camera Data", menuName = "Player Camera Data")]
public class PlayerCameraData : ScriptableObject
{
    public Vector3 offset;
    public Vector3 localOffset;
    public float tensor;
}
