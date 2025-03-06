using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    PlayerController controller;

    public Camera playerCamera;
    public float interactionDistance = 3f;
    public Image eatingProgressBar;
    public float eatingTime = 2f;

    private InteractableItem currentItem;
    private float eatProgress = 0f;

    public GameObject HungerBar;
    public TextMeshProUGUI munchTime;

    private bool isEating = false;

    void Update()
    {
        DetectItem();

        if (currentItem != null && Mouse.current.leftButton.isPressed)
        {
            StartEating();
        }
        else if (isEating && Mouse.current.leftButton.isPressed == false)
        {
            CancelEating();
        }
    }

    void DetectItem()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, interactionDistance))
        {
            InteractableItem item = hit.collider.GetComponent<InteractableItem>();
            
            if (item != null)
            {
                if (currentItem != item)
                {
                    if (currentItem != null) currentItem.Highlight(false);
                    currentItem = item;
                    currentItem.Highlight(true);
                    munchTime.text = "Munch";
                }
                return;
            }
        }

        if (currentItem != null)
        {
            munchTime.text = "";
            currentItem.Highlight(false);
            currentItem = null;
        }
    }

    void StartEating()
    {
        HungerBar.SetActive(true);

        isEating = true;
        eatProgress += Time.deltaTime / eatingTime;
        eatingProgressBar.fillAmount = eatProgress;

        if (eatProgress >= 1f)
        {
            ConsumeItem();
        }
    }

    void CancelEating()
    {
        isEating = false;
        eatProgress = 0f;
        eatingProgressBar.fillAmount = 0f;
        HungerBar.SetActive(false);
    }

    void ConsumeItem()
    {
        if (currentItem != null)
        {

            this.GetComponent<PlayerController>().StartEating();
            currentItem.ApplyEffect(GetComponent<PlayerController>());
            Destroy(currentItem.gameObject);
            CancelEating();
        }
    }
}
