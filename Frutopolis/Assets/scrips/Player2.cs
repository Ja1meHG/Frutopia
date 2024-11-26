using Unity.VisualScripting;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ShouldDie(collision))
        {
            Destroy(gameObject);
        }
    }

    private bool ShouldDie(Collision2D collision)
    {
        bool isDisparo = collision.gameObject.GetComponent<Disparo>();

        if (isDisparo)
        {
            return true;
        }

        return false;
    }
}
