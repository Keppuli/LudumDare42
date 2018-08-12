using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    AudioSource audioS;
    Rigidbody2D rb;
    public string lifeTime;
    Collider2D col;
    SpriteRenderer sr;
    public float alpha = 1;
    public float killTimer = .1f;

    void Start () {
        audioS = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        transform.Rotate(Vector3.forward, Random.Range(-45, 45));

        // Add effects according to explosion distance to player
        float distToPlayer = 0;
        if (Player.instance)
        {
            distToPlayer = Vector3.Distance(transform.position, Player.instance.transform.position);
        }
        else
        {
            distToPlayer = Vector3.Distance(transform.position, Truck.instance.transform.position);
        }

        audioS.volume = 1f - distToPlayer / 4;
        Camera.main.GetComponent<CameraManager>().shakeDuration = 0.2f;
        Camera.main.GetComponent<CameraManager>().shakeAmount = 10f - distToPlayer * 1.5f;
    }
    public void AddForce(Vector2 amount)
    {
        rb.AddForce(amount, ForceMode2D.Impulse);
        Debug.Log(rb.velocity);
    }
    private void Update()
    {
        transform.localScale += new Vector3(.5f, .5f, .5f)*Time.deltaTime;
        alpha -= Time.deltaTime;
        sr.color = new Color(1,1, 1, alpha);
        if (alpha <= 0)
            Destroy(gameObject);
        killTimer -= Time.deltaTime;
        if (killTimer <= 0)
            col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            GameManager.instance.gameOver = true;
            GameManager.instance.gameOverReason.text = "YOU DIED TO A MINE!";
        }
        if (collision.gameObject.CompareTag("Truck"))
        {
            Truck.instance.TakeDamage();
        }
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null && !collision.gameObject.CompareTag("Explosion"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddExplosionForce(2000, transform.position, 1500);
        }
    }

}
