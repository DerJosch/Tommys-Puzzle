// using System;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour {
    public GameObject squarePrefab;
    public GameObject winSquarePrefab;
    public GameObject clickCountUI;

    public int rowCount = 4;
    public int columnCount = 4;

    private List<SquareController> squares;
    private GameObject squaresParent;
    private GameObject winSquaresParent;
    private Text clickCountText;

    private int clickCount;

    void Start() {
        clickCountText = clickCountUI.GetComponent<Text>();
        initField();
    }

    void Update() {
        if (Input.GetKey("space")) {
            winSquaresParent.SetActive(true);
        }
        else {
            winSquaresParent.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            restart();
        }
    }

    public void check() {
        clickCount++;
        clickCountText.text = "ClickCount: " + clickCount;
        foreach (SquareController square in squares) {
            if (square.currentState != square.winState) {
                return;
            }
        }

        Debug.Log("Win!");
    }
    
    private void restart() {
        Destroy(squaresParent);
        Destroy(winSquaresParent);
        initField();
    }

    private void initField() {
        clickCount = 0;
        clickCountText.text = "ClickCount: " + clickCount;
        squares = new List<SquareController>();
        squaresParent = new GameObject("squares");
        winSquaresParent = new GameObject("winSquares");
        winSquaresParent.SetActive(false);
        SquareController.State outerSquaresWinState =
            (SquareController.State)Random.Range(0, Enum.GetValues(typeof(SquareController.State)).Length);
        SquareController.State innerSquaresWinState =
            (SquareController.State)Random.Range(0, Enum.GetValues(typeof(SquareController.State)).Length);

        for (int column = 1; column <= columnCount; column++) {
            for (int row = 1; row <= rowCount; row++) {
                GameObject square = Instantiate(
                    squarePrefab,
                    new Vector3(row, column),
                    Quaternion.identity,
                    squaresParent.transform);
                square.name = "Square " + column + " - " + row;
                SquareController squareController = square.GetComponent<SquareController>();
                squares.Add(squareController);
                squareController.row = row;
                squareController.column = column;
                squareController.setState(SquareController.State.ONE);

                GameObject winSquare = Instantiate(
                    winSquarePrefab,
                    new Vector3(row, column),
                    Quaternion.identity,
                    winSquaresParent.transform);
                SpriteRenderer winStateSpriteRenderer = winSquare.GetComponent<SpriteRenderer>();
                if (squareController.row == 1 || squareController.column == 1 ||
                    squareController.row == 4 || squareController.column == 4) {
                    squareController.winState = outerSquaresWinState;
                    winStateSpriteRenderer.color = SquareController.getColor(outerSquaresWinState);
                }
                else {
                    squareController.winState = innerSquaresWinState;
                    winStateSpriteRenderer.color = SquareController.getColor(innerSquaresWinState);
                }
            }
        }

        foreach (SquareController squareController in squares) {
            foreach (SquareController otherSquareController in squares) {
                if (squareController.Equals(otherSquareController)) {
                    continue;
                }

                if (Mathf.Abs(otherSquareController.row - squareController.row) == 1 &&
                    otherSquareController.column == squareController.column) {
                    squareController.neighbourSquares.Add(otherSquareController);
                }

                if (Mathf.Abs(otherSquareController.column - squareController.column) == 1 &&
                    otherSquareController.row == squareController.row) {
                    squareController.neighbourSquares.Add(otherSquareController);
                }
            }
        }
    }
}