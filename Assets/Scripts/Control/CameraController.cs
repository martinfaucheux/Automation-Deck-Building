using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 20f;
    [SerializeField] float _maxDistance = 100f;

    [SerializeField] float _zoomScale = 100f;
    [SerializeField] float _maxCameraSize = 20;
    [SerializeField] float _minCameraSize = 5;

    void Update()
    {
        Vector2 movement = Vector2.zero;
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
            movement += Vector2.up;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            movement += Vector2.down;

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
            movement += Vector2.left;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            movement += Vector2.right;

        MoveTransform(movement);
        Zoom();
    }

    private void MoveTransform(Vector2 movement)
    {
        Vector3 newPosition = transform.position + ((Vector3)movement) * _moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -_maxDistance, _maxDistance);
        newPosition.y = Mathf.Clamp(newPosition.y, -_maxDistance, _maxDistance);
        transform.position = newPosition;
    }

    private void Zoom()
    {
        float zoomRate = -Input.mouseScrollDelta.y * _zoomScale * Time.deltaTime;
        float orthographicSize = Mathf.Clamp(
            Camera.main.orthographicSize + zoomRate,
            _minCameraSize,
            _maxCameraSize
        );
        Camera.main.orthographicSize = orthographicSize;
    }
}
