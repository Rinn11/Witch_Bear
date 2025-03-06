using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    private Renderer itemRenderer;
    private Color originalColor;

    public Sprite effectImg;

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
                player.ApplySpeedMultiplier(effectValue, effectImg);
                break;
            case ItemEffectType.Slowdown:
                player.ApplySpeedMultiplier(effectValue, effectImg);
                break;
            case ItemEffectType.HighJump:
                player.ApplyJumpBoost(effectValue, effectImg);
                break;
            case ItemEffectType.OpenDoor:
                OpenNearbyDoor();
                break;
            case ItemEffectType.StrengthBuff:
                player.EnableStrengthBuff(effectImg);
                break;
            case ItemEffectType.Shrink:
                player.ShrinkPlayer(effectImg);
                break;
            case ItemEffectType.ReverseControls:
                player.ReverseControls(effectImg);
                break;
            case ItemEffectType.FlipScreen:
                player.FlipScreen(effectImg);
                break;
            case ItemEffectType.Teleport:
                player.TeleportPlayer(effectImg);
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
