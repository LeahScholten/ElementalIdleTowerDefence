using System;
using UnityEngine;

public class TurretSpot : MonoBehaviour {
    protected SpriteRenderer spriteRenderer;
    protected Color color;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
    }

    public virtual void HoverOver(GameManager.BuildOption mode) {
        if (mode == GameManager.BuildOption.Nothing || mode == GameManager.BuildOption.Sell) {
            return;
        }
        spriteRenderer.color = Color.white;
    }

    public void EndHoverOver() {
        spriteRenderer.color = color;
    }
}