using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
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
        
        //Debug.Log($"OnCollisionEnter2D: {col.gameObject.name}");

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
}
