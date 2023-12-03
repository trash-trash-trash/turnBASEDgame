using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    public Camera mainCamera;
    private float cameraRotationAngle;

    public float offset;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0f + offset, mainCamera.transform.rotation.eulerAngles.y, 0f);
    }
}