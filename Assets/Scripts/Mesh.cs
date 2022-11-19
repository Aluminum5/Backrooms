using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mesh : MonoBehaviour {
    public Transform player;
    public NavMeshSurface surface;
    public float tile = 3f;

    void Start() {
        LateUpdate();
    }

    void LateUpdate() {
        Vector3 vec = player.transform.position;
        float x = Mathf.Round(vec.x / tile) * tile;
        float z = Mathf.Round(vec.z / tile) * tile;
        transform.position = new Vector3(x, 0, z);
        surface.BuildNavMesh();
    }
}
