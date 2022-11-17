using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingMovement : MonoBehaviour
{
    public float speed = 2.5f;
    
    public GameObject BackgroundBlur;
    public GameObject YouDied;

    void Update()
    {
        transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.name == "Player")
        {
            BackgroundBlur.SetActive(true);
            YouDied.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
