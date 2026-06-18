using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;
    public PlayerInputHandler inputHandler;
    
    private float joystickRadius;

    void Start()
    {
        // FIX: Menggunakan rect.width agar ukuran asli pikselnya tetap terbaca akurat walaupun UI di-scale/stretch
        joystickRadius = joystickBackground.rect.width / 2f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / joystickRadius);
            pos.y = (pos.y / joystickRadius);

            Vector2 inputVector = (pos.magnitude > 1.0f) ? pos.normalized : pos;

            // Menggerakkan bulatan tengah analog
            joystickHandle.anchoredPosition = new Vector2(inputVector.x * joystickRadius, inputVector.y * joystickRadius);
            
            // Mengirimkan nilai ke pergerakan Player
            if (inputHandler != null)
            {
                inputHandler.MovementInput = inputVector;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset posisi analog ke tengah saat dilepas
        joystickHandle.anchoredPosition = Vector3.zero;
        if (inputHandler != null)
        {
            inputHandler.MovementInput = Vector2.zero;
        }
    }
}