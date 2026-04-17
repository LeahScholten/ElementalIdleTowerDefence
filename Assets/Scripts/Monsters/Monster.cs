using System;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    GameObject[] trackPoints;
    private int pointIndex = 0;
    [SerializeField] protected float speed;
    [SerializeField] protected int value;
    [SerializeField] protected int maxHealth = 1;
    [SerializeField] private GameObject healthBar;
    private int health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        trackPoints = GameObject.FindGameObjectsWithTag("Track");
        Array.Sort(trackPoints, (x, y) => x.name.CompareTo(y.name));
    }

    // Update is called once per frame
    void Update()
    {
        GameObject point = trackPoints[pointIndex];
        Vector2 difference = point.transform.position - transform.position;
        Vector2 direction = (point.transform.position - transform.position).normalized;
        transform.Translate(speed * Time.deltaTime * direction);
        if (difference.magnitude < 0.1) {
            pointIndex = (pointIndex + 1) % trackPoints.Length;
        }
    }

    private bool LoseHealth(int amount) {
        if (amount >= health) {
            return false;
        }
        health -= amount;
        healthBar.transform.localScale = new Vector3((float)health / maxHealth * 0.75f, 1);
        healthBar.transform.Translate(new Vector3((float)-amount / maxHealth / 2, 0));
        return true;
    }

    public virtual void GetAttacked(Tower tower) {
        if (!LoseHealth(tower.AttackPower)) {
            Destroy(gameObject);
            FindAnyObjectByType<GameManager>().AddMoney(value);
        }
    }
}