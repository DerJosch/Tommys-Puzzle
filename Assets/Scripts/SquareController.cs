using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareController : MonoBehaviour {
    
    public SpriteRenderer spriteRenderer;
    
    public int row;
    public int column;
    public State currentState = State.ONE;
    public State winState = State.ONE;
    
    public List<SquareController> neighbourSquares = new List<SquareController>();

    private GameController gameController;

    void Start() {
        gameController = FindObjectOfType<GameController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown() {
        setState(nextState(currentState));
        foreach (SquareController neighbourSquare in neighbourSquares) {
            neighbourSquare.setState(nextState(neighbourSquare.currentState));
        }

        gameController.check();
    }

    public void setState(State state) {
        currentState = state;
        spriteRenderer.color = getColor(state);
    }

    private State nextState(State state) {
        switch (state) {
            case State.ONE: return State.TWO;
            case State.TWO: return State.THREE;
            case State.THREE: return State.FOUR;
            case State.FOUR: return State.ONE;
            default: throw new NotSupportedException("State " + state + " is unsupported.");
        }
    }

    public enum State {
        ONE,    // red
        TWO,    // green
        THREE,  // blue
        FOUR    // yellow
    }
    
    public static Color getColor(State state) {
        switch (state) {
            case State.ONE: return new Color(1f, 0.5f, 0.5f);
            case State.TWO: return new Color(0.5f, 1f, 0.5f);
            case State.THREE: return new Color(0.5f, 0.5f, 1f);
            case State.FOUR: return new Color(1f, 1f, 0.5f);
            default: throw new NotSupportedException("State " + state + " is unsupported.");
        }
    }
}
