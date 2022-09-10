using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Game : MonoBehaviour {

    public GameObject puzzleSquarePrefab;
    public float spacing = .1f;
    public int columns = 4;
    public int rows = 4;
    public int shuffleTimes = 20;
    public int shuffleDuration = 1;

    public Color[] colors = {
        new(205, 92, 92),
        new(223, 223, 128),
        new(104, 173, 86),
        new(88, 116, 152),
    };

    private readonly Random random = new();
    private GameObject[] field;
    private PuzzleSquare[] puzzleSquares;
    private Color[] fieldColors;
    private Color[] winPattern;
    private bool isWinPatternVisible;

    public void onTogglePattern() {
        if (!isWinPatternVisible) {
            fieldColors = puzzleSquares.Select(controller => controller.currentColor).ToArray();
        }

        isWinPatternVisible = !isWinPatternVisible;

        for (var i = 0; i < puzzleSquares.Length; i++) {
            puzzleSquares[i].setColor(isWinPatternVisible ? winPattern[i] : fieldColors[i]);
        }
    }

    private void Start() {
        field = new GameObject[columns * rows];
        puzzleSquares = new PuzzleSquare[columns * rows];
        winPattern = getWinPattern();

        var width = puzzleSquarePrefab.GetComponent<Renderer>().bounds.size.x;
        var height = puzzleSquarePrefab.GetComponent<Renderer>().bounds.size.y;
        var fieldWidth = width * columns + spacing * (columns - 1);
        var fieldHeight = height * rows + spacing * (rows - 1);

        for (var x = 0; x < columns; x++) {
            for (var y = 0; y < rows; y++) {
                var puzzleCellIndex = x + y * columns;

                var puzzleSquare = Instantiate(puzzleSquarePrefab, new Vector3(
                    -fieldWidth / 2 + width / 2 + x * width + x * spacing,
                    (-fieldHeight / 2 + height / 2 + y * height + y * spacing) * -1,
                    0
                ), Quaternion.identity);
                puzzleSquare.transform.Rotate(10, 0, 0);
                puzzleSquare.name = $"PuzzleCell x:{x}, y: {y}";
                field[puzzleCellIndex] = puzzleSquare;

                var squareController = puzzleSquare.GetComponent<PuzzleSquare>();
                squareController.column = x;
                squareController.row = y;
                squareController.setColor(winPattern[puzzleCellIndex], false);
                squareController.OnClicked += performMove;
                puzzleSquares[puzzleCellIndex] = squareController;
            }
        }

        for (var i = 0; i < shuffleTimes; i++) {
            var column = random.Next(columns);
            var row = random.Next(rows);

            StartCoroutine(queueMove(column, row, new WaitForSeconds((float)shuffleDuration / shuffleTimes * i)));
        }
    }

    private void performMove(int column, int row) {
        if (isWinPatternVisible) {
            return;
        }

        var cellIndex = row * columns + column;

        var top = cellIndex - columns;
        var bottom = cellIndex + columns;
        var left = cellIndex - 1;
        var right = cellIndex + 1;

        if (top >= 0) {
            var topCell = puzzleSquares[top];
            topCell.setColor(getNextColor(topCell.currentColor));
        }

        if (bottom < field.Length) {
            var bottomCell = puzzleSquares[bottom];
            bottomCell.setColor(getNextColor(bottomCell.currentColor));
        }

        if (left >= 0 && left % columns != columns - 1) {
            var leftCell = puzzleSquares[left];
            leftCell.setColor(getNextColor(leftCell.currentColor));
        }

        if (right < field.Length && right % columns != 0) {
            var rightCell = puzzleSquares[right];
            rightCell.setColor(getNextColor(rightCell.currentColor));
        }

        var clickedCell = puzzleSquares[cellIndex];
        clickedCell.setColor(getNextColor(clickedCell.currentColor));
    }

    private Color[] getWinPattern() {
        var borderColor = colors[random.Next(colors.Length - 1)];
        var innerColor = getNextColor(borderColor);

        var pattern = Enumerable.Repeat(borderColor, rows * columns).ToArray();
        for (var index = 0; index < pattern.Length; index++) {
            if (index > columns
                && index % columns != 0
                && (index - columns + 1) % columns != 0
                && (rows - 1) * columns > index) {
                pattern[index] = innerColor;
            }
        }

        return pattern;
    }

    private Color getNextColor(Color color) {
        return colors[(Array.IndexOf(colors, color) + 1) % colors.Length];
    }

    private IEnumerator queueMove(int column, int row, WaitForSeconds delay) {
        yield return delay;
        performMove(column, row);
    }

}
