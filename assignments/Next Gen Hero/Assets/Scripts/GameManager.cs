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

    public Text wayPointSequenceText;

    public int maxEnemyCount = 10;

    private int eggCount;

    private int enemyCount;

    private int enemyDestroyedCount;

    private int touchedEnemyCount;

    private Bounds worldBounds;

    private GameObject eggPrefab;

    private GameObject fireIndicatorPrefab;

    private GameObject[] enemyPreFabs = new GameObject[2];

    private GameObject wayPointBarrierFab;

    private string[] wayPointNames = new string[6];

    private WayPointInfo[] wayPointInfos = new WayPointInfo[6];

    private bool wayPointsVisible = true;

    private int numberOfLives = 5;

    private bool survivalMode;

    public static GameManager Instance { get; private set; }

    public bool MouseMode { get; private set; } = true;

    public bool RandomWayPoints { get; private set; }

    void Awake()
    {
        Instance = this;

        eggPrefab = Resources.Load<GameObject>("Prefab/Egg");
        fireIndicatorPrefab = Resources.Load<GameObject>("Prefab/FireIndicator");

        enemyPreFabs[0] = Resources.Load<GameObject>("Prefab/Plane");
        enemyPreFabs[1] = Resources.Load<GameObject>("Prefab/Plane_Green");

        wayPointBarrierFab = Resources.Load<GameObject>("Prefab/WayPointBarrier");

        worldBounds = new Bounds(Vector3.zero, Vector3.one);

        UpdateWorldBounds();

        UpdateWayPointSequenceText();

        UpdateEggCountText();

        UpdateEnemyCountText();

        UpdateHeroInfoText();

        InitializeWaypoints();
    }

    // Update is called once per frame
    void Update()
    {
        while(enemyCount < maxEnemyCount)
        {
            SpawnAnEnemy();
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

        if (Input.GetKeyDown(KeyCode.H))
        {
            wayPointsVisible = !wayPointsVisible;

            ToggleWayPointVisibility();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            RandomWayPoints = !RandomWayPoints;

            UpdateWayPointSequenceText();
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

    private void UpdateWayPointSequenceText()
    {
        var sequence = RandomWayPoints ? "Random" : "Sequence";

        if (wayPointSequenceText != null)
        {
            wayPointSequenceText.text = $"WAYPOINTS: ({sequence})";
        }
        else
        {
            Debug.Log("wayPointSequenceText is null");
        }
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

        SpawnAnEnemy();
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

    public void SpawnAnEgg(Vector2 position, Vector2 direction)
    {
        GameObject egg = Instantiate(eggPrefab);

        egg.transform.position = position;
        egg.transform.up = direction;

        Instance.IncreaseEggCount();

        var fireIndicator = Instantiate(fireIndicatorPrefab);
        fireIndicator.transform.position = new Vector2(0, - worldBounds.size.y / 2 * 0.95f);
        fireIndicator.transform.up = Vector2.up;
    }

    public void SpawnAnEnemy()
    {
        if (enemyCount < maxEnemyCount)
        {
            var randEnemy = Random.Range(0, 2);

            GameObject enemy = Instantiate(enemyPreFabs[randEnemy]);

            var randX = Random.Range(-0.9f, 0.9f);
            var randY = Random.Range(-0.9f, 0.9f);

            var position = new Vector2(worldBounds.size.x * randX / 2.0f, worldBounds.size.y * randY / 2.0f);

            enemy.transform.position = position;
            enemy.transform.up = Vector2.up;

            IncreaseEnemyCount();
        }
    }

    private void AddWayPoint(string wayPointName, Vector2 position)
    {
        int index = System.Array.IndexOf(wayPointNames, wayPointName);

        var wayPoint = Instantiate(wayPointInfos[index].wayPointFab);

        if (position.x == float.NegativeInfinity)
        {
            position = wayPointInfos[index].wayPointInitialPosition;
        }
        else
        {
            var xOffset = Random.Range(-15.0f, 15.0f);
            var yOffset = Random.Range(-15.0f, 15.0f);

            position.x = wayPointInfos[index].wayPointInitialPosition.x + xOffset;
            position.y = wayPointInfos[index].wayPointInitialPosition.y + yOffset;
        }

        wayPoint.transform.position = position;
        wayPoint.transform.up = Vector2.up;
        wayPoint.tag = wayPointName;

        wayPointInfos[index].wayPointCurrentPosition = wayPoint.transform.position;

        var wayPointBarrier = Instantiate(wayPointBarrierFab);

        wayPointBarrier.transform.position = position;
        wayPointBarrier.transform.up = Vector2.up;

        wayPointBarrier.tag = wayPointName;

        wayPointInfos[index].wayPoint = wayPoint;
        wayPointInfos[index].wayPointBarrier = wayPointBarrier;
    }

    public void DestroyWayPoint(GameObject wayPointObject)
    {
        var wayPointName = wayPointObject.name.Replace("(Clone)", "");

        if (string.IsNullOrWhiteSpace(wayPointName))
        {
            return;
        }

        var position = wayPointObject.transform.position;

        var index = System.Array.IndexOf(wayPointNames, wayPointName);

        Debug.Log($"Destroying WayPoint {index}. {wayPointInfos[index].wayPointBarrier.tag}");

        Destroy(wayPointInfos[index].wayPointBarrier);

        Destroy(wayPointObject);

        AddWayPoint(wayPointName, position);
    }

    private void InitializeWaypoints()
    {
        var namePrefix = 'A';

        var maxXValue = worldBounds.size.y * 0.8f / 2;
        var maxYValue = worldBounds.size.y * 0.6f / 2;

        for (int i = 0; i < 6; i++)
        {
            var name = $"{namePrefix}_Walk";

            if (i == 0)
            {
                wayPointInfos[i].wayPointInitialPosition = new Vector2(-maxXValue, maxYValue);
            }
            else if (i == 1)
            {
                wayPointInfos[i].wayPointInitialPosition = new Vector2(maxXValue, -maxYValue);
            }
            else if (i == 2)
            {
                wayPointInfos[i].wayPointInitialPosition = new Vector2(maxXValue / 2, 0);
            }
            else if (i == 3)
            {
                wayPointInfos[i].wayPointInitialPosition = new Vector2(-maxXValue, -maxYValue);
            }
            else if (i == 4)
            {
                wayPointInfos[i].wayPointInitialPosition = new Vector2(maxXValue, maxYValue);
            }
            else if (i == 5)
            {
                wayPointInfos[i].wayPointInitialPosition = new Vector2(-maxXValue / 2, 0);
            }

            wayPointNames[i] = name;

            wayPointInfos[i].wayPointFab = Resources.Load<GameObject>($"Prefab/{name}");
            AddWayPoint(name, Vector2.negativeInfinity);

            namePrefix++;
        }
    }

    void ToggleWayPointVisibility()
    {
        foreach (var wayPointInfo in wayPointInfos)
        {
            wayPointInfo.wayPoint.SetActive(wayPointsVisible);
        }
    }

    public GameObject GetPointingObject(GameObject currentObject)
    {
        if (currentObject == null)
        {
            return wayPointInfos[Random.Range(0, wayPointNames.Length - 1)].wayPoint;
        }

        var name = currentObject.name.Replace("(Clone)", "");

        int index = System.Array.IndexOf(wayPointNames, name);

        index++;

        if (index >= wayPointNames.Length)
        {
            index = 0;
        }

        return wayPointInfos[index].wayPoint;
    }

    public int GetPointingWayPointIndex(int currentIndex)
    {
        if (currentIndex == -1)
        {
            return Random.Range(0, wayPointNames.Length - 1);
        }

        var index = currentIndex;

        if(RandomWayPoints)
        {
            while (index == currentIndex)
            {
                index = Random.Range(0, wayPointNames.Length - 1);
            }
        }
        else
        {
            index = currentIndex + 1;
        }

        if (index >= wayPointNames.Length)
        {
            index = 0;
        }

        return index;
    }

    public GameObject GetWayPoint(int index)
    {
        return wayPointInfos[index].wayPoint;
    }

    public int GetIndexOfWayPointObject(GameObject obj)
    {
        var name = obj.tag;

        return System.Array.IndexOf(wayPointNames, name);
    }
}
