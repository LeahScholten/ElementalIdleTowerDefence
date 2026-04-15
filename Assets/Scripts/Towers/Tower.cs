using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    protected float range = 2;
    
    Monster FindnearestMonster() {
        Monster[] monsters = FindObjectsByType<Monster>(FindObjectsSortMode.None);
        if (monsters == null) {
            return null;
        }
        
        Monster closestMonster = null;
        float closestDistance = float.MaxValue;

        foreach (Monster monster in monsters) {
            if(closestMonster == null) {
                closestMonster = monster;
                continue;
            }
            float distance = (monster.transform.position - transform.position).sqrMagnitude;
            if (distance < closestDistance) {
                closestDistance = distance;
                closestMonster = monster;
            }
        }
        
        return closestMonster;
    }

    void ShootAtMonster(Monster monster) {
        if (monster == null) {
            Debug.LogWarning("No monster found");
        }else if ((monster.transform.position - transform.position).sqrMagnitude > range) {
            Debug.Log("Monster out of range");
        }
        else {
            Debug.Log(monster.name);
        }
    }
    
    // Update is called once per frame
    void Update() {
        Monster nearestMonster = FindnearestMonster();
        ShootAtMonster(nearestMonster);
    }
}
