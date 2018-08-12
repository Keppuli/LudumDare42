using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhaust : MonoBehaviour {

    SpriteRenderer sr;
    float alpha = 1;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        transform.Rotate(Vector3.forward, Random.Range(-45, 45));

    }

    // Update is called once per frame
    void Update () {

        alpha -= Time.deltaTime;
        sr.color = new Color(1, 1, 1, alpha);
        if (alpha <= 0)
            Destroy(gameObject);
	}
}
