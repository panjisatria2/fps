using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerInputHandler inputHandler;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (inputHandler != null) inputHandler.JumpTriggered = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (inputHandler != null) inputHandler.JumpTriggered = false;
    }
}