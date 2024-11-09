using UnityEngine;
public class ParallaxLayerNew : MonoBehaviour
{
    [SerializeField] float multiplier = 0.0f;
    [SerializeField] bool horizontalOnly = true;
    public Transform cameraTransform;
    private Vector3 startCameraPos;
    private Vector3 startPos;
    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
        startCameraPos = cameraTransform.position;
        startPos = transform.position;
    }
    private void LateUpdate()
    {
        var position = startPos;
        if (cameraTransform != null) { 
            if (horizontalOnly)
                position.x += multiplier * (cameraTransform.position.x - startCameraPos.x);
            else
                position += multiplier * (cameraTransform.position - startCameraPos);
            transform.position = position;
        }
    }
}