using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variáveis

    public float moveSpeed = 5f; // Velocidade do movimento
    private Vector2 moveInput; // Input de movimento
    private Transform playerTransform; // Transform do jogador

    #endregion

    #region Funções

    private void Awake()
    {
        // Obter o componente Transform
        playerTransform = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        // Ativar as ações de input
        var playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Move"].performed += OnMovePlayer;
        playerInput.actions["Move"].canceled += OnMovePlayer;
    }

    private void OnDisable()
    {
        // Desativar as ações de input
        var playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Move"].performed -= OnMovePlayer;
        playerInput.actions["Move"].canceled -= OnMovePlayer;
    }

    private void OnMovePlayer(InputAction.CallbackContext context)
    {
        // Captura o input de movimento (teclado, controle, etc)
        moveInput = context.ReadValue<Vector2>();
    }

    #endregion

    private void Update()
    {
        // Atualizar a movimentação direta
        MovePlayerDirectly();
    }

    private void MovePlayerDirectly()
    {
        // Calcula o deslocamento baseado no input
        Vector3 move = new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;

        // Aplica o movimento diretamente ao transform
        playerTransform.position += move;
    }
}
