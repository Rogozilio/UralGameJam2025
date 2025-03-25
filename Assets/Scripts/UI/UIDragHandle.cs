using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class UIDragHandle :MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Camera canvasCamera;
    private bool isDragging;
    private Vector2 offset;

    private Vector3 _originPosition;

    public RectTransform dropZone; // Зона, куда объект можно положить
    public UnityEvent onDropZone;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            canvasCamera = canvas.worldCamera; // Для WorldSpace
        }

        _originPosition = rectTransform.localPosition;
    }

    private void Update()
    {
        // Когда нажата ЛКМ
        if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOverUI())
            {
                isDragging = true;

                // Вычисляем смещение между центром объекта и курсором
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rectTransform,
                    Input.mousePosition,
                    canvasCamera,
                    out offset
                );
            }
        }

        // Когда ЛКМ отпущена
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            
            // Проверяем, пересекается ли объект с зоной
            if (IsOverDropZone())
            {
                onDropZone?.Invoke();
                // Устанавливаем объект в центр зоны
                // SnapToDropZone();
            }
            
            ReturnToRespawn();
        }

        // Если объект перетаскивается
        if (isDragging)
        {
            // Получаем текущую позицию мыши
            Vector2 mousePos = Input.mousePosition;

            // Переводим экранную позицию в локальные координаты
            Vector2 newPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                mousePos,
                canvasCamera,
                out newPos
            );

            rectTransform.localPosition = newPos - offset; // Устанавливаем новую позицию
        }
    }

    // Проверка, находится ли мышь над текущим объектом
    private bool IsMouseOverUI()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(
            rectTransform,
            Input.mousePosition,
            canvasCamera
        );
    }

    // Проверка, находится ли объект над зоной
    private bool IsOverDropZone()
    {
        // Если dropZone не задана, вернуть false
        if (dropZone == null) return false;

        return RectTransformUtility.RectangleContainsScreenPoint(
            dropZone,
            Input.mousePosition,
            canvasCamera
        );
    }

    // "Привязываем" объект к зоне
    private void SnapToDropZone()
    {
        if (dropZone == null) return;

        // Устанавливаем объект в центр Drop Zone
        rectTransform.position = dropZone.position;
    }
    
    private void ReturnToRespawn()
    {
        rectTransform.localPosition = _originPosition;
    }
}