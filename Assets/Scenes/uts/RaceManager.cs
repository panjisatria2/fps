using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    [Header("UI Settings")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI countdownText;

    [Header("Finish UI Settings")]
    public GameObject finishPanel; 
    public TextMeshProUGUI finalTimeText; 

    [Header("Car Settings")]
    public Rigidbody carRigidbody;
    public mobilscript scriptMobil; 

    private float elapsedTime = 0f;
    private bool isRacing = false;

    void Start() 
    { 
        if(finishPanel != null) finishPanel.SetActive(false);
        StartCoroutine(StartCountdown()); 
    }

    void Update()
    {
        if (isRacing)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
        UpdateSpeedDisplay();
    }

    // --- DETEKSI SENSOR FINISH ---
    private void OnTriggerEnter(Collider other) 
    { 
        // Biar kita tau di Console objek apa yang masuk
        Debug.Log("Kena objek: " + other.gameObject.name);

        // Langsung cek: kalau yang nabrak namanya mengandung "Car3", langsung FINISH!
        if (isRacing && other.gameObject.name.Contains("Car3"))
        {
            Debug.Log("FINISH TERDETEKSI!");
            EndRace();
        }
    }

    void EndRace()
    {
        isRacing = false;
        Debug.Log("BALAPAN SELESAI!");
        
        // Matikan kontrol gas mobil
        if(scriptMobil != null) scriptMobil.bisaJalan = false;

        // Paksa fisik mobil berhenti total
        if (carRigidbody != null) {
            carRigidbody.linearVelocity = Vector3.zero;
            carRigidbody.angularVelocity = Vector3.zero;
            carRigidbody.isKinematic = true; 
        }

        // Munculkan Panel Rincian Waktu
        if (finishPanel != null) 
        {
            finishPanel.SetActive(true);
            Debug.Log("Finish Panel Diaktifkan.");
        }

        if (finalTimeText != null) {
            finalTimeText.text = "Waktu Akhir: " + timerText.text;
        }
    }

    // Fungsi untuk tombol Ulangi & Next
    public void RestartLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    public void LoadNextLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }

    IEnumerator StartCountdown()
    {
        if(scriptMobil != null) scriptMobil.bisaJalan = false;
        if (carRigidbody != null) carRigidbody.isKinematic = true;

        float countdownTime = 3f;
        while (countdownTime > 0)
        {
            if (countdownText != null) countdownText.text = countdownTime.ToString("0");
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        if (countdownText != null) countdownText.text = "GO!";
        
        isRacing = true;
        if(scriptMobil != null) scriptMobil.bisaJalan = true;
        if (carRigidbody != null) carRigidbody.isKinematic = false; 
        
        elapsedTime = 0f;
        yield return new WaitForSeconds(1f);
        if (countdownText != null) countdownText.text = ""; 
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 100F) % 100F);
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    void UpdateSpeedDisplay()
    {
        if (carRigidbody != null && speedText != null)
        {
            float speed = carRigidbody.linearVelocity.magnitude * 3.6f;
            speedText.text = speed.ToString("F0") + " KM/H";
        }
    }
}