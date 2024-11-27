using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterMovement;

public class CustomCharacterMovement : CharacterMovement3D
{
    // teleport to given location
    public void Teleport(Vector3 location)
    {
        // when using physics (Rigidbody) setting the transform location can be unreliable
        // transform.position = location;

        // we want to set the position of the Rigidbody instead
        Rigidbody.position = location;
        NavMeshAgent.Warp(location);
    }
}