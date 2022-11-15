using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingMovement : MonoBehaviour
{
    public float speed = 2.5f;

    void Update()
    {
        transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
    }
}
