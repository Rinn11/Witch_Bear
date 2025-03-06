using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    private Renderer itemRenderer;
    private Color originalColor;

    public Color highColor;
    public Image mistImage;

    public enum ItemEffectType { SpeedBoost, Slowdown, HighJump, OpenDoor, StrengthBuff, Shrink, ReverseControls, FlipScreen, Teleport }
    public ItemEffectType effectType;
    public float effectValue = 1.5f; // Default multiplier for speed/jump

    public GameObject door;

    void Start()
    {
        itemRenderer = GetComponent<Renderer>();
        originalColor = itemRenderer.material.color;
    }

    public void Highlight(bool highlight)
    {
        itemRenderer.material.color = highlight ? Color.yellow : originalColor;
    }

    public void ApplyEffect(PlayerController player)
    {
        //mistImage.color = highColor;

        switch (effectType)
        {
            case ItemEffectType.SpeedBoost:
                player.ApplySpeedMultiplier(effectValue);
                break;
            case ItemEffectType.Slowdown:
                player.ApplySpeedMultiplier(1 / effectValue);
                break;
            case ItemEffectType.HighJump:
                player.ApplyJumpBoost(effectValue);
                break;
            case ItemEffectType.OpenDoor:
                OpenNearbyDoor();
                break;
            case ItemEffectType.StrengthBuff:
                player.EnableStrengthBuff();
                break;
            case ItemEffectType.Shrink:
                player.ShrinkPlayer();
                break;
            case ItemEffectType.ReverseControls:
                player.ReverseControls();
                break;
            case ItemEffectType.FlipScreen:
                player.FlipScreen();
                break;
            case ItemEffectType.Teleport:
                player.TeleportPlayer();
                break;
        }

        Destroy(gameObject); // Remove item after use
    }

    private void OpenNearbyDoor()
    {
        Debug.Log("Door opened!");
        door.GetComponent<InteractableItem>().enabled = true; 
    }
}
