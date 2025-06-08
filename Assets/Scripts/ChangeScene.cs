using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public int difficulty = 0;

    public void SetDifficulty(int difficulty)
    {
        this.difficulty = difficulty;
    }
    
    public void MoveToScene(int sceneID)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneID);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameManager gm = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        if (gm != null)
            gm.SetDifficulty(difficulty);
    }
}
