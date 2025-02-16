using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject questionPanel;
    public GameObject wrongAnswerPanel;
    public GameObject feedbackPanel;

    public TMPro.TextMeshProUGUI questionText; // Texto para la pregunta
    public TMPro.TextMeshProUGUI feedbackText; // Texto para la retroalimentación

    private GameManager gm; // Referencia al GameManager

    private void Start()
    {
        //gm = FindObjectOfType<GameManager>();
        gm = FindFirstObjectByType<GameManager>();
        HideAllPanels(); // Asegúrate de que todas las ventanas estén ocultas al inicio
    }

    private void HideAllPanels()
    {
        questionPanel.SetActive(false);
        wrongAnswerPanel.SetActive(false);
        feedbackPanel.SetActive(false);
    }

    public void ShowQuestion(string question, string[] options, System.Action<int> onAnswerSelected)
    {
        HideAllPanels();
        questionPanel.SetActive(true);
        questionText.text = question;

        // Configurar botones
        for (int i = 0; i < options.Length; i++)
        {
            int optionIndex = i; // Captura la variable local
            var button = questionPanel.transform.GetChild(i + 1).GetComponent<UnityEngine.UI.Button>();
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = options[i];
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onAnswerSelected(optionIndex));
        }
    }

    public void ShowWrongAnswerPanel()
    {
        HideAllPanels();
        wrongAnswerPanel.SetActive(true);
    }

    public void ShowFeedback(string feedback)
    {
        HideAllPanels();
        feedbackPanel.SetActive(true);
        feedbackText.text = feedback;
    }

    public void RetryButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Asegúrate de que el menú principal tenga esta escena
    }
}

