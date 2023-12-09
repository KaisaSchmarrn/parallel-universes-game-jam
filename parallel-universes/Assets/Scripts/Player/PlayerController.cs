using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public void Teleport(Vector3 newPosition, Quaternion newRotation)
    {
        transform.position = newPosition;
        transform.rotation = newRotation;

        Debug.Log("teleported to " + newPosition);
    }
}
