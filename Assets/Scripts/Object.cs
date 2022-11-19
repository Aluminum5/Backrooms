using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour {
    void Update() {
        GameObject obj = GameObject.FindWithTag("Player");
        Vector3 vec = obj.transform.position;
        Vector3 pos = new Vector3(vec.x, transform.position.y, vec.z);
        if(Vector3.Distance(transform.position, pos) > Static.distance) {
            Destroy(gameObject);
        }
    }
}
