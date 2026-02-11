using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PowerupInteractable : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{ 
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Find the player controller and activate the powerup 
        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.StartCoroutine(player.ActivatePowerUp());
        } 
        // Destroy the object after grabbing 
        Destroy(gameObject); } 
}