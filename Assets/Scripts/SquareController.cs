using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SquareController : MonoBehaviour {
    
    public int row = 0;
    public int column = 0;
    public State currentState = State.ONE;
    
    public SpriteRenderer spriteRenderer;

    public List<SquareController> neighbourSquares = new List<SquareController>();

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown() {
        setState(nextState(currentState));
        foreach (SquareController neighbourSquare in neighbourSquares) {
            neighbourSquare.setState(nextState(neighbourSquare.currentState));
        }
    }

    public void setState(State state) {
        currentState = state;

        switch (state) {
            case State.ONE: spriteRenderer.color = Color.red; break;
            case State.TWO: spriteRenderer.color = Color.green; break;
            case State.THREE: spriteRenderer.color = Color.blue; break;
            case State.FOUR: spriteRenderer.color = Color.yellow; break;
        }
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
}
