using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginRegistration : MonoBehaviour {
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_Text feedbackText;

    private void Start() {
        if (loginButton != null)
        {
            loginButton.onClick.AddListener(OnLoginClick);
        }
    }

    private void OnLoginClick() {
        string username = userInput.text.Trim();
        string password = passwordInput.text;

        // Validate inputs
        if (string.IsNullOrEmpty(username))
        {
            ShowFeedback("Username cannot be empty", Color.red);
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            ShowFeedback("Password cannot be empty", Color.red);
            return;
        }

        // Attempt login
        bool success = DataBaseManager.instance.LoginUser(username, password);

        if (success)
        {
            ShowFeedback("Login successful! Welcome " + username, Color.green);
            ClearInputs();

            // Handle successful login - load game scene, hide panels, etc.
            Invoke("OnSuccessfulLogin", 1f);
        }
        else
        {
            ShowFeedback("Invalid username or password", Color.red);
        }
    }

    private void OnSuccessfulLogin() {
        // Hide both panels
        LayoutManager.instance.loginPanel.SetActive(false);
        LayoutManager.instance.registerPanel.SetActive(false);

        // You can load your game scene here or enable game objects
        SceneManager.LoadScene("GameScene");
    }

    private void ShowFeedback(string message, Color color) {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = color;
        }
        Debug.Log(message);
    }

    private void ClearInputs() {
        userInput.text = "";
        passwordInput.text = "";
    }
}