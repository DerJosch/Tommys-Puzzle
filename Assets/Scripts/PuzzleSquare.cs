using UnityEngine;

public class PuzzleSquare : MonoBehaviour {

    public delegate void ClickAction(int column, int row);

    public event ClickAction OnClicked;
    public int column;
    public int row;
    public float transitionDuration = .5f;

    private Material material;

    public Color currentColor { get; private set; }
    private Color previousColor;
    private float transitionTime;

    public void setColor(Color color, bool withTransition = true) {
        currentColor = color;
        if (withTransition) {
            transitionTime = 0;
        }
    }

    private void Start() {
        material = GetComponent<MeshRenderer>().material;
        transitionTime = transitionDuration;
    }

    private void Update() {
        if (transitionTime == 0) {
            previousColor = material.color;
        }

        if (transitionTime < transitionDuration) {
            transitionTime += Time.deltaTime;
        }

        if (transitionTime > transitionDuration) {
            transitionTime = transitionDuration;
        }

        material.color = Color.Lerp(previousColor, currentColor, transitionTime / transitionDuration);
    }

    private void OnMouseDown() {
        OnClicked?.Invoke(column, row);
    }

}
