using UnityEngine;
using UnityEngine.InputSystem;

public class Soundboard : MonoBehaviour
{
    public AudioClip[] soundEffects; // Assign sounds in the inspector
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Keyboard.current.numpad1Key.wasPressedThisFrame) PlaySound(0);
        if (Keyboard.current.numpad2Key.wasPressedThisFrame) PlaySound(1);
        if (Keyboard.current.numpad3Key.wasPressedThisFrame) PlaySound(2);
        if (Keyboard.current.numpad4Key.wasPressedThisFrame) PlaySound(3);
    }

    void PlaySound(int index)
    {
        if (index < soundEffects.Length)
        {
            audioSource.PlayOneShot(soundEffects[index]);
        }
    }
}
