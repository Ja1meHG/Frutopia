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
    [SerializeField] private GameObject panelGanador;
    [SerializeField] private TMPro.TextMeshProUGUI textoGanador;

    private bool isPaused = false;
    private string[] frasesDerrota = {
        "Jugador 1 ha sido derrotado",
        "Jugador 2 ha sido derrotado",
        "¡Has perdido!",
        "¡Inténtalo de nuevo!",
        "¡No te rindas!",
        "¡Sigue intentándolo!",
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

    public void MostrarPantallaGanador(string ganador)
    {
        panelGanador.SetActive(true);
        textoGanador.text = "El ganador es " + ganador;
    }

}
