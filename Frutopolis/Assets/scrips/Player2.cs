using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private int vidaPersonaje = 6;
    private bool isGrounded;
    public event EventHandler OnPlayerDeath;
    [SerializeField] private UIManager uiManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (ShouldDie(collision))
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    // private bool ShouldDie(Collision2D collision)
    // {
    //     bool isDisparo = collision.gameObject.GetComponent<Disparo>();

    //     if (isDisparo)
    //     {
    //         return true;
    //     }

    //     return false;
    // }

    public void PersonajeHurt(){
        if(vidaPersonaje > 0){
            vidaPersonaje--;
            uiManager.RestaCorazones(vidaPersonaje);
            anim.SetBool("dañado", true);
            StartCoroutine(ResetAnimation("dañado"));

        } if(vidaPersonaje == 0){
            Die();
        }
    }

        private void Die()
    {
        anim.SetBool("IsDead", true);
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        gameObject.SetActive(false);
        //Destroy(gameObject, 0.6f);
        Debug.Log("Game Over");
    }

    public bool IsDead()
    {
        return vidaPersonaje <= 0;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprueba si el personaje toca el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    IEnumerator ResetAnimation(string animation)
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool(animation, false);
    }
}
