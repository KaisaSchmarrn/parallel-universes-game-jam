using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGDev.BasicExtensions;

public class BounceAndRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private MinMax yOffset;

    private float bounceTimer;

    private void Update()
    {
        transform.Rotate(transform.up, rotationSpeed * Time.deltaTime);

        bounceTimer += bounceSpeed * Time.deltaTime;

        float y = Mathf.Sin(bounceTimer).Remap(-1f, 1f, yOffset.min, yOffset.max);

        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }
}
