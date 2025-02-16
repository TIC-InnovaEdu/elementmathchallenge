using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FeedbackManager : MonoBehaviour
{
    public GameObject feedbackPanel; // Panel de feedback dentro del FeedbackCanvas
    public TextMeshProUGUI feedbackText; // Texto del panel de feedback
    public Button retryButton; // Botón para intentar de nuevo
    public Button exitButton; // Botón para salir

    void Start()
    {
        feedbackPanel.SetActive(false); // Ocultar panel al inicio

        retryButton.onClick.AddListener(CloseFeedback); // Asignar evento al botón
        exitButton.onClick.AddListener(ExitFeedback); // Si necesitas que haga otra acción, cámbialo
    }

    public void ShowFeedback(string message)
    {
        feedbackText.text = message;
        feedbackPanel.SetActive(true);
    }

    public void CloseFeedback()
    {
        feedbackPanel.SetActive(false);
        // Aquí puedes llamar a la función que vuelva a mostrar la pregunta si lo deseas

        Debug.Log("Reiniciando el juego...");
        Time.timeScale = 1f; // Reactiva el tiempo del juego

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitFeedback()
    {
        feedbackPanel.SetActive(false);
        // Aquí puedes programar que regrese al menú o al juego normal
    }
}

