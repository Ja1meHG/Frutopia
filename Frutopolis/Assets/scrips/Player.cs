using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName;
    public int playerHealth = 3;
    public Transform launchPosition;
    public float porcentajePausa = 0.75f; // Punto de pausa (75% de la animación)
    private Animator animator;

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        Debug.Log(playerName + " ha recibido " + damage + " puntos de daño. Salud restante: " + playerHealth);
    }   

    void Update()
    {
        // Si el jugador presiona el botón izquierdo del ratón se activa la animación de ataque
        if (Input.GetMouseButton(0))
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            // Iniciar ataque
            animator.SetBool("atacando", true); // Activar animación de ataque
            animator.speed = 1f; // Velocidad normal

            // Pausar la animación en el 75% (posición 6)
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("ataque") && stateInfo.normalizedTime >= porcentajePausa)
            {
                animator.speed = 0f; // Pausar la animación
            }
        }
        else
        {
            // Reanudar animación al soltar el botón
            if (animator != null)
            {
                animator.speed = 1f; // Reanudar velocidad normal
                animator.SetBool("atacando", false); // Volver a Idle
            }
        }
    }
}
