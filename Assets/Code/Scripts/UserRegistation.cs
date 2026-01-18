using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserRegistation : MonoBehaviour
{
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button registerButton;
    [SerializeField] private TMP_Text feedbackText;

    private void Start() {
        if (registerButton != null)
        {
            registerButton.onClick.AddListener(OnRegisterClick);
        }
    }

    private void OnRegisterClick() {
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

        if (password.Length < 8)
        {
            ShowFeedback("Password must be at least 6 characters", Color.red);
            return;
        }

        // Check if user already exists
        if (DataBaseManager.instance.UserExists(username))
        {
            ShowFeedback("Username already exists. Please choose another.", Color.red);
            return;
        }

        // Register the user
        bool success = DataBaseManager.instance.RegisterUser(username, password);

        if (success)
        {
            ShowFeedback("Registration successful! You can now login.", Color.green);
            ClearInputs();

            // Optionally switch to login panel
            Invoke("SwitchToLogin", 1.5f);
        }
        else
        {
            ShowFeedback("Registration failed. Please try again.", Color.red);
        }
    }

    private void SwitchToLogin() {
        LayoutManager.instance.registerPanel.SetActive(false);
        LayoutManager.instance.loginPanel.SetActive(true);
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
