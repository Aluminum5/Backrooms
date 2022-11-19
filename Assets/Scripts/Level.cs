using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    public int size = 5;
    public int grid = 3;
    public int spacing = 5;
    public GameObject[] parts;
    public GameObject[] entitys;
    public Transform parent;
    public Transform entity;
    public Transform player;
    public Vector3 colission;
    public Vector3 hole;
    public float spawn = 8;

    void Start() {
        Update();
    }

    void Update() {
        float x = Mathf.Round(player.position.x / grid) * grid;
        float z = Mathf.Round(player.position.z / grid) * grid;

        float start = (float) -(size - 1) / 2 * grid;
        for(int row = 0; row < size; row++) {
            for(int column = 0; column < size; column++) {
                Vector3 vec = new Vector3(player.position.x, 0, player.position.z);
                Vector3 pos = new Vector3(x + start + row * grid, 0, z + start + column * grid);
                bool size = Vector3.Distance(vec, pos) < Static.distance;
                if(!Physics.CheckBox(pos, colission / 2) && size) {
                    float noise = Mathf.PerlinNoise(pos.x / spacing, pos.z / spacing);
                    float math = Mathf.PerlinNoise(pos.x / spacing * 2, pos.z / spacing * 2);
                    noise = Mathf.Min(Mathf.Max(noise, 0), 1);
                    GameObject tile = parts[(int) Mathf.Round(noise * (parts.Length - 1))];
                    Quaternion rot = tile.transform.rotation;
                    Vector3 euler = rot.eulerAngles;
                    float random = (Mathf.Round(noise * 80) - 40) * 90.0f;
                    rot = Quaternion.Euler(euler.x, euler.y + random, euler.z);
                    pos.y = tile.transform.position.y;
                    Instantiate(tile, pos, rot, parent);
                    if(noise > 1 - (1 / spawn) && math > 0.5f) {
                        pos = new Vector3(pos.x, 0, pos.z);
                        if(Physics.CheckBox(pos, hole / 2)) {
                            GameObject entity = entitys[(int) Mathf.Round(noise * (entitys.Length - 1))];
                            Instantiate(entity, pos, rot, this.entity);
                        }
                    }
                }
            }
        }
    }
}
