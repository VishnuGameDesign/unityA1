using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Vector3 Position => transform.position;
    public Vector3 Direction => transform.forward;
    public Quaternion Rotation => transform.rotation;
}