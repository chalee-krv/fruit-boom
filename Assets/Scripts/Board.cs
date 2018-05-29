using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    wait,
    move
}

public class Board : MonoBehaviour {
    public int width;
    public int height;
    public int offSet;

    public GameObject[] fruits;
    public GameObject[,] allFruits;

    public GameState currState = GameState.move;
    private FindMatches findMatches;

    void Start() {
        allFruits = new GameObject[width, height];
        findMatches = FindObjectOfType<FindMatches>();
        init();
    }

    void init() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Vector2 tempPos = new Vector2(x, y + offSet);
                int fruitToUse = Random.Range(0, fruits.Length - 2);

                for (int i = 0; i < 100; i++) {
                    fruitToUse = Random.Range(0, fruits.Length - 2);
                    if (!matchesAt(x, y, fruits[fruitToUse])) {
                        break;
                    }
                }

                GameObject fruit = Instantiate(fruits[fruitToUse], tempPos, Quaternion.identity);
                fruit.GetComponent<Fruit>().row = y;
                fruit.GetComponent<Fruit>().col = x;
                fruit.transform.parent = this.transform;
                fruit.name = "(" + x + ", " + y + ")";
                allFruits[x, y] = fruit;
            }
        }
    }

    private bool matchesAt(int col, int row, GameObject piece) {
        if (col > 1 && row > 1) {
            if (allFruits[col - 1, row].tag == piece.tag && allFruits[col - 2, row].tag == piece.tag) {
                return true;
            }

            if (allFruits[col, row - 1].tag == piece.tag && allFruits[col, row - 2].tag == piece.tag) {
                return true;
            }
        }
        else if (col <= 1 || row <= 1) {
            if (row > 1 && allFruits[col, row - 1].tag == piece.tag && allFruits[col, row - 2].tag == piece.tag) {
                return true;
            }

            if (col > 1 && allFruits[col - 1, row].tag == piece.tag && allFruits[col - 2, row].tag == piece.tag) {
                return true;
            }
        }

        return false;
    }

    private void addSpecialFruit(int col, int row, int fruitToUse) {
        findMatches.currMatches.Remove(allFruits[col, row]);
        Destroy(allFruits[col, row]);

        Vector2 v = new Vector2(col, row);
        GameObject fruit = Instantiate(fruits[fruitToUse], v, Quaternion.identity);
        fruit.GetComponent<Fruit>().row = row;
        fruit.GetComponent<Fruit>().col = col;
        fruit.transform.parent = this.transform;
        allFruits[col, row] = fruit;
    }

    public void bombMatches() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allFruits[i, j]) {
                    if (allFruits[i, j].CompareTag("bomb")) {
                        Score.value += 5000;
                        addSpecialFruit(i, j, fruits.Length - 1);
                    }
                    else if (allFruits[i, j].CompareTag("rainbow")) {
                        Score.value += 2500;
                        addSpecialFruit(i, j, fruits.Length - 2);
                    }
                    else {
                        bombMatchedAt(i, j);
                    }
                }
            }
        }

        StartCoroutine(decreaseRowCo());
    }

    private void bombMatchedAt(int col, int row) {
        if (allFruits[col, row].GetComponent<Fruit>().matchedState != MatchedState.none) {
            findMatches.currMatches.Remove(allFruits[col, row]);
            Score.value += 100;
            Destroy(allFruits[col, row]);
            allFruits[col, row] = null;
        }
    }

    public void bombAll() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allFruits[i, j]) {
                    Score.value += 100;
                    Destroy(allFruits[i, j]);
                    allFruits[i, j] = null;
                }
            }
        }

        StartCoroutine(decreaseRowCo());
    }

    public void bombRainbow(int at, string dir) {
        for (int i = 0; i < (dir == "row" ? height : width); i++) {
            if (dir == "row" && allFruits[i, at]) {
                Score.value += 100;
                Destroy(allFruits[i, at]);
                allFruits[i, at] = null;
            }
            else if (dir == "col" && allFruits[at, i]) {
                Score.value += 100;
                Destroy(allFruits[at, i]);
                allFruits[at, i] = null;
            }
        }

        StartCoroutine(decreaseRowCo());
    }

    private IEnumerator decreaseRowCo() {
        int nullCount = 0;
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (!allFruits[i, j]) {
                    nullCount++;
                }
                else if (nullCount > 0) {
                    allFruits[i, j].GetComponent<Fruit>().row -= nullCount;
                    allFruits[i, j] = null;
                }
            }

            nullCount = 0;
        }

        yield return new WaitForSeconds(.4f);
        StartCoroutine(fillBoardCo());
    }

    private void fillBoard() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (!allFruits[i, j]) {
                    Vector2 tempPos = new Vector2(i, j + offSet);
                    int fruitToUse = Random.Range(0, fruits.Length - 2);
                    GameObject piece = Instantiate(fruits[fruitToUse], tempPos, Quaternion.identity);
                    allFruits[i, j] = piece;
                    piece.GetComponent<Fruit>().row = j;
                    piece.GetComponent<Fruit>().col = i;
                }
            }
        }
    }

    private bool matchesOnBoard() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allFruits[i, j] && allFruits[i, j].GetComponent<Fruit>()) {
                    if (allFruits[i, j].GetComponent<Fruit>().matchedState != MatchedState.none) {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private IEnumerator fillBoardCo() {
        fillBoard();
        yield return new WaitForSeconds(.5f);

        while (matchesOnBoard()) {
            yield return new WaitForSeconds(.5f);
            bombMatches();
        }

        yield return new WaitForSeconds(.5f);
        currState = GameState.move;
    }

    public void restart() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allFruits[i, j]) {
                    Destroy(allFruits[i, j]);
                    allFruits[i, j] = null;
                }
            }
        }

        init();
    }
}