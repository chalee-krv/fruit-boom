using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour {
    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0) && FindObjectOfType<Board>().currState == GameState.move) {
            Count.value--;
            FindObjectOfType<FindMatches>().hint();
        }
    }
}