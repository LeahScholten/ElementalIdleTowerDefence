using System;
using UnityEngine;

public class TurretSpot : MonoBehaviour {
    SpriteRenderer spriteRenderer;
    Color color;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
    }

    public void HoverOver() {
        spriteRenderer.color = Color.red;
    }

    public void EndHoverOver() {
        spriteRenderer.color = color;
    }
}
