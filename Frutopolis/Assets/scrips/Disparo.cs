using UnityEngine;

public class Disparo : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float maxDistance;

    private Camera mainCamara;
    private Rigidbody2D rb;
    private Vector2 startPosition, ClampedPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamara = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        startPosition = transform.position;
    }
    private void OnMouseDrag()
    {
        SetPosition();
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
       Throw();
    }

    private void Throw()
    {
        // Cambia el Rigidbody a dinámico para permitir que las fuerzas físicas lo afecten
        rb.bodyType = RigidbodyType2D.Dynamic;

        // Calcula el vector de lanzamiento
        Vector2 throwVector = startPosition - ClampedPosition;

        // Aplica la fuerza de lanzamiento
        rb.AddForce(throwVector * force);
        float resetTime = 2f;
        Invoke("Reset", resetTime);
    }

   private void Reset()
   {
      transform.position = startPosition;
      rb.bodyType = RigidbodyType2D.Kinematic;
      rb.linearVelocity = Vector2.zero;
      mainCamara.GetComponent<CamaraMovent>().ResetPosition();
   }

}
