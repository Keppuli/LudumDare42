using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceMine : MonoBehaviour {

    Rigidbody2D rb;
    public bool triggered;
    public float explodeTime;
    public float timer;
    public GameObject explosion;
    public GameObject scrap;
    public Sprite armed;
    SpriteRenderer sr;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        timer = explodeTime;
    }
    private void Update()
    {

        if (triggered)
        {
            sr.sprite = armed;
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Explode();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        triggered = true;
    }
    void Explode()
    {
        GameObject instance = Instantiate(explosion,transform.position,Quaternion.identity);
        //instance.GetComponent<Explosion>().AddForce(rb.velocity); // Transfer velocity to explosion so that movement vector remains
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        GameManager.instance.debrisEliminated += 1;
    }
}
