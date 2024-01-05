using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField EmailInputField;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TextMeshProUGUI loginRegisterText;
    [SerializeField] private Toggle showPasswordToggle;
    [SerializeField] private GameObject LoginRegisterUI;
    //Here we check if our Playfab title id is the same as the one of our project
    public void Start()
    {
        loginRegisterText.gameObject.SetActive(false);
        LoginRegisterUI.SetActive(true);
        passwordInputField.contentType = TMP_InputField.ContentType.Password;
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
           
            PlayFabSettings.staticSettings.TitleId = "48C80";
            Debug.Log("Playfab Project ID is Correct!");
        }
        

    }
    //here we check if the toggle is on or off and change the password input field content type accordindly
    private void Update()
    {
        if (showPasswordToggle.isOn)
        {
            passwordInputField.contentType = TMP_InputField.ContentType.Standard;
        }
        else
        {
            passwordInputField.contentType = TMP_InputField.ContentType.Password;
        }
        passwordInputField.ForceLabelUpdate();
    }
    #region PlayFab
    //We register the user function
    public void RegisterUser()
    {
        //playfab doesn't allow password length to be less than 6 
        if (passwordInputField.text.Length < 6)
        {
            loginRegisterText.text = "Password too short";
            loginRegisterText.gameObject.SetActive(true);
            return;
        }
        //we register a new user based on their email,username and password
        var request = new RegisterPlayFabUserRequest
        {
            Email = EmailInputField.text,
            Username = usernameInputField.text,
            Password = passwordInputField.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess,OnRegisterFailure);
    }
    //login user function
    //we login the user if the username and the password match
    public void LoginUser()
    {
        var request = new LoginWithPlayFabRequest {
          
            Username = usernameInputField.text,
            Password = passwordInputField.text,
        };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
    }
    //if we have succesfuly registered we activate the text and put the text to registered new user
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        loginRegisterText.gameObject.SetActive(true);
        LoginRegisterUI.SetActive(false);
        loginRegisterText.text = "Registered New User " + usernameInputField.text;
        Debug.Log("Registered new user");
    }

//if we have succesfuly logged in we activate the text and put the text to registered new user 
private void OnLoginSuccess(LoginResult result)
    {

        loginRegisterText.gameObject.SetActive(true);
        LoginRegisterUI.SetActive(false);
        loginRegisterText.text = "User Logged in" + usernameInputField.text;
        Debug.Log("User Logged in");
    }
    //function that handles if we haven't registered correctly
    private void OnRegisterFailure(PlayFabError error)
    {
        loginRegisterText.gameObject.SetActive(true);
        Debug.LogWarning("Something went wrong with the register method");
        Debug.LogError("Here's some debug information:");
        loginRegisterText.text = error.ErrorMessage;
        Debug.LogError(error.GenerateErrorReport());
    }
    //function that handles if we haven't logged in correctly
    private void OnLoginFailure(PlayFabError error)
    {
        loginRegisterText.gameObject.SetActive(true);
        loginRegisterText.text = error.ErrorMessage;
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
    #endregion PlayFab
}