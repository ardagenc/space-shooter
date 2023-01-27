using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float speed = 8;

    void Update()
    {
        // translate laser up
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // if laser position is greater than y, destroy
        if (transform.position.y >= 8)
        {
            // check if this object has a parent
            // destroy the parent 
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
