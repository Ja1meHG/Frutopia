using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float maxForce = 10f; // Ajustar este valor según las necesidades del juego
    public int damage = 1;
    public LineRenderer trajectoryLine; // Línea para la trayectoria

    private Rigidbody2D rb;
    private bool isDragging = false;
    private Vector2 startPoint;
    private Player owner;
    private Player target;
    private GameController gameController;
    private CameraController cameraController;
    private Animator animator;

    public void Initialize(Player owner, Player target, GameController controller, CameraController cameraController)
    {
        this.owner = owner;
        this.target = target;
        this.gameController = controller;
        this.cameraController = cameraController;

        transform.position = owner.launchPosition.position;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Bloquea movimiento hasta que se lance

        // Configuración del LineRenderer
        trajectoryLine.sortingLayerName = "Bakcground"; // Asegúrate de que esta capa esté delante del mapa
        trajectoryLine.sortingOrder = 10; // Orden superior al resto de los objetos
        trajectoryLine.startColor = Color.white;
        trajectoryLine.endColor = Color.red;
        trajectoryLine.startWidth = 0.09f;
        trajectoryLine.endWidth = 0.09f;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 dragPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            UpdateTrajectory(dragPosition);
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

    }

    private void OnMouseDown()
    {
        isDragging = true;
        startPoint = transform.position;
        trajectoryLine.positionCount = 0; // Limpia la línea de trayectoria
    }

    private void OnMouseDrag()
    {
        Vector2 dragPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        UpdateTrajectory(dragPosition);
    }

    private void OnMouseUp()
    {
        isDragging = false;
        Vector2 releasePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Lanza el proyectil
        Vector2 force = (startPoint - releasePosition) * maxForce;
        Launch(force);
    }

    private void UpdateTrajectory(Vector2 dragPosition)
    {
        Vector2 force = (startPoint - dragPosition) * maxForce;
        Vector2[] trajectoryPoints = CalculateTrajectory(force);
        DrawTrajectory(trajectoryPoints);
    }

    private Vector2[] CalculateTrajectory(Vector2 force)
    {
        int numPoints = 30;
        Vector2[] points = new Vector2[numPoints];

        Vector2 startingPosition = transform.position;
        Vector2 velocity = force / rb.mass;

        for (int i = 0; i < numPoints; i++)
        {
            float t = i * 0.1f; // Tiempo entre puntos
            points[i] = startingPosition + velocity * t + 0.5f * Physics2D.gravity * t * t;
        }

        return points;
    }

    private void DrawTrajectory(Vector2[] points)
    {
        trajectoryLine.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            trajectoryLine.SetPosition(i, new Vector3(points[i].x, points[i].y, -1f)); // Ajuste del eje Z
        }
    }

    private void Launch(Vector2 force)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(force, ForceMode2D.Impulse); // Usa Impulse para aplicar una fuerza instantánea

        // Desactiva la línea de trayectoria después de lanzar
        trajectoryLine.positionCount = 0;

        // Informa a la cámara que siga el proyectil
        cameraController.FollowProjectile(transform);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == owner.gameObject)
        {
            Debug.Log("Impacto con el lanzador ignorado.");
            return;
        }

        if (collision.gameObject == target.gameObject)
        {
            Debug.Log($"Impacto en {target.playerName}");
            target.TakeDamage(damage);

            gameController.CheckGameOver();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Impacto en el suelo.");
        }

        StartDestroySequence();
    }

    private void StartDestroySequence()
    {
        Debug.Log("Activando parámetro Destroy y reproduciendo animación de destrucción.");
        animator.SetBool("Destroy", true);

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Comienza un ciclo de verificación para saber cuándo termina la animación
        InvokeRepeating("CheckAnimationEnd", 0.1f, 0.1f);
    }

    private void CheckAnimationEnd()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Verifica si la animación actual es "DestroyBola" y si ha terminado
        if (stateInfo.IsName("DestroyBola") && stateInfo.normalizedTime >= 0.6f)
        {
            Debug.Log("Animación de destrucción completada. Regresando al estado original.");
            animator.SetBool("Destroy", false); // Regresar al estado original
            CancelInvoke("CheckAnimationEnd"); // Detener el ciclo de verificación
            Destroy(gameObject); // Destruir el proyectil
            gameController.EndTurn(); // Finalizar el turno
        }
    }
}
