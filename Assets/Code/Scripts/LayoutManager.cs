using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutManager : MonoBehaviour
{
    public static LayoutManager instance { get; private set; }
    public GameObject loginPanel;
    public GameObject registerPanel;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    private void Start() {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
    }


}
