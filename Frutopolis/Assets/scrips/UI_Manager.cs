using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> listaCorazones;
    [SerializeField] private Sprite corazonDesactivado;
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private TMPro.TextMeshProUGUI textoDerrota;

    private bool isPaused = false;
    private string[] frasesDerrota = {
        "!Miau-serable derrota! !Inténtalo de nuevo!",
        "!No te preocupes! !Tienes 7 vidas más!",
        "!Parece que hoy el perro tuvo un buen día... y tú no! !A seguir entrenando!",
        "!Te dejaron con los pelos de punta... y no precisamente de emoción! !Vuelve a intentarlo!",
        "!Parece que necesitas más que bolas de estambre para vencer a ese perro! !Piensa en una nueva estrategia!",
        "!Ups! Parece que el desarrollador del juego hizo al perro muy fuerte. !Perdón!",
        "!No te rindas! !El programador está trabajando en una actualización para que los gatos sean más poderosos!",
        "!Error 404: Victoria no encontrada! !Inténtalo de nuevo!",
        "!Parece que te mandaron al paraíso de los gatos... !pero puedes volver!"
    };

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

        

    public void RestaCorazones(int indice)
    {
        Image imagenCorazon = listaCorazones[indice].GetComponent<Image>();
        imagenCorazon.sprite = corazonDesactivado;
    }


    public void SeleccionarTextoDerrota()
    {
        textoDerrota.text = frasesDerrota[UnityEngine.Random.Range(0, frasesDerrota.Length)];
    }

    public void MostrarGameOver(object sender, EventArgs e)
    {
        SeleccionarTextoDerrota();
        panelGameOver.SetActive(true);
    }

    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
