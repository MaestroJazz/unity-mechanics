using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class HeroBehavior : MonoBehaviour
{

    public float speed = 0.01f;
    private float rotateSpeed = 45.0f;

    private Direction direction = Direction.Right;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //speed += Input.GetAxis("Vertical");


        if (!GameManager.Instance.IsObjectWithinWorldBounds(GetComponent<Renderer>().bounds))
        {
            //speed = -speed;
        }



        Vector3 dir = Vector3.right;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            dir = Vector3.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            dir = Vector3.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Vector3.left;
        }

        transform.position += dir * speed * Time.smoothDeltaTime;

        //transform.Rotate(Vector3.forward, -1.0f * Input.GetAxis("Horizontal") * rotateSpeed * Time.smoothDeltaTime);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision with {collision.gameObject.name}");

        if (!collision.gameObject.name.Contains("Potato"))
        {
            return;
        }

        speed *= 0.90f;
    }
}
