using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using System.Collections.Generic;
using Unity.VisualScripting;


public class Preguntas : MonoBehaviour
{
    [SerializeField] private string createQuest = "http://127.0.0.1:13756/questions/create";
    private int contadorPreguntas = 0; // Contador para numerar las preguntas

    [SerializeField] private Transform listaPreguntasPanel; // Contenedor de las preguntas
    [SerializeField] private Transform eliminarButtonPanel;
    [SerializeField] private GameObject preguntaPrefab; // Prefab de cada botón de pregunta
    [SerializeField] private GameObject eliminarButtonPrefab;
    [SerializeField] private Transform agregarPreguntaButton;
    private GameObject eliminarButtonInstance;
    private List<Pregunta> listaPreguntas = new List<Pregunta>(); // Lista de preguntas ingresadas
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private TMP_InputField preguntaInputField;
    [SerializeField] private TMP_InputField opcion1InputField;
    [SerializeField] private TMP_InputField opcion2InputField;
    [SerializeField] private TMP_InputField opcion3InputField;
    [SerializeField] private TMP_InputField opcion4InputField;
    [SerializeField] private Toggle opcionACorrect;
    [SerializeField] private Toggle opcionBCorrect;
    [SerializeField] private Toggle opcionCCorrect;
    [SerializeField] private Toggle opcionDCorrect;
    [SerializeField] private Button createPreg;
    private GameObject botonEliminarActual;

    // Clase auxiliar para serializar la lista
    [System.Serializable]
    private class PreguntaLista
    {
        public List<Pregunta> preguntas;
    }

    public void CrearPregunta()
    {
        if (contadorPreguntas >= 24)
        {
            alertText.text = "Límite de preguntas alcanzado (24). No puedes agregar más preguntas.";
            Debug.LogWarning("Límite de preguntas alcanzado (24). No puedes agregar más preguntas.");
            ActivateButtons(false);
            return;
        }

        string nuevaPreguntaTexto = preguntaInputField.text;
        string opcion1Texto = opcion1InputField.text;
        string opcion2Texto = opcion2InputField.text;
        string opcion3Texto = opcion3InputField.text;
        string opcion4Texto = opcion4InputField.text;
        string respuestaCorrecta = "";

        if (opcionACorrect.isOn) respuestaCorrecta = "A";
        else if (opcionBCorrect.isOn) respuestaCorrecta = "B";
        else if (opcionCCorrect.isOn) respuestaCorrecta = "C";
        else if (opcionDCorrect.isOn) respuestaCorrecta = "D";

        // Verificar si la pregunta ya existe
        foreach (var pregunta in listaPreguntas)
        {
            if (pregunta.rQuestion == nuevaPreguntaTexto &&
                pregunta.rOptions[0].text == opcion1Texto &&
                pregunta.rOptions[1].text == opcion2Texto &&
                pregunta.rOptions[2].text == opcion3Texto &&
                pregunta.rOptions[3].text == opcion4Texto &&
                pregunta.rAnswer == respuestaCorrecta)
            {
                alertText.text = "Esta pregunta ya ha sido agregada.";
                Debug.LogWarning("Intento de agregar una pregunta duplicada.");
                return; // Detiene la función si es duplicada
            }
        }

        // Si la pregunta no es duplicada, la agrega
        alertText.text = "Enviando Pregunta...";
        Pregunta nuevaPregunta = new Pregunta
        {
            rQuestion = nuevaPreguntaTexto,
            rOptions = new List<rOpcion>
        {
            new rOpcion { option = "A", text = opcion1Texto },
            new rOpcion { option = "B", text = opcion2Texto },
            new rOpcion { option = "C", text = opcion3Texto },
            new rOpcion { option = "D", text = opcion4Texto }
        },
            rAnswer = respuestaCorrecta
        };

        // Convertir a JSON y enviarlo a la base de datos
        string jsonData = JsonUtility.ToJson(nuevaPregunta);
        //Debug.Log(jsonData);
        StartCoroutine(EnviarPregunta(jsonData));

    }


    IEnumerator EnviarPregunta(string jsonData)
    {
        using (UnityWebRequest request = new UnityWebRequest(createQuest, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Respuesta del servidor: " + request.downloadHandler.text); 
                CreateResponsePreg responsePreg = JsonUtility.FromJson<CreateResponsePreg>(request.downloadHandler.text);

                if (responsePreg.data == null || string.IsNullOrEmpty(responsePreg.data.id))
                {
                    Debug.LogWarning("Error: El servidor no devolvió un ID válido.");
                    yield break;
                }

                if (responsePreg.code == 0)
                {
                    contadorPreguntas++;
                    alertText.text = $" Pregunta {contadorPreguntas} creada con éxito.";
                    LimpiarFormulario();

                    Pregunta nuevaPregunta = JsonUtility.FromJson<Pregunta>(jsonData);
                    nuevaPregunta.id = responsePreg.data.id; // Guardar el ID de la base de datos
                    // Agregar la pregunta a la lista
                    Debug.Log($"Agregando pregunta a la lista. Total antes de agregar: {listaPreguntas.Count}");
                    listaPreguntas.Add(nuevaPregunta);
                    Debug.Log($"Pregunta agregada. Total después de agregar: {listaPreguntas.Count}");
                    //GuardarPreguntas();

                    // Crear un botón en el panel de preguntas
                    GameObject nuevoBoton = Instantiate(preguntaPrefab, listaPreguntasPanel);
                    nuevoBoton.GetComponentInChildren<TextMeshProUGUI>().text = $"Pregunta {contadorPreguntas}";
                    
                    // Agregar evento para llenar los campos al hacer clic
                    int index = listaPreguntas.Count-1;
                    nuevoBoton.GetComponent<Button>().onClick.AddListener(() => CargarPregunta(index));

                }
                else
                { 
                    switch (responsePreg.code)
                    {
                        case 1:
                            Debug.LogWarning("Esta pregunta ya ha sido ingresada en la base de datos.");
                            alertText.text = "Esta pregunta ya ha sido ingresada.";
                            break;
                        case 2:
                            Debug.LogWarning("La pregunta no puede estar vacía.");
                            alertText.text = "La pregunta no puede estar vacía.";
                            break;
                        case 3:
                            Debug.LogWarning("Debes llenar todos los campos.");
                            alertText.text = "Debes llenar todos los campos.";
                            break;
                        case 4:
                            Debug.LogWarning("Debes llenar todos los campos.");
                            alertText.text = "Debes llenar todos los campos.";
                            break;
                        case 5:
                            Debug.LogWarning("Debes seleccionar una respuesta correcta.");
                            alertText.text = "Debes seleccionar una respuesta correcta.";
                            break;
                        default:
                            alertText.text = "Corruption detected";
                            ActivateButtons(false);
                            break;
                    }

                }

            }
            else
            {
                alertText.text = "Error connecting to the server...";
                Debug.LogWarning("Error al enviar la pregunta: " + request.error);
            }
        }
        yield return null;
    }


    //void GuardarPreguntas()
    //{
    //    string json = JsonUtility.ToJson(new PreguntaLista { preguntas = listaPreguntas });
    //    PlayerPrefs.SetString("preguntasGuardadas", json);
    //    PlayerPrefs.Save();
    //}

    //void Start()
    //{
        

    //    CargarPreguntas();  // Cargar preguntas guardadas al iniciar
    //}

    //void CargarPreguntas()
    //{
    //    if (PlayerPrefs.HasKey("preguntasGuardadas"))
    //    {
    //        string json = PlayerPrefs.GetString("preguntasGuardadas");
    //        PreguntaLista datos = JsonUtility.FromJson<PreguntaLista>(json);
    //        listaPreguntas = datos.preguntas;

    //        // Eliminar preguntas previas antes de cargar nuevas
    //        foreach (Transform child in listaPreguntasPanel)
    //        {
    //            Destroy(child.gameObject);
    //        }

    //        // Restaurar botones en la UI
    //        foreach (var pregunta in listaPreguntas)
    //        {
    //            GameObject nuevoBoton = Instantiate(preguntaPrefab, listaPreguntasPanel);
    //            nuevoBoton.GetComponentInChildren<TextMeshProUGUI>().text = $"Pregunta {contadorPreguntas++}";

    //            int index = listaPreguntas.Count - 1;
    //            nuevoBoton.GetComponent<Button>().onClick.AddListener(() => CargarPregunta(index));

    //        }

    //        Debug.Log("Preguntas cargadas.");
    //    }
    //}

    private void CargarPregunta(int index)
    {
        if (index < 0 || index >= listaPreguntas.Count)
        {
            Debug.LogWarning($"Error: Índice fuera de rango ({index}). Lista de preguntas tiene {listaPreguntas.Count} elementos.");
            return;
        }

        Pregunta preguntaSeleccionada = listaPreguntas[index];
        Debug.Log($"Cargando pregunta {index + 1} con ID: {preguntaSeleccionada.id}");

        // Verifica si las opciones existen antes de acceder a ellas
        if (preguntaSeleccionada.rOptions == null || preguntaSeleccionada.rOptions.Count < 4)
        {
            Debug.LogError("Error: La pregunta seleccionada no tiene suficientes opciones.");
            return;
        }

        // Llenar los InputFields
        preguntaInputField.text = preguntaSeleccionada.rQuestion;
        opcion1InputField.text = preguntaSeleccionada.rOptions[0].text;
        opcion2InputField.text = preguntaSeleccionada.rOptions[1].text;
        opcion3InputField.text = preguntaSeleccionada.rOptions[2].text;
        opcion4InputField.text = preguntaSeleccionada.rOptions[3].text;

        // Seleccionar la respuesta correcta
        opcionACorrect.isOn = preguntaSeleccionada.rAnswer == "A";
        opcionBCorrect.isOn = preguntaSeleccionada.rAnswer == "B";
        opcionCCorrect.isOn = preguntaSeleccionada.rAnswer == "C";
        opcionDCorrect.isOn = preguntaSeleccionada.rAnswer == "D";
        MostrarBotonEliminar(index, preguntaSeleccionada.id);

        Debug.Log($"Cargada la pregunta {index + 1}");
    }

    private void MostrarBotonEliminar(int index, string preguntaId)
    {
        if (botonEliminarActual != null)
        {
            Destroy(botonEliminarActual);
        }
        botonEliminarActual = Instantiate(eliminarButtonPrefab, eliminarButtonPanel);
        botonEliminarActual.transform.SetParent(agregarPreguntaButton.parent, false);
        botonEliminarActual.transform.SetAsLastSibling();
        botonEliminarActual.GetComponent<Button>().onClick.AddListener(() => EliminarPregunta(index, preguntaId));
    }


    private void EliminarPregunta(int index, string preguntaId)
    {
        Debug.Log("EliminarPregunta llamado con index: " + index + ", id: " + preguntaId);

        if (string.IsNullOrEmpty(preguntaId))
        {
            Debug.LogError("ID de la pregunta es NULL o vacío. No se puede eliminar.");
            return;
        }

        StartCoroutine(EliminarPreguntaRequest(index, preguntaId));
    }

    IEnumerator EliminarPreguntaRequest(int index, string preguntaId)
    {
        Debug.Log("Intentando eliminar pregunta con ID: " + preguntaId);
        string url = $"http://127.0.0.1:13756/questions/delete/{preguntaId}";
        using (UnityWebRequest request = UnityWebRequest.Delete(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Pregunta {index + 1} eliminada correctamente.");

                // Eliminar la pregunta de la lista y de la UI
                Destroy(listaPreguntasPanel.GetChild(index).gameObject);
                listaPreguntas.RemoveAt(index);
                alertText.text = $"Pregunta {index + 1} eliminada correctamente.";
                LimpiarFormulario();
            }
            else
            {
                Debug.LogError("Error al eliminar la pregunta: " + request.error);
            }
        }
    }

    private void LimpiarFormulario()
    {
        preguntaInputField.text = "";
        opcion1InputField.text = "";
        opcion2InputField.text = "";
        opcion3InputField.text = "";
        opcion4InputField.text = "";
        opcionACorrect.isOn = false;
        opcionBCorrect.isOn = false;
        opcionCCorrect.isOn = false;
        opcionDCorrect.isOn = false;
    }


    private void ActivateButtons(bool toggle)
    {
        createPreg.interactable = toggle;
        opcionACorrect.interactable = toggle;
        opcionBCorrect.interactable = toggle;
        opcionCCorrect.interactable = toggle;
        opcionDCorrect.interactable = toggle;
    }

}
