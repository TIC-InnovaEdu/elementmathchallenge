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
    private Gem currentGem; // Referencia a la gema que activó la pregunta
    private Question selectedQuestion; // Nueva variable para almacenar la pregunta actual

    // Lista de preguntas predefinidas
    private List<Question> questions = new List<Question> {
    new Question("¿Cuánto es 7 + 5?", new string[] { "10", "11", "12", "13" }, 2),
    new Question("¿Cuál es el resultado de 9 - 4?", new string[] { "4", "5", "6", "7" }, 1),
    new Question("¿Cuánto es 6 × 3?", new string[] { "12", "15", "18", "20" }, 2),
    new Question("¿Cuál es el resultado de 20 ÷ 4?", new string[] { "4", "5", "6", "7" }, 1),
    new Question("Si un triángulo tiene tres lados, ¿cuántos lados tiene un pentágono?", new string[] { "4", "5", "6", "7" }, 1),
    new Question("¿Cuánto es 8 × 7?", new string[] { "48", "54", "56", "63" }, 2),
    new Question("Si tienes 15 manzanas y regalas 6, ¿cuántas te quedan?", new string[] { "7", "8", "9", "10" }, 2),
    new Question("¿Cuál es la raíz cuadrada de 81?", new string[] { "7", "8", "9", "10" }, 2),
    new Question("Si un auto viaja a 60 km/h, ¿cuánto recorrerá en 2 horas?", new string[] { "100 km", "120 km", "140 km", "160 km" }, 1),
    new Question("¿Cuánto es 3²?", new string[] { "6", "7", "8", "9" }, 3),
    new Question("¿Cuál es el resultado de 14 + 6?", new string[] { "18", "19", "20", "21" }, 2),
    new Question("Si un cuadrado tiene 4 lados, ¿cuántos lados tiene un octágono?", new string[] { "6", "7", "8", "9" }, 2),
    new Question("¿Cuánto es 5 × 5?", new string[] { "20", "25", "30", "35" }, 1),
    new Question("Si divido 36 entre 6, ¿cuál es el resultado?", new string[] { "5", "6", "7", "8" }, 1),
    new Question("¿Cuánto es 50 - 23?", new string[] { "25", "27", "28", "29" }, 2),
    new Question("¿Cuál es la fracción equivalente a 0.5?", new string[] { "1/2", "1/3", "1/4", "2/3" }, 0),
    new Question("Si un reloj marca las 3:45, ¿cuántos minutos faltan para las 4:00?", new string[] { "10", "15", "20", "25" }, 1),
    new Question("Si un rectángulo tiene un largo de 10 cm y un ancho de 4 cm, ¿cuál es su área?", new string[] { "30 cm²", "40 cm²", "50 cm²", "60 cm²" }, 1),
    new Question("Si el perímetro de un cuadrado es 16 cm, ¿cuánto mide cada lado?", new string[] { "2 cm", "3 cm", "4 cm", "5 cm" }, 2),
    new Question("¿Cuánto es 2³?", new string[] { "6", "7", "8", "9" }, 2),
    new Question("Si tienes 2 docenas de huevos, ¿cuántos huevos tienes?", new string[] { "12", "24", "36", "48" }, 1),
    new Question("¿Cuál es el número primo más pequeño?", new string[] { "0", "1", "2", "3" }, 2),
    new Question("Si un triángulo tiene un ángulo de 90°, ¿cómo se llama?", new string[] { "Equilátero", "Isósceles", "Rectángulo", "Obtusángulo" }, 2),
    new Question("¿Cuánto es 0.25 en forma de fracción?", new string[] { "1/4", "1/3", "1/2", "3/4" }, 0),
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
            feedbackManager.ShowFeedback("Respuesta incorrecta. ¡Inténtalo de nuevo!");
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


