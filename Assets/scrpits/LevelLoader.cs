using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int spawnIndex;

    public Animator transition;
    public float transitionTime = 1f;

    void Start()
    {
        Debug.Log("LevelLoader is active.");
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }
        else
        {
            Debug.LogWarning("Animator transition is not assigned!");
        }

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        LoadNextLevel();
    }
}
