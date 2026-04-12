using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject turretSpotPrefab;
    [SerializeField] private GameObject turretSet;
    private TurretSpot previousTurretSpot;

    void SpawnTurretFields() {
        // Generate the top and bottom border turret spots
        for (float x = -8; x <= 0; x++) {
            Instantiate(turretSpotPrefab, new Vector2(x, 6), turretSpotPrefab.transform.rotation, turretSet.transform);
            Instantiate(turretSpotPrefab, new Vector2(x, -6), turretSpotPrefab.transform.rotation, turretSet.transform);
        }

        // Generate the left and right border turret spots
        for (int y = -5; y <= 5; y++) {
            Instantiate(turretSpotPrefab, new Vector2(-8, y), turretSpotPrefab.transform.rotation, turretSet.transform);
            Instantiate(turretSpotPrefab, new Vector2(0, y), turretSpotPrefab.transform.rotation, turretSet.transform);
        }
        
        // Generate the between border turret spots
        for (int x = -4; x <= -1; x++) {
            Instantiate(turretSpotPrefab, new Vector2(x, -2), turretSpotPrefab.transform.rotation, turretSet.transform);
            Instantiate(turretSpotPrefab, new Vector2(x, 2), turretSpotPrefab.transform.rotation, turretSet.transform);
        }

        // Generate the left fill turret spots
        for (int y = -4; y <= 4; y++) {
            Instantiate(turretSpotPrefab, new Vector2(-6, y), turretSpotPrefab.transform.rotation, turretSet.transform);
        }
        
        // Generate horizontal fill turret spots
        for (int x = -5; x <= -2; x++) {
            Instantiate(turretSpotPrefab, new Vector2(x, 4),  turretSpotPrefab.transform.rotation, turretSet.transform);
            Instantiate(turretSpotPrefab, new Vector2(x, 0),  turretSpotPrefab.transform.rotation, turretSet.transform);
            Instantiate(turretSpotPrefab, new Vector2(x, -4),  turretSpotPrefab.transform.rotation, turretSet.transform);
        }
    }

    void Start() {
        SpawnTurretFields();
    }
}
