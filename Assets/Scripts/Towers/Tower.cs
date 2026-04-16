using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    protected float range = 2;
    [SerializeField] protected float reloadTime = 2;
    [SerializeField] protected int value = 1;
    [SerializeField] private GameObject turretSpotPrefab;
    bool canShoot = true;
    
    Monster FindnearestMonster() {
        Monster[] monsters = FindObjectsByType<Monster>(FindObjectsSortMode.None);
        if (monsters == null) {
            return null;
        }
        
        Monster closestMonster = null;
        float closestDistance = float.MaxValue;

        foreach (Monster monster in monsters) {
            float distance = (monster.transform.position - transform.position).sqrMagnitude;
            if(closestMonster == null) {
                closestMonster = monster;
                closestDistance = distance;
                continue;
            }
            if (distance < closestDistance) {
                closestDistance = distance;
                closestMonster = monster;
            }
        }
        
        return closestMonster;
    }

    void Reload() {
        canShoot = true;
    }

    void ShootAtMonster(Monster monster) {
        if (!canShoot) {
            return;
        }
        
        if (monster == null) {
            return;
        }
        
        if ((monster.transform.position - transform.position).sqrMagnitude > range) {
            return;
        }
        
        monster.GetAttacked(this);
        canShoot = false;
        Invoke(nameof(Reload), reloadTime);
    }
    
    // Update is called once per frame
    void Update() {
        Monster nearestMonster = FindnearestMonster();
        ShootAtMonster(nearestMonster);
    }

    public void Sell() {
        Instantiate(turretSpotPrefab, transform.position, transform.rotation, transform.parent);
        Destroy(gameObject);
    }
}
