using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remove : MonoBehaviour {
    private Vector3 die = Vector3.zero;
    public GameObject player;
    public float speed = 360f;

    void Update() {
        if(die != Vector3.zero) {
            Quaternion rotation = Quaternion.LookRotation(die - transform.position);
            Quaternion rotate = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * speed);
            transform.rotation = rotate;
        }
    }

    public bool Dead() {
        return die != Vector3.zero;
    }

    public bool Die(Vector3 pos) {
        if(die != Vector3.zero) return false;
        player.GetComponent<Move>().enabled = false;
        die = pos;
        return true;
    }

    public void Done() {
        if(die != Vector3.zero) {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
