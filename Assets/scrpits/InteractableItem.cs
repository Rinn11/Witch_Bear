using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    private Renderer itemRenderer;
    private Color originalColor;

    public enum ItemEffectType { SpeedBoost, Slowdown, HighJump, OpenDoor }
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
        }

        Destroy(gameObject); // Remove item after use
    }

    private void OpenNearbyDoor()
    {
        Debug.Log("Door opened!");
        door.SetActive(false);
    }

}
