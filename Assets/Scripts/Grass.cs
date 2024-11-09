using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Grass : MonoBehaviour {
    Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            anim.SetTrigger("move");
        }
    }
}
