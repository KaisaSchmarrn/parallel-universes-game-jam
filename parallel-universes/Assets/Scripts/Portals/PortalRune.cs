using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalRune : MonoBehaviour
{
    [SerializeField] private UniverseType_SO universeType;

    [SerializeField] private MeshFilter activatedRune;
    [SerializeField] private MeshFilter deactivatedRune;

    public bool IsActivated { get; private set; }

    private void Awake()
    {
        if (universeType == null) return;

        activatedRune.mesh = universeType.RuneMesh;
        deactivatedRune.mesh = universeType.RuneMesh;

        if(activatedRune.gameObject.TryGetComponent(out MeshRenderer renderer))
        {
            renderer.material = universeType.UniverseMaterial;
        }
    }

    public void SetActive(bool active)
    {
        activatedRune.gameObject.SetActive(active);
        deactivatedRune.gameObject.SetActive(!active);

        IsActivated = active;
    }

    private void OnGameReset(GameResetEventArgs args)
    {
        SetActive(false);
    }

    private void OnRuneCollected(RuneCollectedEventArgs args)
    {
        if(universeType == args.universeType) SetActive(true);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameReset += OnGameReset;
        GameManager.Instance.OnRuneCollected += OnRuneCollected;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameReset -= OnGameReset;
        GameManager.Instance.OnRuneCollected -= OnRuneCollected;
    }
}
