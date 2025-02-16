using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public QuestionManager questionManager;
    GameManager gm;
    float initialPosition;
    bool flag;
    private bool questionDisplayed = false;
    private void Start()
    {
        //gm = GameObject.FindObjectOfType<GameManager>();
        gm = GameObject.FindFirstObjectByType<GameManager>();
        //questionManager = GameObject.FindObjectOfType<QuestionManager>();

        if (questionManager == null)
        {
            questionManager = GameObject.FindFirstObjectByType<QuestionManager>();

            //Debug.LogError("No se encontró QuestionManager en la escena.");
        }
        flag = false;
        initialPosition = transform.position.y;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!questionDisplayed && gameObject.CompareTag(collision.gameObject.tag))
        {
            questionDisplayed = true;

            gm.gemscollected++;
            Destroy(gameObject);

            // Llamar a una pregunta específica
            ShowRandomQuestion();
        }
    }

    void ShowRandomQuestion()
    {
        if (questionManager != null)
        {
            questionManager.ShowRandomQuestion(this);
        }
        else
        {
            Debug.LogError("QuestionManager no está asignado en la gema.");
        }
    }

    public void CollectGem()
    {
        gm.gemscollected++;
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (initialPosition - transform.position.y <= 0.2f && flag == false)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.01f, transform.position.z);
        }
        else
        {
            flag = true;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
        }
        if (transform.position.y - initialPosition >= 0.2f && flag == true)
        {
            flag = false;
        }
    }
}
