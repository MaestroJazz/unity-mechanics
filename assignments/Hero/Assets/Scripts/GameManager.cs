using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text eggCountText;

    public Text enemyCountText;

    public Text heroInfoText;

    public Text gameOverText;

    public int maxEnemyCount = 10;

    private int eggCount;

    private int enemyCount;

    private int enemyDestroyedCount;

    private int touchedEnemyCount;

    private Bounds worldBounds;

    private GameObject enemyPreFab;

    private int numberOfLives = 5;

    private bool survivalMode;

    public static GameManager Instance { get; private set; }

    public bool MouseMode { get; private set; } = true;

    void Awake()
    {
        Instance = this;

        worldBounds = new Bounds(Vector3.zero, Vector3.one);

        UpdateWorldBounds();

        UpdateEggCountText();

        UpdateEnemyCountText();

        UpdateHeroInfoText();

        if (enemyPreFab == null)
        {
            enemyPreFab = Resources.Load<GameObject>("Prefab/Plane");
        }

    }

    // Update is called once per frame
    void Update()
    {
        while(enemyCount < maxEnemyCount)
        {
            AddEnemy();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            MouseMode = !MouseMode;
            UpdateHeroInfoText();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            survivalMode = !survivalMode;
            UpdateHeroInfoText();
        }
    }

    public void IncreaseEggCount()
    {
        eggCount++;

        UpdateEggCountText();
    }

    public void DecreaseEggCount()
    {
        eggCount--;

        UpdateEggCountText();
    }

    private void UpdateEggCountText()
    {
        if (eggCountText != null)
        {
            eggCountText.text = $"EGG: OnScreen ({eggCount})";
        }
        else
        {
            Debug.Log("eggCountText is null");
        }
    }

    private void IncreaseEnemyCount()
    {
        enemyCount++;

        UpdateEnemyCountText();
    }

    public void DecreaseEnemyCount()
    {
        enemyCount--;
        enemyDestroyedCount++;

        AddEnemy();
        UpdateEnemyCountText();
    }

    private void UpdateEnemyCountText()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = $"ENEMY: Count ({enemyCount}) Destroyed ({enemyDestroyedCount})";
        }
        else
        {
            Debug.Log("enemyCountText is null");
        }
    }

    public void IncreaseTouchedEnemyCount()
    {
        touchedEnemyCount++;

        if (survivalMode)
        {
            numberOfLives--;
        }

        UpdateHeroInfoText();
    }

    private void UpdateHeroInfoText()
    {
        var drive = MouseMode ? "Mouse" : "Key";

        if (heroInfoText != null)
        {
            if (survivalMode)
            {
                heroInfoText.text = $"HERO: Drive ({drive}) TouchedEnemy ({touchedEnemyCount}) Life ({numberOfLives})";
            }
            else
            {
                heroInfoText.text = $"HERO: Drive ({drive}) TouchedEnemy ({touchedEnemyCount})";
            }
        }
        else
        {
            Debug.Log("heroInfoText is null");
        }

        if (numberOfLives == 0)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "GAME OVER!";
        }
        else
        {
            gameOverText.gameObject.SetActive(false);
        }
    }

    public bool HasLives()
    {
        return !survivalMode || numberOfLives > 0;
    }
   
    private void UpdateWorldBounds()
    {
        var camera = Camera.main;

        if (camera == null)
        {
            return;
        }

        float maxY = camera.orthographicSize;
        float maxX = camera.orthographicSize * camera.aspect;
        float sizeX = 2 * maxX;
        float sizeY = 2 * maxY;
        float sizeZ = Mathf.Abs(camera.farClipPlane - camera.nearClipPlane);

        Vector3 cameraPosition = camera.transform.position;
        cameraPosition.z = 0.0f;
        worldBounds.center = cameraPosition;
        worldBounds.size = new Vector3(sizeX, sizeY, sizeZ);
    }

    public bool IsObjectWithinWorldBounds(Bounds objectBounds)
    {
        return worldBounds.Intersects(objectBounds);
    }


    public void AddEnemy()
    {
        if (enemyCount < maxEnemyCount)
        {
            GameObject enemy = GameObject.Instantiate(enemyPreFab);

            var randX = Random.Range(-0.9f, 0.9f);
            var randY = Random.Range(-0.9f, 0.9f);


            var position = new Vector2(worldBounds.size.x * randX / 2.0f, worldBounds.size.y * randY / 2.0f);

            Debug.Log($"RandX: {randX} RandY: {randY}. Position={position} Bounds X: {worldBounds.size.x} Y: {worldBounds.size.y}");


            enemy.transform.position = position;
            enemy.transform.up = Vector2.up;

            IncreaseEnemyCount();
        }
    }

}
