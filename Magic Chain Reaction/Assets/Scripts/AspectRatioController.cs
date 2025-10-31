using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioController : MonoBehaviour
{
    [Tooltip("Target aspect ratio = width / height. Example: 1600f / 830f")]
    public float targetAspect = 1600f / 830f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        ApplyAspect();
    }

    void ApplyAspect()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            // Letterbox (black bars top/bottom)
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            cam.rect = rect;
        }
        else
        {
            // Pillarbox (black bars left/right)
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            cam.rect = rect;
        }
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);
}
