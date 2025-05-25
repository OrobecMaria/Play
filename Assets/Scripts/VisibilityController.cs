using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    public Camera mainCamera;          // Камера, от которой идет луч
    public GameObject textObject;      // Объект с TextMesh
    public float maxDistance = 100f;  // Максимальная длина луча

    private Renderer textRenderer;

    void Start()
    {
        if (textObject != null)
        {
            textRenderer = textObject.GetComponent<Renderer>();
        }
    }

    void Update()
    {
        if (mainCamera == null || textObject == null || textRenderer == null)
            return;

        Vector3 direction = textObject.transform.position - mainCamera.transform.position;
        float distance = direction.magnitude;
        direction.Normalize();

        // Получаем все объекты на пути луча
        RaycastHit[] hits = Physics.RaycastAll(mainCamera.transform.position, direction, distance);

        bool blocked = false;
        foreach (var hit in hits)
        {
            // Если мы встретили что-то, что не является текстом
            if (hit.collider.gameObject != textObject)
            {
                blocked = true;
                break;
            }
        }

        SetTextVisibility(!blocked);
    }

    void SetTextVisibility(bool visible)
    {
        if (textRenderer != null)
        {
            textRenderer.enabled = visible;
        }
    }
}

