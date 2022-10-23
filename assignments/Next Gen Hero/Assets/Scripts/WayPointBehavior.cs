using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointBehavior : MonoBehaviour
{
    Color color;

    private int collisionCount;

    // Start is called before the first frame update
    void Start()
    {
        color = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Waypoint: OnCollisionEnter2D: {collision.gameObject.name}. Count={collisionCount}");

        var isCollisionWithEgg = collision.gameObject.name.StartsWith("Egg");

        if (isCollisionWithEgg)
        {
            color.a *= 0.8f;

            GetComponent<Renderer>().material.color = color;

            collisionCount++;

            Debug.Log($"Waypoint: Collision count: {collisionCount}");

            if (collisionCount == 4)
            {
                GameManager.Instance.DestroyWayPoint(gameObject);
            }
        }
    }
}
