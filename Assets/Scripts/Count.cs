using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Count : MonoBehaviour {
    public static int value = 45;
    TextMesh scoreText;

    // Use this for initialization
    void Start() {
        scoreText = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update() {
        scoreText.text = value.ToString();
        if (value <= 0) {
            FindObjectOfType<Board>().currState = GameState.wait;
        }
    }
}