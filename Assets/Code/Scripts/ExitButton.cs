using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private Button exitButton;

    private void Start() {
        exitButton = GetComponent<Button>();
        if(exitButton != null )
        exitButton.onClick.AddListener(Exit);
    }

    private void Exit() {
        Application.Quit();
        Debug.Log("Exit!");
    }
}

