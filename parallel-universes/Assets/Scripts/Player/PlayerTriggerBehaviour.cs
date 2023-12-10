using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTriggerBehaviour : MonoBehaviour
{
    public UnityEvent<PlayerTriggerEventArgs> onPlayerTriggerEnter;
    public UnityEvent<PlayerTriggerEventArgs> onPlayerTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.TryGetComponent(out PlayerController player))
        {
            onPlayerTriggerEnter?.Invoke(new PlayerTriggerEventArgs(player, this));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerController player))
        {
            onPlayerTriggerExit?.Invoke(new PlayerTriggerEventArgs(player, this));
        }
    }
}

public struct PlayerTriggerEventArgs
{
    public PlayerController player;
    public PlayerTriggerBehaviour trigger;

    public PlayerTriggerEventArgs(PlayerController player, PlayerTriggerBehaviour trigger)
    {
        this.player = player;
        this.trigger = trigger;
    }
}