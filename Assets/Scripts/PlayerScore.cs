using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    public TMP_Text textScore;
    private int score = 0;

    void Update()
    {
        
        if (transform.position.x > score) {
            score = (int)transform.position.x;
        }

        if (textScore.text != score.ToString()) {
            textScore.text = score.ToString();
        }
    }
}
