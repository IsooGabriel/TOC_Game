using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static DBManager_Gabu;
using System.Runtime.InteropServices;

public class PlayerInputManager : MonoBehaviour
{
    #region Variáveis


    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);

    public float moveSpeed = 5f; // Velocidade do movimento
    public float limit = 30;
    private Vector2 moveInput; // Input de movimento
    private Transform playerTransform; // Transform do jogador
    public GameObject menuObject;

    public readonly float mouseSpeed = 5f;

    public Player_Gabu player;

    #endregion

    #region Funções


    private void OnEnable()
    {
        // Ativar as ações de input
        var playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        playerInput.actions["Move"].performed += OnMovePlayer;
        playerInput.actions["Move"].canceled += OnMovePlayer;
        playerInput.actions["Attack"].canceled += Attack;
        playerInput.actions["Reroll"].started += OnRerollStarted;
        playerInput.actions["Reroll"].performed += OnRerollCanceled;
        playerInput.actions["Reroll"].canceled += OnRerollCanceled;
        playerInput.actions["Menu"].started += OnPlessMenu;
        playerInput.actions["MoveMouse"].performed += MoveMouse;
    }

    private void OnDisable()
    {
        // Desativar as ações de input
        var playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        playerInput.actions["Move"].performed -= OnMovePlayer;
        playerInput.actions["Move"].canceled -= OnMovePlayer;
        playerInput.actions["Attack"].canceled -= Attack;
        playerInput.actions["Reroll"].started -= OnRerollStarted;
        playerInput.actions["Reroll"].performed -= OnRerollCanceled;
        playerInput.actions["Reroll"].canceled -= OnRerollCanceled;
        playerInput.actions["Menu"].started -= OnPlessMenu;
        playerInput.actions["MoveMouse"].performed -= MoveMouse;
    }

    private void OnMovePlayer(InputAction.CallbackContext context)
    {
        // Captura o input de movimento (teclado, controle, etc)
        moveInput = context.ReadValue<Vector2>();
        player.Move(moveInput);
    }

    private void OnPlessMenu(InputAction.CallbackContext context)
    {
        Time.timeScale = menuObject.activeSelf ? 1 : 0;
        menuObject.SetActive(!menuObject.activeSelf);
    }

    private void Attack(InputAction.CallbackContext context)
    {
        // マウスカーソルの位置を取得し、ワールド座標に変換
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0; // 2Dゲームの場合、Z座標を0に設定

        // プレイヤーの位置とマウスカーソルの位置から発射角度を計算
        Vector3 direction = (mouseWorldPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        player.AttackButton(Quaternion.Euler(0,0,angle));
    }

    // ボタンが押された瞬間
    private void OnRerollStarted(InputAction.CallbackContext context)
    {
        player.rerollTime = player.rerollSpeed;
    }
    private void OnRerollPreform(InputAction.CallbackContext context)
    {
        player.Reroll();
    }
    // ボタンが放された瞬間
    private void OnRerollCanceled(InputAction.CallbackContext context)
    {
        return;
    }


    private Vector3 MovePlayerDirectly()
    {
        // Calcula o deslocamento baseado no input
        return new Vector3(moveInput.x, moveInput.y, 0) ;
    }

    public void MoveMouse(InputAction.CallbackContext context)
    {
        Vector2 moveMouse = context.ReadValue<Vector2>();
        // マウスカーソルの位置を取得し、ワールド座標に変換
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.x = Mathf.Clamp(moveMouse.x+ mouseWorldPosition.x, -Screen.width, Screen.width);
        mouseWorldPosition.y = Mathf.Clamp(moveMouse.y + mouseWorldPosition.y, -Screen.height, Screen.height);

        SetCursorPos((int)mouseWorldPosition.x,(int)mouseWorldPosition.y);
    }

    #endregion

    private void Awake()
    {
        // Obter o componente Transform
        playerTransform = GetComponent<Transform>();
        moveSpeed = DB.playerDBs[DB.AccountID].speed;
        menuObject.SetActive(false);
    }
    private void Update()
    {
        // Atualizar a movimentação direta
        playerTransform.position += MovePlayerDirectly() * player.speed * Time.deltaTime;
        playerTransform.position = new Vector3(Mathf.Clamp(playerTransform.position.x, -limit, limit), Mathf.Clamp(playerTransform.position.y, -limit, limit), 0);
    }

}
