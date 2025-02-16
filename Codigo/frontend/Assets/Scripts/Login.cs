using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class Login : MonoBehaviour
{
    [SerializeField] private string loginEndpoint = "http://127.0.0.1:13756/account/login";
    [SerializeField] private string createEndpoint = "http://127.0.0.1:13756/account/create";

    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button CreateButton;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;

    public void onLoginClick()
    {
        alertText.text = "Iniciando sesion...";
        ActivateButtons(false);

        StartCoroutine(TryLogin());
    }

    public void onCreateClick()
    {
        alertText.text = "Creando cuenta...";
        ActivateButtons(false);

        StartCoroutine(TryCreate());
    }

    private IEnumerator TryLogin()
    {

        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (username.Length < 3 || username.Length > 24)
        {
            alertText.text = "Usuario invalido";
            ActivateButtons(true);
            yield break;
        }

        if (password.Length < 3 || password.Length > 24)
        {
            alertText.text = "Contraseña invalida";
            ActivateButtons(true);
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("rUsername", username);
        form.AddField("rPassword", password);

        UnityWebRequest request = UnityWebRequest.Post(loginEndpoint, form);
        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;

            if (startTime > 10.0f)
            {
                break;
            }

            yield return null;
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

            if (response.code == 0) //login succes?
            {
                alertText.text = string.Empty;
                ActivateButtons(false);
                //ProfAccount returnedAccount = JsonUtility.FromJson<ProfAccount>(request.downloadHandler.text);
                alertText.text = "Welcome " + ((response.data.adminFlag == 1) ? " Admin" : "");
                SceneManager.LoadScene("ProfPreg");

            }
            else
            {
                switch (response.code)
                {
                    case 1:
                        alertText.text = "Invalid Credentials...";
                        ActivateButtons(true);
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
            ActivateButtons(true);
        }

        yield return null;
    }

    private IEnumerator TryCreate()
    {

        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (username.Length < 3 || username.Length > 24)
        {
            alertText.text = "Usuario invalido";
            ActivateButtons(true);
            yield break;
        }

        if (password.Length < 3 || password.Length > 24)
        {
            alertText.text = "Contraseña invalida";
            ActivateButtons(true);
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("rUsername", username);
        form.AddField("rPassword", password);

        UnityWebRequest request = UnityWebRequest.Post(createEndpoint, form);
        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;

            if (startTime > 10.0f)
            {
                break;
            }

            yield return null;
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            CreateResponse response = JsonUtility.FromJson<CreateResponse>(request.downloadHandler.text);

            if (response.code == 0 ) //signup succes?
            {
                alertText.text = "Se ha creado la cuenta";
                ActivateButtons(true);

            }
            else
            {
                switch (response.code)
                {
                    case 1:
                        alertText.text = "Invalid Credentials";
                        break;
                    case 2:
                        alertText.text = "Username is already taken";
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
        }
        ActivateButtons(true);

        yield return null;
    }


    private void ActivateButtons(bool toggle)
    {
        loginButton.interactable = toggle;
        CreateButton.interactable = toggle;
    }
}
