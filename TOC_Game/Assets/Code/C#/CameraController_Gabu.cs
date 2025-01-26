using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController_Gabu : MonoBehaviour
{
    #region 変数

    public Camera camera;
    public float moveSpeed = 10f;
    public float dragSpeed = 10f;
    public float cameraPositionY = 10f;
    public float cameraPositionX = 10f;
    public float zoomSpeed = 0.1f;
    public float maxZoom = 50f;
    public float minZoom = 1f;
    private Vector2 moveInput;
    private Vector3 dragOrigin;
    private bool isDragging = false;
    private float correction = 0.1f;

    #endregion

    #region 関数


    private void OnEnable()
    {
        // PlayerInputコンポーネントを取得し、アクションを有効化
        var playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        playerInput.actions["Move"].performed += OnMoveCamera;
        playerInput.actions["Move"].canceled += OnMoveCamera;
        playerInput.actions["Drag"].started += OnDragCameraStart;
        playerInput.actions["Drag"].performed += OnDragCamera;
        playerInput.actions["Drag"].canceled += OnDragCameraEnd;
        playerInput.actions["Zoom"].performed += OnZoomCamera;
    }

    private void OnDisable()
    {
        // PlayerInputコンポーネントを取得し、アクションを無効化
        var playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        playerInput.actions["Move"].performed -= OnMoveCamera;
        playerInput.actions["Move"].canceled -= OnMoveCamera;
        playerInput.actions["Drag"].started -= OnDragCameraStart;
        playerInput.actions["Drag"].performed -= OnDragCamera;
        playerInput.actions["Drag"].canceled -= OnDragCameraEnd;
        playerInput.actions["Zoom"].performed -= OnZoomCamera;
    }


    private void OnMoveCamera(InputAction.CallbackContext context)
    {
        // キーボード入力値を取得
        moveInput = context.ReadValue<Vector2>();
    }


    private void OnDragCameraStart(InputAction.CallbackContext context)
    {
        // マウスドラッグの開始位置を記録
        dragOrigin = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        isDragging = true;
    }
    private void OnDragCamera(InputAction.CallbackContext context)
    {
        if (isDragging)
        {
            // マウスドラッグによるカメラ移動
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3 move = dragOrigin - currentPos;
            transform.Translate(move * dragSpeed, Space.World);
        }
    }
    private void OnDragCameraEnd(InputAction.CallbackContext context)
    {
        // ドラッグ終了
        isDragging = false;
    }


    private void OnZoomCamera(InputAction.CallbackContext context)
    {
        // マウススクロールによるカメラのズーム
        float scrollValue = context.ReadValue<float>() * zoomSpeed;
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - scrollValue, minZoom, maxZoom);
    }

    #endregion

    private void Start()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
    }

    private void Update()
    {
        // キーボードによるカメラの移動
        Vector3 move = new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + move;

        // カメラの位置が制限を超えないようにする
        newPosition.x = Mathf.Clamp(newPosition.x, -cameraPositionX, cameraPositionX);
        newPosition.y = Mathf.Clamp(newPosition.y, -cameraPositionY, cameraPositionY);

        transform.position = newPosition;

        // マウスドラッグによるカメラの移動
        if (isDragging)
        {
            // ドラッグ中は毎フレームカメラを移動
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            if (Vector3.Distance(dragOrigin, currentPosition) < correction)
            {
                return;
            }

            move = dragOrigin - currentPosition;
            newPosition = transform.position + move;

            // カメラの位置が制限を超えないようにする
            newPosition.x = Mathf.Clamp(newPosition.x, -cameraPositionX, cameraPositionX);
            newPosition.y = Mathf.Clamp(newPosition.y, -cameraPositionY, cameraPositionY);

            transform.position = newPosition;

            // 現在の位置を次のフレームの開始位置として更新
            //dragOrigin = currentPosition;
        }
    }
}
