using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Bounds worldBounds;

    private GameObject heroPrefab;

    private GameObject potatoPrefab;

    public int TotalPotatoes { get; set; } = 5;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        heroPrefab = Resources.Load<GameObject>("Prefab/Hero");

        potatoPrefab = Resources.Load<GameObject>("Prefab/Potato");

        worldBounds = new Bounds(Vector3.zero, Vector3.one);

        UpdateWorldBounds();

        SpawnHero();

        for (int i=0; i < TotalPotatoes; i++)
        {
            SpawnPotato(i);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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


        Debug.Log($"Center={worldBounds.center} Size={worldBounds.size}");
    }

    public bool IsObjectWithinWorldBounds(Bounds objectBounds)
    {
        return worldBounds.Intersects(objectBounds);
    }

    public void SpawnHero()
    {

        GameObject hero = GameObject.Instantiate(heroPrefab);

        var position = new Vector2(-worldBounds.size.x / 2 + 1, 0);

        hero.transform.position = position;
        hero.transform.up = Vector2.up;

    }

    public void SpawnPotato(int sector)
    {
        GameObject potato = GameObject.Instantiate(potatoPrefab);

        var size = worldBounds.size.x / 2;

        var value = Random.Range(-size * 0.9f, -size * 0.9f + size * 0.9f / TotalPotatoes);

        var randX = sector * 2 * size * 0.9f / TotalPotatoes + value;

        var randY = worldBounds.size.y * Random.Range(-0.9f, 0.9f) / 2.0f;

        Debug.Log($"i={sector} Val={value} X={randX} Y={randY}");

        var position = new Vector2(randX, randY);

        potato.transform.position = position;
        potato.transform.up = Vector2.up;
    }
}
