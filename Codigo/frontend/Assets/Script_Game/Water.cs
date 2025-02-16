using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public GameObject deathPanel; // Panel de muerte (arrastrar el DeathPanel desde la jerarquía)

    private void Start()
    {
        deathPanel.SetActive(false); // Asegúrate de que el Panel esté desactivado al inicio
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el jugador toca el agua del color contrario
        if (!gameObject.CompareTag(collision.gameObject.tag))
        {
            Debug.Log("Death"); // Confirmación en la consola
            TriggerDeath(); // Llama a la función para activar el panel
        }
    }

    private void TriggerDeath()
    {
        Time.timeScale = 0f; // Pausa el juego
        deathPanel.SetActive(true); // Activa el Panel de muerte
    }
}


