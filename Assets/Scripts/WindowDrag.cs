
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDrag : MonoBehaviour,
    IDragHandler,
    IPointerDownHandler
{
    [SerializeField]
    private RectTransform window;

    // クリックしたら最前面にする
    public void OnPointerDown(PointerEventData eventData)
    {
        window.SetAsLastSibling();
    }

    // ドラッグで移動
    public void OnDrag(PointerEventData eventData)
    {
        window.position += (Vector3)eventData.delta;

        // 画面外に出ないように制限
        ClampWindowPosition();
    }

    private void ClampWindowPosition()
    {
        Vector3[] corners = new Vector3[4];
        window.GetWorldCorners(corners);

        float minX = corners[0].x;
        float maxX = corners[2].x;
        float minY = corners[0].y;
        float maxY = corners[2].y;

        float offsetX = 0f;
        float offsetY = 0f;

        // 左端
        if (minX < 0)
            offsetX = -minX;

        // 右端
        if (maxX > Screen.width)
            offsetX = Screen.width - maxX;

        // 下端
        if (minY < 0)
            offsetY = -minY;

        // 上端
        if (maxY > Screen.height)
            offsetY = Screen.height - maxY;

        window.position += new Vector3(offsetX, offsetY, 0);
    }
}
