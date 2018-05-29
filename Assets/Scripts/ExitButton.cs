using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour {
    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0))
            Application.Quit();
    }
}
