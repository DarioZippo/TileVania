using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] float scoreIncrement = 100f;

    bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && wasCollected == false) {
            wasCollected = true;

            FindObjectOfType<GameSession>().AddToScore(scoreIncrement);
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
