using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Win Screen")]
    public GameObject winScreen;

    [Header("Win Goals")]
    public int chefGoal = 20;

    private int enemiesRemaining;
    private int customersServed = 0;
    private bool hasGameEnded = false; // ✅ Prevent multiple wins

    void Start()
    {
        Debug.Log("GameManager is active!");

        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("Enemies at start: " + enemiesRemaining);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void EnemyDefeated()
    {
        if (hasGameEnded) return;

        enemiesRemaining--;
        Debug.Log("Enemy Defeated! Remaining: " + enemiesRemaining);

        if (enemiesRemaining <= 0)
        {
            ShowWinScreen("Dungeon Player");
        }
    }

    public void CustomerServed()
    {
        if (hasGameEnded) return;

        customersServed++;
        Debug.Log("Customer served! Total: " + customersServed);

        if (customersServed >= chefGoal)
        {
            ShowWinScreen("Chef");
        }
    }

    void ShowWinScreen(string winner)
    {
        if (hasGameEnded) return;

        hasGameEnded = true;
        Debug.Log($"{winner} wins! Showing win screen.");

        if (winScreen != null)
        {
            winScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Debug.LogError("WinScreen Panel is NOT assigned in GameManager!");
        }
    }

    public void RestartLevel()
    {
        Debug.Log("Restart Level button clicked!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
