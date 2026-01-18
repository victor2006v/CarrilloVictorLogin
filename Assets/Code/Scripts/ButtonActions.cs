using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    [SerializeField] private Button registerButton;
    [SerializeField] private Button loginButton;

    private void OnEnable() {
        if (registerButton != null) registerButton.onClick.AddListener(OnRegister);
        if (loginButton != null) loginButton.onClick.AddListener(OnLogin);
    }
    private void OnDisable() {
        if (registerButton != null) registerButton.onClick.RemoveListener(OnRegister);
        if (loginButton != null) loginButton.onClick.RemoveListener(OnLogin);
    }

    private void OnLogin() {
        LayoutManager.instance.loginPanel.SetActive(true);
        LayoutManager.instance.registerPanel.SetActive(false);
    }
    private void OnRegister() { 
        LayoutManager.instance.registerPanel.SetActive(true);
        LayoutManager.instance.loginPanel.SetActive(false);
    }
}
