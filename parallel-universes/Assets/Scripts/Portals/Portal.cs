using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private UniverseType_SO universeType;
    [SerializeField] private Portal nextPortal;
    [SerializeField] private MeshRenderer lightRenderer;
    [SerializeField] private MeshRenderer eventHorizonRenderer;


    private PlayerController player;
    private float dotProductLastFrame = 0f;

    public UniverseType_SO UniverseType => universeType;

    private void Awake()
    {
        if (universeType != null) lightRenderer.material = universeType.UniverseMaterial;

        if (nextPortal == null) return;
        if (nextPortal.UniverseType != null) eventHorizonRenderer.material = nextPortal.UniverseType.UniverseMaterial;
    }

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
            
            player.Teleport(newPosition, newRotation);
            player = null;

            GameManager.Instance.HandleUniverseSwitched(universeType, nextPortal.UniverseType);
        }

        dotProductLastFrame = dotProduct;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerController player))
        {
            this.player = player;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerController player))
        {
            this.player = null;
        }
    }
}
