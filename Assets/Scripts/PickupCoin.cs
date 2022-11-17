using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupCoin : MonoBehaviour
{
    private TMP_Text coinCounter;

    private void Start() {
        coinCounter = GameObject.Find("CoinCounter").GetComponent<TMP_Text>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            string curText = coinCounter.text;
            curText = curText.Substring(1);

            coinCounter.text = "x" + (int.Parse(curText) + 1).ToString();

            this.gameObject.SetActive(false);
        }
    }
}
