using UnityEngine;
using UnityEngine.InputSystem;
public class InputReader : MonoBehaviour
{
    [SerializeField] private InputActionAsset playerControls;

    //Gathers action map name and assigns it to a variable
    [SerializeField] private string actionMapName = "Player";

    //Allows editor to change name of var if the name in the acton map changes
    [Header("Input Names")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string look = "Look";

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;

    // Gets the input and sets the variable
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpTrigger { get; private set; }

    public static InputReader Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Assigns the input action variables to the action in the action map
        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        jumpAction = playerControls.FindActionMap(actionMapName).FindAction(jump);
        lookAction = playerControls.FindActionMap(actionMapName).FindAction(look);

        RegisterInputAction();
    }

    //Passes the input action to get/set variable when the action is performed or canceled
    void RegisterInputAction()
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = context.ReadValue<Vector2>();

        lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => LookInput = context.ReadValue<Vector2>();

        jumpAction.performed += context => JumpTrigger = true;
        jumpAction.canceled += context => JumpTrigger = false;


    }
    //Turns the actions on
    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        lookAction.Enable();

    }
}
