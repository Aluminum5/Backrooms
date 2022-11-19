using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    private CharacterController controller;
    private Vector3 velocity = Vector3.zero;
    private float sprint = 0.0f;
    private float xRot = 0f;
    public Camera camera;
    public float mouse = 90f;
    public float speed = 3f;
    public float jump = 1f;
    public float gravity = 10f;
    public float force = 1;
    public float fov = 70;
    public float max = 5f;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        sprint = max;
    }

    void Update() {
        float mouseX = Input.GetAxis("Mouse X") * mouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouse * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        camera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        bool grounded = controller.isGrounded;
        if(controller.collisionFlags != CollisionFlags.None) {
            velocity.y = 0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if(Input.GetAxis("Sprint") > 0 && z > 0) {
            sprint = Mathf.Max(sprint - Time.deltaTime, 0);
            z += Input.GetAxis("Sprint") * (sprint / max);
        }else sprint = Mathf.Min(sprint + Time.deltaTime, max);
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * Time.deltaTime * speed);
        camera.fieldOfView = fov + z * speed;

        if(grounded) velocity.y = -force;
        if(Input.GetButtonDown("Jump") && grounded) {
            velocity.y += Mathf.Sqrt(jump * -3.0f * -gravity);
        }

        velocity.y += -gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
