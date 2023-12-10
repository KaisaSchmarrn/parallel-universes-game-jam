using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "New Universe Type", menuName = "Universe Type")]
public class UniverseType_SO : ScriptableObject
{
    [SerializeField] private Material universeMaterial;
    [SerializeField] private Mesh runeMesh;
    [SerializeField] private VolumeProfile postProcessingProfile;

    public Material UniverseMaterial => universeMaterial;
    public Mesh RuneMesh => runeMesh;
    public VolumeProfile PostProcessingProfile => postProcessingProfile;
}
