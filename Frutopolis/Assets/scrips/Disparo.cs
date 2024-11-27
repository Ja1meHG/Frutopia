using UnityEngine;

public class Disparo : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float maxDistance;
    [SerializeField] private float maxFlightTime = 1f; // Duración máxima en segundos antes de reiniciar

    private Camera mainCamara;
    private Rigidbody2D rb;
    private Vector2 startPosition, ClampedPosition;
    private Animator animator;
    private bool isDestroying = false;
    private bool isFlying = false; // Indica si la bola está en vuelo
    private float flightTimer = 0f; // Contador de tiempo en vuelo

    void Start()
    {
        mainCamara = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator no encontrado en el GameObject");
        }
    }

    private void Update()
    {
        // Si la bola está volando, aumenta el contador de tiempo
        if (isFlying && !isDestroying)
        {
            flightTimer += Time.deltaTime;

            // Si el tiempo de vuelo supera el máximo permitido, reinicia la bola
            if (flightTimer >= maxFlightTime)
            {
                Debug.Log("Tiempo máximo de vuelo alcanzado. Reseteando la bola.");
                StartDestroySequence();
            }
        }
    }

    private void OnMouseDrag()
    {
        if (!isDestroying)
        {
            SetPosition();
        }
    }

    private void SetPosition()
    {
        Vector2 dragPosition = mainCamara.ScreenToWorldPoint(Input.mousePosition);
        ClampedPosition = dragPosition;

        float dragDistance = Vector2.Distance(startPosition, dragPosition);

        if (dragDistance > maxDistance)
        {
            ClampedPosition = startPosition + (dragPosition - startPosition).normalized * maxDistance;
        }

        if (dragPosition.x > startPosition.x)
        {
            ClampedPosition.x = startPosition.x;
        }

        transform.position = ClampedPosition;
    }

    private void OnMouseUp()
    {
        if (!isDestroying)
        {
            Throw();
        }
    }

    private void Throw()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 throwVector = startPosition - ClampedPosition;
        rb.AddForce(throwVector * force);

        // Marca la bola como en vuelo y reinicia el temporizador
        isFlying = true;
        flightTimer = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            Debug.Log($"Colisión detectada con: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

    if (!isDestroying)
    {
        // Verifica si colisiona con un enemigo
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            Debug.Log("Impacto detectado en el enemigo.");

            // Obtén el script del enemigo y reduce su vida
            Player2 enemy = collision.gameObject.GetComponent<Player2>();
            if (enemy != null)
            {
                enemy.PersonajeHurt();
            }

            StartDestroySequence();
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Impacto detectado en el suelo.");
            StartDestroySequence();
        }
    }
    }

    private void StartDestroySequence()
    {
        isDestroying = true;
        isFlying = false; // Detener el seguimiento del tiempo de vuelo

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
        if (stateInfo.IsName("DestroyBola") && stateInfo.normalizedTime >= 1f)
        {
            Debug.Log("Animación de destrucción completada. Regresando al estado original.");
            animator.SetBool("Destroy", false); // Regresar al estado original
            CancelInvoke("CheckAnimationEnd"); // Detener el ciclo de verificación
            Reset(); // Llamar al método de reinicio
        }
    }

    private void Reset()
    {
        Debug.Log("Reseteando la bola.");
        animator.SetBool("Destroy", false); // Asegurar el regreso al estado original
        transform.position = startPosition;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        mainCamara.GetComponent<CamaraMovent>().ResetPosition();
        isFlying = false; // Detener el estado de vuelo
        flightTimer = 0f; // Reinicia el contador de tiempo
        isDestroying = false; // Permitir que la bola se pueda usar nuevamente
    }
}