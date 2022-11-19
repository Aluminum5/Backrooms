using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour {
    private NavMeshAgent controller;
    private GameObject camera;
    public AudioSource audio;
    public Animator anim;
    public float walk = 3f;
    public float speed = 5f;
    public float max = 2f;
    public float distance = 2f;
    public Transform view;
    public Vector3 offset;

    void Start() {
        NavMesh.pathfindingIterationsPerFrame = 500;
        camera = GameObject.FindWithTag("MainCamera");
        controller = GetComponent<NavMeshAgent>();
        controller.destination = transform.position;
    }

    void Update() {
        controller.isStopped = false;
        Vector3 cPos = camera.transform.position;
        Vector3 direction = cPos - view.position;
        float dist = Vector3.Distance(view.position, cPos);
        float delta = Time.deltaTime;
        float aux = audio.volume;

        controller.speed = walk;
        audio.volume = Mathf.Max(aux - delta * max, 0);
        if(!Physics.Raycast(view.position, direction, dist)) {
            audio.volume = Mathf.Min(aux + delta * max, 1);
            if(dist < distance) Catch(transform.position + offset);
            controller.destination = cPos;
            controller.speed = speed;
        }

        IsDead(camera);
        bool run = controller.speed > walk;
        anim.SetBool("Walking", true);
        anim.SetBool("Running", run);
        if(controller.velocity == Vector3.zero) {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
        }
    }

    void IsDead(GameObject camera) {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        bool time = state.normalizedTime >= 1.0f;
        if(camera.GetComponent<Remove>().Dead()) {
            Rotate(camera.transform.position);
            controller.velocity = Vector3.zero;
            controller.isStopped = true;
            if(state.IsName("Scream") && time) {
                camera.GetComponent<Remove>().Done();
            }
        }
    }

    void Rotate(Vector3 pos) {
        pos.y = transform.position.y;
        Quaternion rotation = Quaternion.LookRotation(pos - transform.position);
        Quaternion rotate = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * 360.0f);
        transform.rotation = rotate;
    }

    void Catch(Vector3 pos) {
        if(camera.GetComponent<Remove>().Die(pos)) {
            anim.SetTrigger("Scream");
        }
    }
}
