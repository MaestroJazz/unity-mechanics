using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 20f;
    private float rate = 0.04f;

    //private GameObject pointingObject;

    private int pointingWayPointIndex = -1;

    Color color;

    private int collisionCount;
    // Start is called before the first frame update
    void Start()
    {
        color = GetComponent<Renderer>().material.color;

        //pointingObject = GameManager.Instance.GetPointingObject(null);

        pointingWayPointIndex = GameManager.Instance.GetPointingWayPointIndex(pointingWayPointIndex);
    }

    // Update is called once per frame
    void Update()
    {
        var pointingObject = GameManager.Instance.GetWayPoint(pointingWayPointIndex);
        PointAtPosition(pointingObject.transform.position, rate * Time.smoothDeltaTime);

        transform.position += (speed * Time.smoothDeltaTime) * transform.up;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"ONTRIGGERENTER:------ {collision.gameObject.name}");

        if (!collision.gameObject.name.Contains("WayPointBarrier"))
        {
            return;
        }

        if (pointingWayPointIndex != GameManager.Instance.GetIndexOfWayPointObject(collision.gameObject))
        {
            return;
        }

        //pointingObject = GameManager.Instance.GetPointingObject(pointingObject);
        pointingWayPointIndex = GameManager.Instance.GetPointingWayPointIndex(pointingWayPointIndex);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Plane")
            || collision.gameObject.name.Contains("Walk"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            return;
        }

        if (!collision.gameObject.name.Contains("Hero")
            && !collision.gameObject.name.Contains("Egg"))
        {
            return;
        }

        color.a *= 0.8f;

        GetComponent<Renderer>().material.color = color;

        collisionCount++;

        var isCollisionWithHero = collision.gameObject.name == "Hero";

        if (isCollisionWithHero || collisionCount == 4)
        {
            Destroy(gameObject);

            GameManager.Instance.DecreaseEnemyCount();

            if (isCollisionWithHero)
            {
                GameManager.Instance.IncreaseTouchedEnemyCount();
            }
        }
    }

    void OnApplicationQuit()
    {
        Destroy(gameObject);
    }

    private void PointAtPosition(Vector3 p, float r)
    {
        Vector3 v = p - transform.position;
        transform.up = Vector3.LerpUnclamped(transform.up, v, r);
    }
}
