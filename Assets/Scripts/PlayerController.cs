using Mirror;
using Mirror.Examples.AssignAuthority;
using UnityEngine;
using UnityEngine.InputSystem;

// [RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    public float playerMoveSpeed = 5f; // Скорость передвижения
    [SerializeField]
    public float playerRotationSpeed = 10f; // Скорость поворота
    [SerializeField]
    // private CharacterController controller; // Контроллер игрока
    public Rigidbody rb;
    [SerializeField]
    private Vector2 moveInput; // Ввод перемещения
    [SerializeField]
    private PlayerInputActions inputActions; // Инпут система
    [SerializeField]
    public GameObject cameraObject; // Трансформ камеры
    private Transform cameraTransform;
    public Animator animator;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Interact.performed += OnInteract;
    }

    void OnDisable()
    {
        inputActions.Player.Disable();

        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Interact.performed -= OnInteract;
    }

    void Start()
    {

        if (!isLocalPlayer)
        {
            // Локальная камера игрока
            Camera localPlayerCamera = cameraObject.GetComponent<Camera>();
            if (localPlayerCamera != null)
            {
                localPlayerCamera.gameObject.SetActive(false);
                cameraTransform = localPlayerCamera.transform;
            }

            // Локальный звук игрока
            AudioListener localPlayerAudio = cameraObject.GetComponent<AudioListener>();
            if (localPlayerAudio != null)
            {
                localPlayerAudio.enabled = false;
            }

            enabled = false;
            return;
        }

        // Локальный игрок — подключаем камеру
        if (cameraObject != null)
        {
            Camera localPlayerCamera = cameraObject.GetComponent<Camera>();
            if (localPlayerCamera != null)
            {
                localPlayerCamera.gameObject.SetActive(true);
            }

            cameraTransform = cameraObject.transform;
        }
        else
        {
            Debug.LogWarning("Нет cameraObject");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!isLocalPlayer || rb == null) return;

        // Направление движения по камере
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 moveDirection = cameraRight * moveInput.x + cameraForward * moveInput.y;

        // Прямое движение
        rb.linearVelocity = new Vector3(
            moveDirection.x * playerMoveSpeed,
            rb.linearVelocity.y, // сохраняем гравитацию
            moveDirection.z * playerMoveSpeed
        );

        // Поворот к направлению движения
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, playerRotationSpeed * Time.fixedDeltaTime);
        }

        // Анимация
        animator.SetFloat("Speed", moveInput.magnitude);

    }

    private void HandleCameraRotation()
    {
        if (Mouse.current == null) return;

        float mouseX = Mouse.current.delta.ReadValue().x;
        float mouseY = Mouse.current.delta.ReadValue().y;

        // Крутим вокруг игрока
        Vector3 playerCameraOffset = Quaternion.Euler(0, mouseX * 0.2f, 0) * (cameraTransform.position - transform.position);
        cameraTransform.position = transform.position + playerCameraOffset;
        cameraTransform.LookAt(transform.position + Vector3.up * 1f);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Нажата E — взаимодействие!");
    }
}
