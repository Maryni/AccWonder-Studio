using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    private ScrollRect scrollRect;
    private float contentOriginalY;

    private void Start()
    {
        // Отримати компонент ScrollRect
        scrollRect = GetComponent<ScrollRect>();

        // Підписатися на подію onValueChanged
        if (scrollRect != null)
        {
            scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
        }

        // Зберегти початковий Y контенту
        contentOriginalY = scrollRect.content.localPosition.y;
    }

    private void OnScrollValueChanged(Vector2 normalizedPosition)
    {
        // Перевірити, чи контент виходить за межі вниз
        if (normalizedPosition.y < 0)
        {
            // Змістіть весь вміст (включаючи всі компоненти) вгору на величину normalizedPosition.y
            scrollRect.content.localPosition = new Vector3(scrollRect.content.localPosition.x, contentOriginalY - normalizedPosition.y * scrollRect.content.rect.height, scrollRect.content.localPosition.z);
        }
    }

    // Відписатися від події при знищенні скрипта
    private void OnDestroy()
    {
        if (scrollRect != null)
        {
            scrollRect.onValueChanged.RemoveListener(OnScrollValueChanged);
        }
    }
}
