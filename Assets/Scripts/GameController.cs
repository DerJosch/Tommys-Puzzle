using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject squarePrefab;

    public int rowCount = 4;
    public int columnCount = 4;

    public List<SquareController> squares = new List<SquareController>();

    void Start() {
        initField();
    }

    private void initField() {

        for (int column = 1; column <= columnCount; column++) {
            for (int row = 1; row <= rowCount; row++) {
                GameObject square = Instantiate(squarePrefab, new Vector3(row, column), Quaternion.identity);
                square.name = "Square " + column + " - " + row;
                SquareController squareController = square.GetComponent<SquareController>();
                squares.Add(squareController);
                squareController.row = row;
                squareController.column = column;
                squareController.setState(SquareController.State.ONE);
            }
        }
        
        foreach(SquareController squareControllerToCalculate in squares) {
            foreach (SquareController squareController in squares) {
                if (squareControllerToCalculate.Equals(squareController)) {
                    continue;
                }
                if(Mathf.Abs(squareController.row - squareControllerToCalculate.row) == 1 &&
                   squareController.column == squareControllerToCalculate.column) {
                    squareControllerToCalculate.neighbourSquares.Add(squareController);
                }
                if(Mathf.Abs(squareController.column - squareControllerToCalculate.column) == 1 &&
                   squareController.row == squareControllerToCalculate.row) {
                    squareControllerToCalculate.neighbourSquares.Add(squareController);
                }
            }
        }
    }
}
