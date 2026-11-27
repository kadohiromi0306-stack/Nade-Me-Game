
using UnityEngine;
using UnityEngine.UI;

public class CircleRaycast : MonoBehaviour, ICanvasRaycastFilter
{
    [Range(0.1f, 1f)]
    [SerializeField] private float radiusScale = 1f;

    public bool IsRaycastLocationValid(
        Vector2 screenPoint,
        Camera eventCamera)
    {
        RectTransform rect = GetComponent<RectTransform>();

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect,
            screenPoint,
            eventCamera,
            out Vector2 localPoint))
        {
            return false;
        }

        Rect area = rect.rect;

        float radiusX = area.width * 0.5f * radiusScale;
        float radiusY = area.height * 0.5f * radiusScale;

        float dx = (localPoint.x - area.center.x) / radiusX;
        float dy = (localPoint.y - area.center.y) / radiusY;

        return dx * dx + dy * dy <= 1f;
    }
}
