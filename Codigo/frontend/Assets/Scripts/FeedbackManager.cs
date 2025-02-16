using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FeedbackManager : MonoBehaviour
{
    public GameObject feedbackPanel; // Panel de feedback dentro del FeedbackCanvas
    public TextMeshProUGUI feedbackText; // Texto del panel de feedback
    public Button retryButton; // Bot�n para intentar de nuevo
    public Button exitButton; // Bot�n para salir

    void Start()
    {
        feedbackPanel.SetActive(false); // Ocultar panel al inicio

        retryButton.onClick.AddListener(CloseFeedback); // Asignar evento al bot�n
        exitButton.onClick.AddListener(ExitFeedback); // Si necesitas que haga otra acci�n, c�mbialo
    }

    public void ShowFeedback(string message)
    {
        feedbackText.text = message;
        feedbackPanel.SetActive(true);
    }

    public void CloseFeedback()
    {
        feedbackPanel.SetActive(false);
        // Aqu� puedes llamar a la funci�n que vuelva a mostrar la pregunta si lo deseas

        Debug.Log("Reiniciando el juego...");
        Time.timeScale = 1f; // Reactiva el tiempo del juego

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitFeedback()
    {
        feedbackPanel.SetActive(false);
        // Aqu� puedes programar que regrese al men� o al juego normal
    }
}

