using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform Player;
    public Vector3 offset;
    public float smoothSpeed = 0.125f; // плавность следования
    public float mouseSensitivity = 400f; 
    public float verticalRotationLimit = 80f; 

    private float xRotation = 0f; // наклон по вертикали
    private float yRotation = 0f; // горизонтальный поворот камеры

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // захват курсора
        Cursor.visible = false;

        // Инициализация yRotation по текущему положению игрока
        if (Player != null)
        {
            yRotation = Player.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
        // Изначально наклон камеры по вертикали равен 0
        xRotation = 0f;
    }

    private void LateUpdate()
    {
        // Получение движения мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Обновляем вертикальный наклон камеры (наклон вверх/вниз)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalRotationLimit, verticalRotationLimit);

        // Обновляем горизонтальный поворот камеры
        yRotation += mouseX;

        // Вращение камеры по вертикали (наклон)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Передача горизонтального поворота игроку
        if (Player != null)
        {
            Player.rotation = Quaternion.Euler(0f, yRotation, 0f);
        }

        // Обновляем позицию камеры относительно игрока с учетом смещения
        Vector3 desiredPosition = Player.position + Player.rotation * offset;

        // Плавное перемещение камеры
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}
