using UnityEngine;
using UnityEngine.EventSystems;

public class MobileTouchpad : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public PlayerInputHandler inputHandler;
    public float sensitivityMultiplier = 0.5f; 

    public void OnPointerDown(PointerEventData eventData)
    {
        // Deteksi awal sentuhan
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (inputHandler != null)
        {
            // FIX: Langsung tembak nilainya ke PlayerInputHandler saat jari bergerak
            inputHandler.RotationInput = eventData.delta * sensitivityMultiplier;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (inputHandler != null)
        {
            inputHandler.RotationInput = Vector2.zero;
        }
    }

    // FIX: Menggunakan LateUpdate (berjalan di akhir frame) agar script kamera player 
    // sempat membaca nilai rotasinya terlebih dahulu sebelum di-reset ke nol
    void LateUpdate()
    {
        if (inputHandler != null)
        {
            inputHandler.RotationInput = Vector2.zero;
        }
    }
}