using UnityEngine;

public class CameraDepth : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
    }
}
