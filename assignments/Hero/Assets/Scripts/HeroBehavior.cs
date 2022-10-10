using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBehavior : MonoBehaviour
{
    private GameObject eggPrefab;

    private float fireRate = 0.2f;

    private float nextFire = 0.0f;

    private float rotateSpeed = 45.0f;

    public float speed = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        eggPrefab = Resources.Load<GameObject>("Prefab/Egg");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.HasLives())
        {
            Destroy(gameObject);
        }

        if (Input.GetKey(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            SpawnAnEgg(transform.position, transform.up);
        }

        if (GameManager.Instance.MouseMode)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;
        }
        else
        {
            speed += Input.GetAxis("Vertical") / 60;

            transform.position += transform.up * speed * Time.smoothDeltaTime;
        }

        transform.Rotate(Vector3.forward, -1.0f * Input.GetAxis("Horizontal") * rotateSpeed * Time.smoothDeltaTime);
    }

    public void SpawnAnEgg(Vector2 position, Vector2 direction)
    {
        GameObject egg = GameObject.Instantiate(eggPrefab);

        egg.transform.position = position;
        egg.transform.up = direction;

        Debug.Log($"Transform UP={direction.x}, {direction.y}");

        GameManager.Instance.IncreaseEggCount();
    }

}
