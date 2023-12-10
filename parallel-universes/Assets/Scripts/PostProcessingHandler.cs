using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Volume))]
public class PostProcessingHandler : MonoBehaviour
{
    private Volume volume;

    private void Awake()
    {
        volume = GetComponent<Volume>();
    }
    private void OnUniverseSwitched(UniverseSwitchedEventArgs args)
    {
        if (args.nextUniverseType == null) return;

        volume.profile = args.nextUniverseType.PostProcessingProfile;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnUniverseSwitched += OnUniverseSwitched;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnUniverseSwitched -= OnUniverseSwitched;
    }
}
