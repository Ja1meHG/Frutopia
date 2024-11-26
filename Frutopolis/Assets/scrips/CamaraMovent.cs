using UnityEngine;

public class CamaraMovent : MonoBehaviour
{
    [SerializeField] private Transform disparoTransform;
    [SerializeField] private float xStopPosition;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    private void LateUpdate()
    {
        if(disparoTransform.position.x > transform.position.x && transform.position.x < xStopPosition)
        {
            transform.position = new Vector3(disparoTransform.position.x, transform.position.y, transform.position.z);
        }
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
    }
}
