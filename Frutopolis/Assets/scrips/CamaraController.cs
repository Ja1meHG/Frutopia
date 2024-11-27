using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float smoothSpeed = 0.125f; // Velocidad de movimiento suave de la cámara
    public Vector3 offset; // Offset de la cámara con respecto al objetivo
    private Transform target; // El objetivo actual de la cámara
    private bool followingProjectile = false; // Indica si la cámara sigue al proyectil

    public void MoveToTarget(Transform newTarget)
    {
        // Cambia el objetivo de la cámara y desactiva el seguimiento del proyectil
        target = newTarget;
        followingProjectile = false;
        offset = new Vector3(0, 1, -10); // Ajusta el offset predeterminado
    }

    public void FollowProjectile(Transform projectileTransform)
    {
        // Activa el seguimiento del proyectil
        target = projectileTransform;
        followingProjectile = true;
        offset = new Vector3(0, -0.5f, -10); // Ajusta el offset para seguir al proyectil
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
