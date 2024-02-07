using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float jumpForce;
    public float mouseSensetivity;

    float moveX, moveZ, mouseHorizontal, mouseVertical, jumped, moveSpeed;

    Vector3 movement;

    private Transform cam;

    Rigidbody rb;

    bool isGrounded, releasedJumpKey;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
    }

    private void Update()
    {
        GetPlayerInput();

        transform.Translate(movement);

        if (jumped != 0 && releasedJumpKey && isGrounded)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        transform.Rotate(Vector3.up * mouseHorizontal);
        cam.Rotate(Vector3.right * -mouseVertical);

        if (jumped != 0)
            releasedJumpKey = false;
        else
            releasedJumpKey = true;
    }

    void GetPlayerInput()
    {
        mouseHorizontal = Input.GetAxisRaw("Mouse X") * mouseSensetivity;
        mouseVertical = Input.GetAxisRaw("Mouse Y") * mouseSensetivity;

        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");
        movement = new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;

        jumped = Input.GetAxisRaw("Jump");

        if (Input.GetAxisRaw("Sprint") != 0)
            moveSpeed = speed * 1.5f;
        else
            moveSpeed = speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
            isGrounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
            isGrounded = false;
    }
}
