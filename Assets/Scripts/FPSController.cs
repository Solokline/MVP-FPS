using UnityEngine;
/// <summary>
/// Базовое управление FPS-персонажем: перемещение и вращение камеры от мыши.
/// </summary>
public class FPSController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 80f; // Ограничение взгляда вверх/вниз

    [Header("References")]
    [SerializeField] private Transform playerCamera; // Камера (дочерний объект)
    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f; // Текущий поворот по вертикали
    private bool isGrounded;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        // Блокируем курсор в центре экрана
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    /// <summary>
    /// Поворот камеры от мыши (горизонталь — поворот игрока, вертикаль — поворот камеры).
    /// </summary>
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        // Поворот игрока по горизонтали (вокруг оси Y)
        transform.Rotate(Vector3.up * mouseX);
        // Поворот камеры по вертикали (вокруг оси X) с ограничением
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    /// <summary>
    /// Перемещение персонажа (WASD + прыжок + гравитация).
    /// </summary>
    private void HandleMovement()
    {
        // Проверка земли: CharacterController имеет свойство isGrounded
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Небольшой прижим к земле
        }

        // Ввод движения
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Бег с Shift
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Гравитация
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}