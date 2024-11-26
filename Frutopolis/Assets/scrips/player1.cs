using UnityEngine;

public class player1 : MonoBehaviour
{
    public Animator animator; // Referencia al Animator
    private bool atacando = false; // Estado actual de ataque
    private bool animacionFinalizada = false; // Indica si se ejecutaron los últimos cuadros
    public float porcentajePausa = 0.75f; // Punto de pausa (75% de la animación)

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!atacando)
            {
                // Iniciar ataque
                atacando = true;
                animacionFinalizada = false;
                animator.SetBool("atacando", true); // Activar animación de ataque
                animator.speed = 1f; // Velocidad normal
            }

            // Pausar la animación en el 75% (posición 6)
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("ataque") && stateInfo.normalizedTime >= porcentajePausa && !animacionFinalizada)
            {
                animator.speed = 0f; // Pausar la animación
            }
        }
        else if (atacando)
        {
            // Reanudar animación al soltar el botón
            if (!animacionFinalizada)
            {
                animator.speed = 1f; // Reanudar velocidad normal
                animacionFinalizada = true; // Marcar como finalizada
            }

            // Reiniciar el estado
            AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (currentInfo.IsName("ataque") && currentInfo.normalizedTime >= 1f)
            {
                atacando = false;
                animator.SetBool("atacando", false); // Volver a Idle
            }
        }
    }
}
