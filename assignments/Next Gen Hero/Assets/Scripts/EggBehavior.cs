using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehavior : MonoBehaviour
{
    public float speed = 40.0f;

    void Update()
    {
        transform.position += (speed * Time.smoothDeltaTime) * transform.up;

        if (!GameManager.Instance.IsObjectWithinWorldBounds(GetComponent<Renderer>().bounds))
        {
            Destroy(gameObject);

            GameManager.Instance.DecreaseEggCount();
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"OnCollisionEnter2D: {collision.gameObject.name} {collision.gameObject.tag}");


        if (collision.gameObject.name == "Hero" || collision.gameObject.name.Contains("Egg"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else
        {
            Debug.Log($"EggBehavior: DESTROYING!!! OnCollisionEnter2D: {collision.gameObject.name}. This object: {gameObject.name}");

            Destroy(gameObject);
            GameManager.Instance.DecreaseEggCount();
        }
    }

    void OnApplicationQuit()
    {
        Destroy(gameObject);
    }
}
