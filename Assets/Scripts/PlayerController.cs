using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float jumpForce;
    public float mouseSensetivity;

    [Header("UI")]
    public GameObject BuyUI;
    private TMP_Text moneyText;

    [NonSerialized]
    public int money = 100, Wheat, Seeds = 5;

    float moveX, moveZ, mouseHorizontal, mouseVertical, jumped, moveSpeed;

    Vector3 movement;
    [NonSerialized]
    public Vector3 pos;

    private Transform cam;

    Rigidbody rb;

    Ray ray;
    RaycastHit hit;

    [NonSerialized]
    public Soil soil;

    Collider other = null;

    bool isGrounded, releasedJumpKey, releasedBuyKey;

    private AreaData area;
    private World world;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
        world = GameObject.Find("World").GetComponent<World>();
        moneyText = GameObject.Find("Money").GetComponent<TMP_Text>();
    }

    private void FixedUpdate()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
            other = hit.collider;
        else
            other = null;

        if (other != null)
        {
            if (other.tag == "Farm Slot")
                soil = other.GetComponent<Soil>();
            else
                soil = null;
        }
        if (soil != null)
            soil.stateText.SetActive(true);
    }

    private void Update()
    {
        pos = transform.position;

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

        if (area != null)
        {
            BuyUI.SetActive(true);
            BuyUI.GetComponentInChildren<TMP_Text>().text = "Cost: " + area.type.cost.ToString() + "$" + "\n" + "Press 'E' to buy!";
        }
        else
            BuyUI.SetActive(false);
        moneyText.text = money + "$\nWheat: " + Wheat + "\nSeeds: " + Seeds;
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

        if (Input.GetAxisRaw("Activate") != 0 && BuyUI.activeSelf && releasedBuyKey)
        {
            releasedBuyKey = false;
            buyArea();
        }
        if (Input.GetAxisRaw("Activate") == 0)
            releasedBuyKey = true;

        if (Input.GetAxisRaw("Fire1") != 0 && soil != null)
            soil.Action();
    }

    void buyArea()
    {
        if (money - area.type.cost < 0)
            return;
        Area _a = null;
        money -= area.type.cost;
        foreach (Area a in world.EmptyAreas)
            if (a.farmType == area.type)
                _a = a;
        area = null;
        if (_a != null)
        {
            world.EmptyAreas.Remove(_a);
            world.OwnedAreas.Add(_a);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Barier")
            area = other.GetComponentInParent<AreaData>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Barier")
            area = null;
    }
}
