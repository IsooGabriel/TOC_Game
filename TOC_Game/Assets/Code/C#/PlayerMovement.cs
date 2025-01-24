using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variáveis

    public float moveSpeed = 5f; // Velocidade do movimento
    private Vector2 moveInput; // Input de movimento
    private Transform playerTransform; // Transform do jogador

    private float pressStartTime; // ボタンを押した時刻
    private const float longPressThreshold = 0.5f; // 長押しの閾値（秒）

    public Player_Gabu player;

    #endregion

    #region Funções


    private void OnEnable()
    {
        // Ativar as ações de input
        var playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Move"].performed += OnMovePlayer;
        playerInput.actions["Move"].canceled += OnMovePlayer;
        playerInput.actions["Attack"].performed += OnAttack;
        playerInput.actions["Attack"].canceled += OnAttack;
    }

    private void OnDisable()
    {
        // Desativar as ações de input
        var playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Move"].performed -= OnMovePlayer;
        playerInput.actions["Move"].canceled -= OnMovePlayer;
        playerInput.actions["Attack"].performed -= OnAttack;
        playerInput.actions["Attack"].canceled -= OnAttack;
    }

    private void OnMovePlayer(InputAction.CallbackContext context)
    {
        // Captura o input de movimento (teclado, controle, etc)
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        // Vector3をquaternionに変換
        Quaternion rotation = Quaternion.Euler(MovePlayerDirectly());
        player.Attack(rotation);
    }

    // ボタンが押された瞬間
    private void OnButtonStarted(InputAction.CallbackContext context)
    {
        pressStartTime = Time.time; // ボタンを押した時刻を記録
    }

    // ボタンが放された瞬間
    private void OnButtonCanceled(InputAction.CallbackContext context)
    {
        float pressDuration = Time.time - pressStartTime; // ボタンを押していた時間を計算

        if (pressDuration >= longPressThreshold)
        {
        }
        else
        {
        }
    }

    private Vector3 MovePlayerDirectly()
    {
        // Calcula o deslocamento baseado no input
        return new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;
    }

    #endregion

    private void Awake()
    {
        // Obter o componente Transform
        playerTransform = GetComponent<Transform>();
    }
    private void Update()
    {
        // Atualizar a movimentação direta
        playerTransform.position += MovePlayerDirectly();
    }

}
