using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal nextPortal;

    private PlayerController player;
    private float dotProductLastFrame = 0f;

    private void Update()
    {
        if (player == null || nextPortal == null) return;

        Vector3 dirPlayerToPortal = transform.position - player.transform.position;

        float dotProduct = Vector3.Dot(dirPlayerToPortal, transform.forward);

        if (dotProduct <= 0 && dotProductLastFrame > 0)
        {
            Vector3 relativePosition = transform.InverseTransformPoint(player.transform.position);
            Vector3 newPosition = nextPortal.transform.TransformPoint(relativePosition);

            Quaternion relativeRotation = Quaternion.FromToRotation(transform.forward, player.transform.forward);
            Quaternion newRotation = nextPortal.transform.rotation * relativeRotation;
            /*
            Vector3 relativeRotation = transform.InverseTransformDirection(player.transform.forward);
            Vector3 newForward = transform.TransformDirection(relativeRotation);
            */

            player.Teleport(newPosition, newRotation);
            player = null;
        }

        dotProductLastFrame = dotProduct;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerController player))
        {
            this.player = player;
            Debug.Log("playerEnter");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerController player))
        {
            this.player = null;
            Debug.Log("playerExit");
        }
    }
}
