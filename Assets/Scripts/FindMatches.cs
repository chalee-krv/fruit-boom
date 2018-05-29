using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MatchedState {
    none,
    three,
    four,
    five
}

public class FindMatches : MonoBehaviour {
    private Board board;
    public List<GameObject> currMatches = new List<GameObject>();

    void Start() {
        board = FindObjectOfType<Board>();
    }

    public void findAllMatches() {
        StartCoroutine(findAllMatchesCo());
    }

    private IEnumerator findAllMatchesCo() {
        yield return new WaitForSeconds(.2f);

        for (int i = 0; i < board.width; i++) {
            for (int j = 0; j < board.height; j++) {
                GameObject currFruit = board.allFruits[i, j];

                if (currFruit) {
                    if (i > 0 && i < board.width - 1) {
                        GameObject left1 = board.allFruits[i - 1, j];
                        GameObject right1 = board.allFruits[i + 1, j];

                        if (left1 && right1 && left1.tag == currFruit.tag && right1.tag == currFruit.tag) {
                            MatchedState state = MatchedState.three;

                            if (i > 1 && i < board.width - 2) {
                                GameObject left2 = board.allFruits[i - 2, j];
                                GameObject right2 = board.allFruits[i + 2, j];

                                if (left2 && right2) {
                                    if (left2.tag == currFruit.tag && right2.tag == currFruit.tag) {
                                        state = MatchedState.five;

                                        if (i > 2 && i < board.width - 2) {
                                            for (int k = i + 3; k < board.width; k++) {
                                                GameObject nextRight = board.allFruits[k, j];

                                                if (nextRight && nextRight.tag == currFruit.tag) {
                                                    if (!currMatches.Contains(nextRight)) {
                                                        currMatches.Add(nextRight);
                                                    }

                                                    nextRight.GetComponent<Fruit>().setMatch(state);
                                                }
                                                else
                                                    break;
                                            }
                                        }

                                        if (!currMatches.Contains(left2)) {
                                            currMatches.Add(left2);
                                        }

                                        if (!currMatches.Contains(right2)) {
                                            currMatches.Add(right2);
                                        }

                                        left2.GetComponent<Fruit>().setMatch(state);
                                        right2.GetComponent<Fruit>().setMatch(state);
                                    }
                                    else if (right2.tag == currFruit.tag) {
                                        state = MatchedState.four;
                                        if (!currMatches.Contains(right2)) {
                                            currMatches.Add(right2);
                                        }

                                        right2.GetComponent<Fruit>().setMatch(state);
                                    }
                                }
                            }

                            if (state == MatchedState.five)
                                currFruit.tag = "bomb";
                            else if (state == MatchedState.four && !right1.CompareTag("bomb"))
                                right1.tag = "rainbow";

                            if (!currMatches.Contains(left1))
                                currMatches.Add(left1);
                            left1.GetComponent<Fruit>().setMatch(state);

                            if (!currMatches.Contains(currFruit))
                                currMatches.Add(currFruit);
                            currFruit.GetComponent<Fruit>().setMatch(state);

                            if (!currMatches.Contains(right1))
                                currMatches.Add(right1);
                            right1.GetComponent<Fruit>().setMatch(state);
                        }
                    }

                    if (j > 0 && j < board.height - 1) {
                        GameObject below1 = board.allFruits[i, j - 1];
                        GameObject above1 = board.allFruits[i, j + 1];

                        if (below1 && above1 && below1.tag == currFruit.tag && above1.tag == currFruit.tag) {
                            MatchedState state = MatchedState.three;

                            if (j > 1 && j < board.height - 2) {
                                GameObject below2 = board.allFruits[i, j - 2];
                                GameObject above2 = board.allFruits[i, j + 2];

                                if (below2 && above2) {
                                    if (below2.tag == currFruit.tag && above2.tag == currFruit.tag) {
                                        state = MatchedState.five;

                                        if (j > 2 && j < board.height - 2) {
                                            for (int k = j + 3; k < board.height; k++) {
                                                GameObject nextAbove = board.allFruits[i, k];

                                                if (nextAbove && nextAbove.tag == currFruit.tag) {
                                                    if (!currMatches.Contains(nextAbove)) {
                                                        currMatches.Add(nextAbove);
                                                    }

                                                    nextAbove.GetComponent<Fruit>().setMatch(state);
                                                }
                                                else
                                                    break;
                                            }
                                        }

                                        if (!currMatches.Contains(below2)) {
                                            currMatches.Add(below2);
                                        }

                                        if (!currMatches.Contains(above2)) {
                                            currMatches.Add(above2);
                                        }

                                        below2.GetComponent<Fruit>().setMatch(state);
                                        above2.GetComponent<Fruit>().setMatch(state);
                                    }
                                    else if (above2.tag == currFruit.tag) {
                                        state = MatchedState.four;
                                        if (!currMatches.Contains(above2)) {
                                            currMatches.Add(above2);
                                        }

                                        above2.GetComponent<Fruit>().setMatch(state);
                                    }
                                }
                            }

                            if (state == MatchedState.five)
                                currFruit.tag = "bomb";
                            else if (state == MatchedState.four && !above1.CompareTag("bomb"))
                                above1.tag = "rainbow";

                            if (!currMatches.Contains(below1))
                                currMatches.Add(below1);
                            below1.GetComponent<Fruit>().setMatch(state);

                            if (!currMatches.Contains(currFruit))
                                currMatches.Add(currFruit);
                            currFruit.GetComponent<Fruit>().setMatch(state);

                            if (!currMatches.Contains(above1))
                                currMatches.Add(above1);
                            above1.GetComponent<Fruit>().setMatch(state);
                        }
                    }
                }
            }
        }
    }

    public void hint() {
        for (int i = 0; i < board.width; i++) {
            for (int j = 0; j < board.height; j++) {
                GameObject currFruit = board.allFruits[i, j];

                if (currFruit) {
                    if (i > 0 && i < board.width - 1) {
                        GameObject right = board.allFruits[i + 1, j];

                        if (right) {
                            if (j < board.height - 1) {
                                GameObject leftT = board.allFruits[i - 1, j + 1];
                                if (leftT && leftT.tag == currFruit.tag && right.tag == currFruit.tag) {
                                    leftT.GetComponent<Fruit>().setHint();
                                    currFruit.GetComponent<Fruit>().setHint();
                                    right.GetComponent<Fruit>().setHint();
                                    return;
                                }
                            }

                            if (j > 0) {
                                GameObject leftB = board.allFruits[i - 1, j - 1];
                                if (leftB && leftB.tag == currFruit.tag && right.tag == currFruit.tag) {
                                    leftB.GetComponent<Fruit>().setHint();
                                    currFruit.GetComponent<Fruit>().setHint();
                                    right.GetComponent<Fruit>().setHint();
                                    return;
                                }
                            }
                        }

                        GameObject left = board.allFruits[i - 1, j];

                        if (left) {
                            if (j < board.height - 1) {
                                GameObject rightT = board.allFruits[i + 1, j + 1];

                                if (rightT && rightT.tag == currFruit.tag && left.tag == currFruit.tag) {
                                    rightT.GetComponent<Fruit>().setHint();
                                    currFruit.GetComponent<Fruit>().setHint();
                                    left.GetComponent<Fruit>().setHint();
                                    return;
                                }
                            }

                            if (j > 0) {
                                GameObject rightB = board.allFruits[i + 1, j - 1];

                                if (rightB && rightB.tag == currFruit.tag && left.tag == currFruit.tag) {
                                    rightB.GetComponent<Fruit>().setHint();
                                    currFruit.GetComponent<Fruit>().setHint();
                                    left.GetComponent<Fruit>().setHint();
                                    return;
                                }
                            }
                        }
                    }

                    if (j > 0 && j < board.height - 1) {
                        GameObject below = board.allFruits[i, j - 1];

                        if (below) {
                            if (i > 0) {
                                GameObject topL = board.allFruits[i - 1, j + 1];

                                if (topL && topL.tag == currFruit.tag && below.tag == currFruit.tag) {
                                    topL.GetComponent<Fruit>().setHint();
                                    currFruit.GetComponent<Fruit>().setHint();
                                    below.GetComponent<Fruit>().setHint();
                                    return;
                                }
                            }

                            if (i < board.width - 1) {
                                GameObject topR = board.allFruits[i + 1, j + 1];

                                if (topR && topR.tag == currFruit.tag && below.tag == currFruit.tag) {
                                    topR.GetComponent<Fruit>().setHint();
                                    currFruit.GetComponent<Fruit>().setHint();
                                    below.GetComponent<Fruit>().setHint();
                                    return;
                                }
                            }
                        }

                        GameObject above = board.allFruits[i, j + 1];

                        if (above) {
                            if (i > 0) {
                                GameObject bottomL = board.allFruits[i - 1, j - 1];

                                if (bottomL && bottomL.tag == currFruit.tag && above.tag == currFruit.tag) {
                                    bottomL.GetComponent<Fruit>().setHint();
                                    currFruit.GetComponent<Fruit>().setHint();
                                    above.GetComponent<Fruit>().setHint();
                                    return;
                                }
                            }

                            if (i < board.width - 1) {
                                GameObject bottomR = board.allFruits[i + 1, j - 1];

                                if (bottomR && bottomR.tag == currFruit.tag && above.tag == currFruit.tag) {
                                    bottomR.GetComponent<Fruit>().setHint();
                                    currFruit.GetComponent<Fruit>().setHint();
                                    above.GetComponent<Fruit>().setHint();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        Debug.Log("Nothing");
    }
}