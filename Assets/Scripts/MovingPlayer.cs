using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingPlayer : MonoBehaviour
{
    public Transform cameraTransform; // ссылка на камеру
    public float moveSpeed = 5f; // скорость движения
    public float smoothTime = 0.1f; // время сглаживания
    private Rigidbody rb;
    private Vector3 currentVelocity = Vector3.zero; // для SmoothDamp

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
            if (cameraTransform == null)
            {
                Debug.LogError("Камера не назначена и не найдена. Назначьте камеру вручную или добавьте тег MainCamera.");
            }
        }
    }

    private void FixedUpdate()
    {
        if (cameraTransform == null)
        {
            // Если камера так и не назначена, выходим из метода
            return;
        }

        // Получение ввода
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Получение направления камеры
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        // Создаем вектор движения
        Vector3 moveDirection = (cameraForward * verticalInput) + (cameraRight * horizontalInput);

        // Нормализация для равномерного движения
        if (moveDirection.magnitude > 0.1f)
        {
            moveDirection.Normalize();
            Vector3 targetVelocity = moveDirection * moveSpeed;

            // Плавное изменение скорости
            rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref currentVelocity, smoothTime);
        }
        else
        {
            // Плавное торможение до нуля
            rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, Vector3.zero, ref currentVelocity, smoothTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.CompareTag("Player") && other.CompareTag("Finish"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

