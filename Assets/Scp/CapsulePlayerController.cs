using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput), typeof(Animator))]
public class CapsulePlayerController : MonoBehaviour
{
    private const string MovementStateName = "Movement";

    [SerializeField,Tooltip("How fast the player moves when walking.")] 
    private float playerSpeed = 2f;
    [SerializeField,Tooltip("How high the player jumps.")] 
    private float jumpHeight = 1f;
    [SerializeField,Tooltip("Speed of gravity.")] 
    private float gravityValue = -9.81f;
    [SerializeField,Tooltip("How fast the player rotates.")] 
    private float rotationSpeed = 5f;
    [SerializeField,Tooltip("The acceleration for the player's animations.")] 
    private float animationAcceleration = 5f;
    [SerializeField,Tooltip("Prefab of the bullet to spawn when shooting.")] 
    private GameObject bulletPrefab;
    [SerializeField,Tooltip("The transform of the gun's barrel, used for bullet spawning.")] 
    private Transform barrelTransform;
    [SerializeField,Tooltip("The parent transform for spawned bullets.")] 
    private Transform bulletParent;
    [SerializeField,Tooltip("The maximum distance at which a bullet can hit or miss a target.")] 
    private float bulletHitMissDistance = 25f;
    [SerializeField] private float animationSmoothTime = 0.1f;
    [SerializeField] private float animationPlayTranstion = 0.15f;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Vector3 cameraTargetLocalPosition = new Vector3(0f, 1.5f, 0f);

    private CharacterController controller;
    private PlayerInput playerInput;
    private bool groundedPlayer;
    
    private Vector3 playerVelocity;
    private float currentSpeedFactor;
    private float animatorHorizontal;
    private float animatorVelocity;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;

    private Animator animator;
    int jumpAmimation;
    int moveXAnimatorParameterId;
    int moveZAnimatorParameterId;
    Vector2 currentAnimationBlendVector;
    Vector2 animationVelocity;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTarget = Camera.main.transform;
        //Cache a reference to all of the input actions to avoid thenm with string constantly
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];

        Cursor.lockState = CursorLockMode.Locked;
        // Animations
        animator = GetComponent<Animator>();
        jumpAmimation = Animator.StringToHash("Pistol Jump");
        moveXAnimatorParameterId = Animator.StringToHash("Horizontal");
        moveZAnimatorParameterId = Animator.StringToHash("Vertical");
        //EnsureCameraTarget();
    }
    private void OnEnable()
    {
        shootAction.performed += _ => ShootGun();
    }
    private void OnDisable()
    {
        shootAction.performed -= _ => ShootGun();
    }
    private void ShootGun()
    {
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if(Physics.Raycast(cameraTarget.position, cameraTarget.forward, out hit, Mathf.Infinity))
        {
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = cameraTarget.position + cameraTarget.forward * bulletHitMissDistance; 
            bulletController.hit = false;
        }
    }

    private void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        
        Vector2 input = moveAction.ReadValue<Vector2>();
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity, animationSmoothTime);
        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);
        move = move.x * CameraTarget.right.normalized + move.z * CameraTarget.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime);
        animator.SetFloat(moveXAnimatorParameterId, currentAnimationBlendVector.x );
        animator.SetFloat(moveZAnimatorParameterId, currentAnimationBlendVector.y );

        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotate towards camera direction when moving
        Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
    public Transform CameraTarget
    {
    get
    {
        //EnsureCameraTarget();
        return cameraTarget;
    }
    }
}
