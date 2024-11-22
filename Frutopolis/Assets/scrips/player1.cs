using UnityEngine;

public class player1 : MonoBehaviour
{
    public Animator animator; // Referencia al Animator
    private bool atacando = false; // Estado actual de ataque
    public float duracionAtaque = 0.35f; // Duración de la animación de ataque en segundos

    void Start()
    {
        // Intenta obtener el Animator automáticamente si no está asignado
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Detectar si se presiona la tecla E y no estamos ya atacando
        if (Input.GetKeyDown(KeyCode.E) && !atacando)
        {
            // Activar el estado de ataque
            atacando = true;
            animator.SetBool("atacando", true); // Activar la animación

            // Programar la desactivación del ataque después de la duración de la animación
            Invoke("TerminarAtaque", duracionAtaque);
        }
    }

    // Método para finalizar el ataque y volver al estado Idle
    void TerminarAtaque()
    {
        atacando = false;
        animator.SetBool("atacando", false); // Regresar a Idle
    }
}
