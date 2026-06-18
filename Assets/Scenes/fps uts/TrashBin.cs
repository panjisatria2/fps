using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement;

public class TrashBin : MonoBehaviour
{
    [Header("Score Settings")]
    public int currentScore = 0;
    public int targetScore = 5;

    [Header("Referensi UI Utama")]
    public TextMeshProUGUI scoreText;
    public GameObject levelCompletePanel; 

    [Header("Referensi Teks DI DALAM Panel Selesai")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalTimeText;

    [Header("Referensi Script Lain")]
    public GameTimer gameTimer;

    [Header("Pengaturan Level")]
    public string nextSceneName; 
    public bool isLastLevel = false; 

    private void Start()
    {
        Time.timeScale = 1f; 

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }

        UpdateScoreUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("pickup"))
        {
            currentScore++;
            UpdateScoreUI();
            Destroy(other.gameObject);

            if (currentScore >= targetScore)
            {
                LevelComplete();
            }
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Sampah: " + currentScore + " / " + targetScore;
        }
    }

    private void LevelComplete()
    {
        Debug.Log("🎉 SELAMAT! Area sudah bersih!");
        
        if (gameTimer != null)
        {
            gameTimer.timerIsRunning = false;

            float timeTaken = gameTimer.initialTime - gameTimer.timeRemaining;
            float minutes = Mathf.FloorToInt(timeTaken / 60);
            float seconds = Mathf.FloorToInt(timeTaken % 60);

            if (finalTimeText != null)
            {
                finalTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = currentScore + " / " + targetScore;
        }

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
        }

        Time.timeScale = 0f; 

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartLevel()
    {
        string sceneSekarang = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneSekarang);
    }

    public void NextOrMainMenu()
    {
        if (isLastLevel)
        {
            SceneManager.GetActiveScene();
            SceneManager.LoadScene("MainMenu"); 
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}