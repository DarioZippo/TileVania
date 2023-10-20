using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] float lifetimeBullet = 3f;
    [SerializeField] GameObject particleSystem;

    Rigidbody2D myRigidbody;
    PlayerMovement player;

    float xSpeed;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(xSpeed, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Enemy") {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        else {
            particleSystem.SetActive(false);
            Destroy(this.gameObject, lifetimeBullet);
        }
    }
}
