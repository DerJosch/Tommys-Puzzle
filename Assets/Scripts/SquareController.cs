using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareController : MonoBehaviour {

    private SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown() {
        Debug.Log("Mouse down");
        spriteRenderer.color = new Color(Random.Range(0f, 1f),Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}
