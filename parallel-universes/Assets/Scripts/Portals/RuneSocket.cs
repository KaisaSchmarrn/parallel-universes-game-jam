using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneSocket : MonoBehaviour
{
    [SerializeField] private UniverseType_SO universeType;
    [SerializeField] private MeshRenderer runeRenderer;

    public bool IsCollected { get; private set; }

    private void Awake()
    {
        runeRenderer.material = universeType.UniverseMaterial;

        if(runeRenderer.transform.TryGetComponent(out MeshFilter filter))
        {
            filter.mesh = universeType.RuneMesh;
        }
    }

    private void HandleRuneCollected(bool collected)
    {
        if (IsCollected == collected) return;

        IsCollected = collected;

        runeRenderer.gameObject.SetActive(!IsCollected);

        if (!IsCollected) return;

        GameManager.Instance.HandleRuneCollected(universeType);
    }

    public void OnPlayerTriggerEnter(PlayerTriggerEventArgs args)
    {
        HandleRuneCollected(true);
    }

    private void OnGameReset(GameResetEventArgs args)
    {
        HandleRuneCollected(false);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameReset += OnGameReset;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameReset -= OnGameReset;
    }
}
