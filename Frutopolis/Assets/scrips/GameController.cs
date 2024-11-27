using UnityEngine;

public class GameController : MonoBehaviour
{
    public Player player1;
    public Player player2;
    public Projectile projectilePrefab; // Prefab del proyectil
    public CameraController cameraController; // Referencia al controlador de la cámara

    private Player currentPlayer; // Jugador actual
    private Player opponentPlayer; // Jugador contrario
    [SerializeField] private UIManager uiManager;

    void Start()
    {
        currentPlayer = player1;
        opponentPlayer = player2;

        StartTurn();
    }

    void StartTurn()
    {
        Debug.Log($"Turno de {currentPlayer.name}");

        // Mueve la cámara al jugador actual
        cameraController.MoveToTarget(currentPlayer.transform);

        // Crear y preparar el proyectil
        Projectile projectile = Instantiate(projectilePrefab, currentPlayer.launchPosition.position, Quaternion.identity);
        projectile.Initialize(currentPlayer, opponentPlayer, this, cameraController);
    }

    public void EndTurn()
    {
        // Cambia al jugador contrario
        if (currentPlayer == player1)
        {
            currentPlayer = player2;
            opponentPlayer = player1;
        }
        else
        {
            currentPlayer = player1;
            opponentPlayer = player2;
        }

        StartTurn();
    }

    public void CheckGameOver()
    {
        if (player1.playerHealth <= 0)
        {
            uiManager.MostrarPantallaGanador("Jugador 2");
            Debug.Log("Jugador 2 gana!");
            EndGame();
        }
        else if (player2.playerHealth <= 0)
        {
            uiManager.MostrarPantallaGanador("Jugador 1");
            Debug.Log("Jugador 1 gana!");
            EndGame();
        }
    }

    void EndGame()
    {
        Debug.Log("Fin del juego.");
        Time.timeScale = 0; // Detener el juego
    }

}
