using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Restart : MonoBehaviour {
    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            FindObjectOfType<Board>().restart();
            Score.value = 0;
            Count.value = 45;
        }
    }
}