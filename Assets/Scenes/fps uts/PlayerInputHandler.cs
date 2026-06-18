using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string rotateObject = "RotateObject";


    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction rotateObjectAction;


    // --- KANTONG KHUSUS INPUT MOBILE ---
    private Vector2 mobileMovement;
    private Vector2 mobileRotation;
    private bool mobileJump;

    // --- KANTONG KHUSUS INPUT PC / KEYBOARD ---
    private Vector2 pcMovement;
    private Vector2 pcRotation;
    private bool pcJump;


    // --- PROPERTI GABUNGAN (SMART INPUT) ---
    // Sistem akan mengecek: Kalau analog mobile digerakkan, pakai nilai mobile. Kalau tidak, pakai nilai PC.
    public Vector2 MovementInput 
    { 
        get { return mobileMovement != Vector2.zero ? mobileMovement : pcMovement; }
        set { mobileMovement = value; }
    }
    
    public Vector2 RotationInput 
    { 
        get { return mobileRotation != Vector2.zero ? mobileRotation : pcRotation; }
        set { mobileRotation = value; }
    }
    
    public bool JumpTriggered 
    { 
        get { return mobileJump || pcJump; }
        set { mobileJump = value; }
    }   

    // Sprint dan RotateObject dibiarkan seperti semula
    public bool SprintTriggered { get; private set; }
    public bool RotateObjectTriggered { get; private set; }
    

    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        jumpAction = mapReference.FindAction(jump);
        sprintAction = mapReference.FindAction(sprint);
        rotateObjectAction = mapReference.FindAction(rotateObject);

        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents()
    {
        // Alihkan semua pembacaan input sistem bawaan Unity ke kantong "pc"
        movementAction.performed += inputInfo => pcMovement = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => pcMovement = Vector2.zero;

        rotationAction.performed += inputInfo => pcRotation = inputInfo.ReadValue<Vector2>();
        rotationAction.canceled += inputInfo => pcRotation = Vector2.zero;

        jumpAction.performed += inputInfo => pcJump = true;
        jumpAction.canceled += inputInfo => pcJump = false;

        sprintAction.performed += inputInfo => SprintTriggered = true;
        sprintAction.canceled += inputInfo => SprintTriggered = false;

        rotateObjectAction.performed += inputInfo => RotateObjectTriggered = true;
        rotateObjectAction.canceled += inputInfo => RotateObjectTriggered = false;
    }

    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }

    private void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
    }
}