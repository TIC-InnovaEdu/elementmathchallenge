using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public GameObject questionCanvas;
    public Text questionText;
    public Button[] optionButtons;
    public Text[] optionTexts;
    public FeedbackManager feedbackManager; // Referencia al FeedbackManager


    private int correctAnswerIndex;
    private Gem currentGem; // Referencia a la gema que activ� la pregunta
    private Question selectedQuestion; // Nueva variable para almacenar la pregunta actual

    // Lista de preguntas predefinidas
    private List<Question> questions = new List<Question> {
    new Question("�Cu�nto es 7 + 5?", new string[] { "10", "11", "12", "13" }, 2),
    new Question("�Cu�l es el resultado de 9 - 4?", new string[] { "4", "5", "6", "7" }, 1),
    new Question("�Cu�nto es 6 � 3?", new string[] { "12", "15", "18", "20" }, 2),
    new Question("�Cu�l es el resultado de 20 � 4?", new string[] { "4", "5", "6", "7" }, 1),
    new Question("Si un tri�ngulo tiene tres lados, �cu�ntos lados tiene un pent�gono?", new string[] { "4", "5", "6", "7" }, 1),
    new Question("�Cu�nto es 8 � 7?", new string[] { "48", "54", "56", "63" }, 2),
    new Question("Si tienes 15 manzanas y regalas 6, �cu�ntas te quedan?", new string[] { "7", "8", "9", "10" }, 2),
    new Question("�Cu�l es la ra�z cuadrada de 81?", new string[] { "7", "8", "9", "10" }, 2),
    new Question("Si un auto viaja a 60 km/h, �cu�nto recorrer� en 2 horas?", new string[] { "100 km", "120 km", "140 km", "160 km" }, 1),
    new Question("�Cu�nto es 3�?", new string[] { "6", "7", "8", "9" }, 3),
    new Question("�Cu�l es el resultado de 14 + 6?", new string[] { "18", "19", "20", "21" }, 2),
    new Question("Si un cuadrado tiene 4 lados, �cu�ntos lados tiene un oct�gono?", new string[] { "6", "7", "8", "9" }, 2),
    new Question("�Cu�nto es 5 � 5?", new string[] { "20", "25", "30", "35" }, 1),
    new Question("Si divido 36 entre 6, �cu�l es el resultado?", new string[] { "5", "6", "7", "8" }, 1),
    new Question("�Cu�nto es 50 - 23?", new string[] { "25", "27", "28", "29" }, 2),
    new Question("�Cu�l es la fracci�n equivalente a 0.5?", new string[] { "1/2", "1/3", "1/4", "2/3" }, 0),
    new Question("Si un reloj marca las 3:45, �cu�ntos minutos faltan para las 4:00?", new string[] { "10", "15", "20", "25" }, 1),
    new Question("Si un rect�ngulo tiene un largo de 10 cm y un ancho de 4 cm, �cu�l es su �rea?", new string[] { "30 cm�", "40 cm�", "50 cm�", "60 cm�" }, 1),
    new Question("Si el per�metro de un cuadrado es 16 cm, �cu�nto mide cada lado?", new string[] { "2 cm", "3 cm", "4 cm", "5 cm" }, 2),
    new Question("�Cu�nto es 2�?", new string[] { "6", "7", "8", "9" }, 2),
    new Question("Si tienes 2 docenas de huevos, �cu�ntos huevos tienes?", new string[] { "12", "24", "36", "48" }, 1),
    new Question("�Cu�l es el n�mero primo m�s peque�o?", new string[] { "0", "1", "2", "3" }, 2),
    new Question("Si un tri�ngulo tiene un �ngulo de 90�, �c�mo se llama?", new string[] { "Equil�tero", "Is�sceles", "Rect�ngulo", "Obtus�ngulo" }, 2),
    new Question("�Cu�nto es 0.25 en forma de fracci�n?", new string[] { "1/4", "1/3", "1/2", "3/4" }, 0),
};

    void Start()
    {
        if (questionCanvas == null)
        {
            questionCanvas = FindFirstObjectByType<Canvas>().gameObject;
        }
    }

    public void ShowRandomQuestion(Gem gem)
    {
        if (questions.Count > 0)
        {
            int randomIndex = Random.Range(0, questions.Count);
            selectedQuestion = questions[randomIndex]; // Almacenar la pregunta actual

            ShowQuestion(selectedQuestion.questionText, selectedQuestion.options, selectedQuestion.correctAnswer, gem);
        }
    }

    public void ShowQuestion(string question, string[] options, int correctIndex, Gem gem)
    {
        currentGem = gem;
        questionText.text = question;
        correctAnswerIndex = correctIndex;

        for (int i = 0; i < options.Length; i++)
        {
            optionTexts[i].text = options[i];
            int index = i;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }

        questionCanvas.SetActive(true);
    }


    public void CheckAnswer(int selectedAnswer)
    {
        if (selectedQuestion == null) // Verifica que la pregunta no sea nula
        {
            Debug.LogError("Error: selectedQuestion es nulo.");
            return;
        }

        if (selectedAnswer == selectedQuestion.correctAnswer)
        {
            Debug.Log("Respuesta correcta! Ocultando pregunta...");
            HideQuestion();
        }
        else
        {
            HideQuestion();
            feedbackManager.ShowFeedback("Respuesta incorrecta. �Int�ntalo de nuevo!");
        }
    }

    public void HideQuestion()
    {
        Debug.Log("HideQuestion() fue llamado.");
        questionCanvas.SetActive(false);
    }

}

// Clase para almacenar preguntas
[System.Serializable]
public class Question
{
    public string questionText;
    public string[] options;
    public int correctAnswer;

    public Question(string question, string[] options, int correctAnswer)
    {
        this.questionText = question;
        this.options = options;
        this.correctAnswer = correctAnswer;
    }
}


