using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanelManager : MonoBehaviour
{
    // Método para intentar de nuevo el nivel
    public void Button_IDN()
    {
        Time.timeScale = 1f; // Reactiva el tiempo del juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reinicia el nivel actual
    }

    // Método para regresar al menú principal
    public void ButtonMenu()
    {
        Time.timeScale = 1f; // Reactiva el tiempo del juego
        SceneManager.LoadScene("MenuPrincipal"); // Cambia "MainMenu" por el nombre exacto de tu escena del menú principal
    }
}

