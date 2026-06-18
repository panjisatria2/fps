using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("Pengaturan Waktu")]
    public float timeRemaining = 60f;
    public bool timerIsRunning = false;
    [HideInInspector] public float initialTime;

    [Header("Referensi UI Utama")]
    public TextMeshProUGUI timeText; // Teks timer di pojok kanan atas
    
    [Header("Referensi UI Panel Gagal")]
    public GameObject gameOverPanel; // Tempat menaruh objek panel 'Levelgagal'
    public TextMeshProUGUI failureScoreText; // Slot baru: Teks skor di dalam panel gagal
    public TextMeshProUGUI failureTimeText;  // Slot baru: Teks waktu di dalam panel gagal

    [Header("Referensi Script Lain")]
    public TrashBin trashBin; // Slot baru: Tempat menaruh objek 'TrashBin'

    void Start()
    {
        initialTime = timeRemaining;
        timerIsRunning = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerDisplay(0f); // Paksa UI menjadi 00:00 tepat saat menyentuh angka 0
                GameOver();
            }
        }
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        // Pengaman ekstra: Jika nilai di bawah 0, langsung kunci di angka 0 agar tidak memunculkan -01
        if (timeToDisplay < 0) 
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void GameOver()
    {
        Debug.Log("WAKTU HABIS! GAME OVER!");

        // 1. Hitung durasi pengerjaan (Karena kalah waktu habis, berarti menghabiskan seluruh waktu awal)
        float minutes = Mathf.FloorToInt(initialTime / 60);
        float seconds = Mathf.FloorToInt(initialTime % 60);

        if (failureTimeText != null)
        {
            failureTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        // 2. Ambil nilai skor sampah terakhir dari script TrashBin sebelum game di-freeze
        if (trashBin != null && failureScoreText != null)
        {
            failureScoreText.text = trashBin.currentScore + " / " + trashBin.targetScore;
        }

        // 3. Matikan pergerakan/jalannya waktu di dalam game
        Time.timeScale = 0f;

        // 4. Aktifkan Panel Gagal
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // 5. Bebaskan pergerakan kursor mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}