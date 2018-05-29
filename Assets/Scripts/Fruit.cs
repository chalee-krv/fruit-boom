using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour {
    public int col;
    public int row;
    public int prevCol;
    public int prevRow;
    public string dir;
    public MatchedState matchedState = MatchedState.none;

    private Board board;
    private GameObject anotherFruit;
    private FindMatches findMatches;
    private Vector2 startPos;
    private Vector2 stopPos;
    private Vector2 tempPos;
    public float swipeAngle;

    private readonly float TOLERANCE = .0000001f;

    void Start() {
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
    }

    void Update() {
        if (Mathf.Abs(col - transform.position.x) > .1) {
            tempPos = new Vector2(col, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, .6f);
            if (board.allFruits[col, row] != this.gameObject) {
                board.allFruits[col, row] = this.gameObject;
            }

            findMatches.findAllMatches();
        }
        else {
            tempPos = new Vector2(col, transform.position.y);
            transform.position = tempPos;
        }

        if (Mathf.Abs(row - transform.position.y) > .1) {
            tempPos = new Vector2(transform.position.x, row);
            transform.position = Vector2.Lerp(transform.position, tempPos, .6f);
            if (board.allFruits[col, row] != this.gameObject) {
                board.allFruits[col, row] = this.gameObject;
            }

            findMatches.findAllMatches();
        }
        else {
            tempPos = new Vector2(transform.position.x, row);
            transform.position = tempPos;
        }
    }

    public IEnumerator checkMoveCo() {
        yield return new WaitForSeconds(.5f);

        if (anotherFruit != null) {
            if (gameObject.CompareTag("bomb") || anotherFruit.CompareTag("bomb")) {
                Score.value += 4500;
                Count.value--;
                board.bombAll();
            }
            else if (gameObject.CompareTag("rainbow") || anotherFruit.CompareTag("rainbow")) {
                Score.value += 2000;
                Count.value--;

                if (gameObject.CompareTag("rainbow")) {
                    if (dir == "row")
                        board.bombRainbow(col, "col");
                    else if (dir == "col")
                        board.bombRainbow(row, "row");
                }
                else {
                    if (dir == "row")
                        board.bombRainbow(anotherFruit.GetComponent<Fruit>().col, "col");
                    else if (dir == "col")
                        board.bombRainbow(anotherFruit.GetComponent<Fruit>().row, "row");
                }
            }
            else if (matchedState == MatchedState.none &&
                     anotherFruit.GetComponent<Fruit>().matchedState == MatchedState.none) {
                anotherFruit.GetComponent<Fruit>().row = row;
                anotherFruit.GetComponent<Fruit>().col = col;
                row = prevRow;
                col = prevCol;
                yield return new WaitForSeconds(.5f);
                board.currState = GameState.move;
            }
            else {
                Count.value--;
                board.bombMatches();
            }

            anotherFruit = null;
        }
    }

    private void OnMouseDown() {
        if (board.currState == GameState.move) {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp() {
        if (board.currState == GameState.move) {
            stopPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Math.Abs(startPos.x - stopPos.x) > TOLERANCE && Math.Abs(startPos.y - stopPos.y) > TOLERANCE) {
                swipeAngle = Mathf.Atan2(stopPos.y - startPos.y, stopPos.x - startPos.x) * 180 / Mathf.PI;

                if (swipeAngle > -45 && swipeAngle <= 45 && col < board.width - 1) { //r
                    anotherFruit = board.allFruits[col + 1, row];
                    prevRow = row;
                    prevCol = col;
                    dir = "row";
                    --anotherFruit.GetComponent<Fruit>().col;
                    ++col;
                }
                else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1) { //u
                    anotherFruit = board.allFruits[col, row + 1];
                    prevRow = row;
                    prevCol = col;
                    dir = "col";
                    --anotherFruit.GetComponent<Fruit>().row;
                    ++row;
                }
                else if ((swipeAngle > 135 || swipeAngle <= -135) && col > 0) { //l
                    anotherFruit = board.allFruits[col - 1, row];
                    prevRow = row;
                    prevCol = col;
                    dir = "row";
                    ++anotherFruit.GetComponent<Fruit>().col;
                    --col;
                }
                else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0) { //d
                    anotherFruit = board.allFruits[col, row - 1];
                    prevRow = row;
                    prevCol = col;
                    dir = "col";
                    ++anotherFruit.GetComponent<Fruit>().row;
                    --row;
                }

                StartCoroutine(checkMoveCo());
                board.currState = GameState.wait;
            }
            else {
                board.currState = GameState.move;
            }
        }
    }

    public void setMatch(MatchedState state) {
        setHint();
        matchedState = state;
    }

    public void setHint() {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(1f, 1f, 1f, .2f);
    }
}