using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Book : MonoBehaviour {
    private AudioSource bookpickUpsource;
    public AudioClip pickupSound;
    public GameObject pickupEffect;
	// Use this for initialization
	void Start () {
     //   bookpickUpsource = GetComponent<AudioSource>();
     }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           // bookpickUpsource.clip = pickupSound;
           // bookpickUpsource.Play();
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
