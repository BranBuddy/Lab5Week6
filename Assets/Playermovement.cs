using UnityEngine;
using UnityEngine.InputSystem;
public class Playermovement : MonoBehaviour
{
    private CharacterController characterController;
    private InputReader input;

    [Tooltip("Speed of player")][SerializeField] internal float speed;

    internal Vector3 currentMovement = Vector3.zero;

    [Header("Player Jump Settings")]
    [SerializeField] private float gravity = -9.81f;
    [Tooltip("How high the player will jump")][SerializeField][Range(1, 10)] private float jumpHeight;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        input = InputReader.Instance;
    }

    // Update is called once per frame
    public void Update()
    {
        Move();
        Jumping();
    }

    private void Move()
    {
        //Assigns the movement input gather to a local vector3
        Vector3 horizontalMovement = new Vector3(input.MoveInput.x, 0, input.MoveInput.y);

        //Whenever player rotation changes, the player's direction changes accordingly
        Vector3 worldDirection = transform.TransformDirection(horizontalMovement);

        //Normalizes worldDirection
        worldDirection.Normalize();

        //Mulitplies the direction moved by player speed
        currentMovement.x = worldDirection.x * speed;
        currentMovement.z = worldDirection.z * speed;

        //Moves the player
        characterController.Move(currentMovement * Time.deltaTime);
    }

    private void Jumping()
    {
        float verticalVel;
        verticalVel = currentMovement.y;


            verticalVel = 0;

            //Checks if the action map action is trigger
            if (input.JumpTrigger)
            {
                //Increases y pos according to the square root of the jump height multiplied by gravity
                currentMovement.y += Mathf.Sqrt(jumpHeight * (-gravity));
                Debug.Log("Hi");
            }
            else
            {
                currentMovement.y += gravity * Time.deltaTime;

            }


    }
        //If the player is not grounded they will continously fall until they are grounded
      

    
}
