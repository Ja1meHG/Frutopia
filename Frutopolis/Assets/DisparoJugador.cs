using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 [SerializeField] private Transform ControladorDisparo;
[SerializeField] private GameObject bala;

    private void Update()
    {
      if (Input.GetButtonDown("Fire1")){
            Disparar();
      }
    }

    private void Disparar(){
        Instantiate(bala, ControladorDisparo.position, ControladorDisparo.rotation);
    }

}
