using System;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    GameObject[] trackPoints;
    private int pointIndex = 0;
    [SerializeField] protected float speed;
    [SerializeField] protected int value;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

    public virtual void GetAttacked(Tower tower) {
        Destroy(gameObject);
        FindAnyObjectByType<GameManager>().AddMoney(value);
    }
}
