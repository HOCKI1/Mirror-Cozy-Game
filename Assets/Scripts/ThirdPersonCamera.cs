using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform pivot; // CameraPivot
    public Transform target; // Игрок
    public Vector2 sensitivity = new Vector2(3f, 1.5f); // Чувствительность мыши
    public Vector2 pitchClamp = new Vector2(-30, 60); // Ограничение угла вращения по вертикали
    public float distance = 5f; // Расстояние от камеры до игрока
    public float followSpeed = 5f; // Скорость сглаживания перемещения камеры

    private float yaw;
    private float pitch;

    void LateUpdate()
    {
        if (target == null || pivot == null) return;

        // Плавное движение pivot по X и Z
        Vector3 targetPos = new Vector3(target.position.x, pivot.position.y, target.position.z);
        pivot.position = Vector3.Lerp(pivot.position, targetPos, followSpeed * Time.deltaTime);

        // Движение мыши
        Vector2 look = Mouse.current.delta.ReadValue() * sensitivity;
        yaw += look.x;
        pitch -= look.y;
        pitch = Mathf.Clamp(pitch, pitchClamp.x, pitchClamp.y);

        // Вращение pivot
        pivot.rotation = Quaternion.Euler(pitch, yaw, 0);

        // Камера позади pivot
        transform.localPosition = new Vector3(0, 0, -distance);

        // Камера смотрит на pivot
        transform.LookAt(pivot.position);
    }
}
