using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireIndicatorBehavior : MonoBehaviour
{
    float scaleRate = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float s = transform.localScale.x;
        s -= scaleRate;
        transform.localScale = new Vector3(s, transform.localScale.y, 1);

        if (s < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
