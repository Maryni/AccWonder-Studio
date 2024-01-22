using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    private ScrollRect scrollRect;
    private float contentOriginalY;

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();

        if (scrollRect != null)
        {
            scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
        }

        contentOriginalY = scrollRect.content.localPosition.y;
    }

    private void OnScrollValueChanged(Vector2 normalizedPosition)
    {
        if (normalizedPosition.y < 0)
        {
            scrollRect.content.localPosition = new Vector3(scrollRect.content.localPosition.x, contentOriginalY - normalizedPosition.y * scrollRect.content.rect.height, scrollRect.content.localPosition.z);
        }
    }

    private void OnDestroy()
    {
        if (scrollRect != null)
        {
            scrollRect.onValueChanged.RemoveListener(OnScrollValueChanged);
        }
    }
}
