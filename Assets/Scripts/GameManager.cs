using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public enum BuildOption {
        Nothing,
        WaterTower,
        FireTower,
        AirTower,
        EarthTower,
        Sell
    }

    private BuildOption buildOption;
    public GameObject TurretSpotPrefab { get => turretSpotPrefab; }
    
    [SerializeField] private GameObject turretSpotPrefab;
    [SerializeField] private GameObject airTowerPrefab;
    [SerializeField] private int airTowerPrice;
    [SerializeField] private GameObject waterTowerPrefab;
    [SerializeField] private int waterTowerPrice;
    [SerializeField] private GameObject fireTowerPrefab;
    [SerializeField] private int fireTowerPrice;
    [SerializeField] private GameObject earthTowerPrefab;
    [SerializeField] private int earthTowerPrice;
    [SerializeField] private GameObject turretSet;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private GameObject[] monsterPrefabs;
    [SerializeField] private Vector2 spawnPosition;
    [SerializeField] private float spawnDelay;
    [SerializeField] private float gameOverFrameDurationSec = 0.05f;

    private TurretSpot selectedTurretSpot;
    private Tower selectedTower;
    private Camera mainCamera;
    private int money = 1;

    void SpawnTurretFields() {
        // Generate the top and bottom border turret spots
        for (float x = -10; x <= -2; x++) {
            Instantiate(turretSpotPrefab, new Vector2(x, 6), turretSpotPrefab.transform.rotation, turretSet.transform);
            Instantiate(turretSpotPrefab, new Vector2(x, -6), turretSpotPrefab.transform.rotation, turretSet.transform);
        }

        // Generate the left and right border turret spots
        for (int y = -5; y <= 5; y++) {
            Instantiate(turretSpotPrefab, new Vector2(-10, y), turretSpotPrefab.transform.rotation, turretSet.transform);
            Instantiate(turretSpotPrefab, new Vector2(-2, y), turretSpotPrefab.transform.rotation, turretSet.transform);
        }
        
        // Generate the between border turret spots
        for (int x = -6; x <= -3; x++) {
            Instantiate(turretSpotPrefab, new Vector2(x, -2), turretSpotPrefab.transform.rotation, turretSet.transform);
            Instantiate(turretSpotPrefab, new Vector2(x, 2), turretSpotPrefab.transform.rotation, turretSet.transform);
        }

        // Generate the left fill turret spots
        for (int y = -4; y <= 4; y++) {
            Instantiate(turretSpotPrefab, new Vector2(-8, y), turretSpotPrefab.transform.rotation, turretSet.transform);
        }
        
        // Generate horizontal fill turret spots
        for (int x = -7; x <= -4; x++) {
            Instantiate(turretSpotPrefab, new Vector2(x, 4),  turretSpotPrefab.transform.rotation, turretSet.transform);
            Instantiate(turretSpotPrefab, new Vector2(x, 0),  turretSpotPrefab.transform.rotation, turretSet.transform);
            Instantiate(turretSpotPrefab, new Vector2(x, -4),  turretSpotPrefab.transform.rotation, turretSet.transform);
        }
    }

    void Start() {
        SpawnTurretFields();
        mainCamera = Camera.main;
        DisplayMoney();
        StartCoroutine(SpawnMonsters());
    }

    void StopHoveringOverTurret() {
        if (selectedTurretSpot != null) {
            selectedTurretSpot.EndHoverOver();
            selectedTurretSpot = null;
        }
    }

    void selectTurretSpot(TurretSpot turret) {
        // Return if the mouse is still over the same turret spot
        if (selectedTurretSpot == turret) {
            return;
        }

        // Stop hovering over the previous turret, if any
        StopHoveringOverTurret();
        
        // Hover over the selected turret
        turret.HoverOver(buildOption);
        
        // Store the selected turret
        selectedTurretSpot = turret;
    }

    void SelectTower(Tower tower) {
        if (selectedTower == tower) {
            return;
        }

        if (selectedTower != null) {
            selectedTower.EndHoverOver();
        }

        selectedTower = tower;
        tower.HoverOver(buildOption);
    }

    void StopHoveringOverTower() {
        if (selectedTower != null) {
            selectedTower.EndHoverOver();
            selectedTower = null;
        }
    }

    void HandleMouseOver() {
        if (buildOption == BuildOption.Nothing) {
            return;
        }
        
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        
        // Cast a ray from the mouse
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(mousePosition), Vector2.zero);
        
        // Return if it didn't hit anything
        if (hit.collider == null) {
            StopHoveringOverTurret();
            StopHoveringOverTower();
            return;
        }
        
        // Check whether the hit object was a turret
        GameObject selectedObject = hit.collider.gameObject;
        if (selectedObject.TryGetComponent(out Tower tower)) {
            SelectTower(tower);

            // Stop hovering over the previous turret, if any
            StopHoveringOverTurret();
            
        }else if (selectedObject.TryGetComponent(out TurretSpot turretSpot)) {
            selectTurretSpot(turretSpot);
            StopHoveringOverTower();
        }
    }

    void Update() {
        HandleMouseOver();
    }

    public void QuitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
        #else
        Application.Quit();
        #endif
    }

    public void ResetProgress() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SelectWaterTower() {
        buildOption = BuildOption.WaterTower;
    }
    
    public void SelectFireTower() {
        buildOption = BuildOption.FireTower;
    }
    
    public void SelectAirTower() {
        buildOption = BuildOption.AirTower;
    }
    
    public void SelectEarthTower() {
        buildOption = BuildOption.EarthTower;
    }
    
    public void DeselectAll() {
        buildOption = BuildOption.Nothing;
    }

    public void SelectSell() {
        buildOption = BuildOption.Sell;
    }

    void BuildTower(GameObject towerPrefab, int price) {
        if (selectedTurretSpot == null || !PayMoney(price)) {
            return;
        }
        Instantiate(towerPrefab, selectedTurretSpot.transform.position, towerPrefab.transform.rotation, turretSet.transform);
        Destroy(selectedTurretSpot.gameObject);
        selectedTurretSpot = null;
    }

    public void OnClick() {
        Debug.Log(buildOption);
        switch (buildOption) {
            case BuildOption.Nothing:
                break;
            case BuildOption.Sell:
                if (selectedTower == null) {
                    Debug.Log("No selected tower");
                    break;
                }
                Debug.Log(selectedTower.gameObject.name);
                money += selectedTower.Sell();
                selectedTower = null;
                break;
            case BuildOption.WaterTower:
                BuildTower(waterTowerPrefab, waterTowerPrice);
                break;
            case BuildOption.FireTower:
                BuildTower(fireTowerPrefab, fireTowerPrice);
                break;
            case BuildOption.AirTower:
                BuildTower(airTowerPrefab, airTowerPrice);
                break;
            case BuildOption.EarthTower:
                BuildTower(earthTowerPrefab, earthTowerPrice);
                break;
        }
    }

    private void DisplayMoney() {
        moneyText.text = money.ToString();
    }

    public void AddMoney(int amount) {
        if (amount > 0) {
            money += amount;
            DisplayMoney();
        }
    }
    
    private bool PayMoney(int amount) {
        if (money >= amount) {
            money -= amount;
            DisplayMoney();
            return true;
        }
        return false;
    }

    private IEnumerator SpawnMonsters() {
        while (true) {
            while (Time.deltaTime < gameOverFrameDurationSec) {
                GameObject monster = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
                Instantiate(monster, spawnPosition, monster.transform.rotation);
                yield return new WaitForSeconds(spawnDelay);
                spawnDelay *= 0.99f;
            }

            while (Time.deltaTime > gameOverFrameDurationSec) {
                spawnDelay /= 0.99f;
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
}