using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBehavior : MonoBehaviour
{
    private float fireRate = 0.2f;

    private float nextFire = 0.0f;

    private float rotateSpeed = 45.0f;

    public float speed = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.HasLives())
        {
            Debug.Log($"HeroBehavior: Destroying {gameObject.name}");

            Destroy(gameObject);
        }

        if (Input.GetKey(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            GameManager.Instance.SpawnAnEgg(transform.position, transform.up);
        }

        if (GameManager.Instance.MouseMode)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;
        }
        else
        {
            speed += Input.GetAxis("Vertical");

            transform.position += transform.up * speed * Time.smoothDeltaTime;
        }

        transform.Rotate(Vector3.forward, -1.0f * Input.GetAxis("Horizontal") * rotateSpeed * Time.smoothDeltaTime);
    }

    void OnApplicationQuit()
    {
        Destroy(gameObject);
    }
}
