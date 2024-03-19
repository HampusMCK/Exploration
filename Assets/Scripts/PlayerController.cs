using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float jumpForce;
    public float mouseSensetivity;

    [Header("Object Reference")]
    public GameObject Hand;
    MeshFilter HandMesh;
    MeshRenderer HandMeshRenderer;

    [Header("UI")]
    public GameObject BuyUI;
    public List<GameObject> HotbarSlots;
    public GameObject highlightedHotbar;
    private TMP_Text moneyText;

    [NonSerialized] public int money = 100, Wheat, Seeds = 5, hotbarIndex = 0;
    int ToolToBuyIndex; //Used to decide which tool the raycast is looking at when buying tools

    float moveX, moveZ, mouseHorizontal, mouseVertical, jumped, moveSpeed; //movement floats

    Vector3 movement;

    private Transform cam;

    Rigidbody rb;

    Ray ray;
    RaycastHit hit;

    [NonSerialized] public Soil soil; //Used to determine which soil the player is looking at
    [NonSerialized] public Tools Tool; //Tool that is in the hand
    public List<Tools> tools; //Tools that have been bought

    Collider other = null; //Used to save the object the raycaster hit

    bool isGrounded, releasedJumpKey, releasedBuyKey, releasedMouse; //Bools to prevent double actions

    private AreaData area; //Used to determine which area the player is in contact with when buying a new area
    private World world;

    private void Awake()
    {
        //Hide cursor on start
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        HandMesh = Hand.GetComponent<MeshFilter>();
        HandMeshRenderer = Hand.GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
        world = GameObject.Find("World").GetComponent<World>();
        moneyText = GameObject.Find("Money").GetComponent<TMP_Text>();

        // if (Directory.Exists(Application.persistentDataPath + "/saves/First/"))
        // {
        //     money = World.Instance.data._money;
        //     Seeds = World.Instance.data._seeds;
        //     Wheat = World.Instance.data._wheat;
        // }
    }

    private void FixedUpdate()
    {
        other = null;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
            other = hit.collider;

        if (other != null)
        {
            if (other.tag == "Farm Slot")
                soil = other.GetComponent<Soil>();
            else
                soil = null;

            int.TryParse(other.tag, out ToolToBuyIndex);
        }
        else
        {
            soil = null;
            ToolToBuyIndex = 0;
        }
        if (soil != null)
            soil.stateText.SetActive(true);
    }

    private void Update()
    {
        highlightedHotbar.transform.position = HotbarSlots[hotbarIndex].transform.position;

        ChangeTool();

        if (Tool != null)
            if (Tool.durability <= 0)
                tools.Remove(Tool);

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

    // private void OnApplicationQuit()
    // {
    //     World.Instance.data.getDataFromPlayer(money, Wheat, Seeds);
    //     World.Instance.data.getDataFromWorld(World.Instance.areasBought, World.Instance.toolsBought);

    //     SaveSystem.Save(World.Instance.data);
    // }

    void GetPlayerInput()
    {
        //Get mouse activities
        mouseHorizontal = Input.GetAxisRaw("Mouse X") * mouseSensetivity;
        mouseVertical = Input.GetAxisRaw("Mouse Y") * mouseSensetivity;
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        //////////////////////////////////////////////////////////////////////
        
        //Get movement inputs
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");
        jumped = Input.GetAxisRaw("Jump");
        /////////////////////////////////////////////////////////////////////
        
        movement = new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;


        if (Input.GetAxisRaw("Sprint") != 0)
            moveSpeed = speed * 1.5f;
        else
            moveSpeed = speed;

        //Buying a new area input read
        if (Input.GetAxisRaw("Activate") != 0 && BuyUI.activeSelf && releasedBuyKey)
        {
            releasedBuyKey = false;
            buyArea();
        }
        if (Input.GetAxisRaw("Activate") == 0)
            releasedBuyKey = true;
        //////////////////////////////////////

        //Left Mouse Button input read
        if (Input.GetAxisRaw("Fire1") != 0 && releasedMouse)
        {
            releasedMouse = false;
            if (soil != null)
                soil.Action(this.gameObject);

            if (ToolToBuyIndex != 0)
                buyTool(ToolToBuyIndex);
        }

        if (Input.GetAxisRaw("Fire1") == 0)
            releasedMouse = true;
        /////////////////////////////////////
        

        if (scroll != 0)//When scrolling
        {
            //Scroll through the hotbar
            if (scroll < 0)
                hotbarIndex++;
            if (scroll > 0)
                hotbarIndex--;
            ////////////////////////////////
            
            //If scrolled past hotbar length, jump to other end
            if (hotbarIndex > 8)
                hotbarIndex = 0;
            if (hotbarIndex < 0)
                hotbarIndex = 8;
        }
    }

    void buyArea() //Func to buy new area
    {
        if (money - area.type.cost < 0) //If area is to expensive, end function
            return;
        Area _a = null;
        money -= area.type.cost;
        foreach (Area a in world.Areas)
            if (a.farmType == area.type)
                _a = a;
        area = null;
        if (_a != null)
        {
            world.OwnedAreas.Add(_a);
        }
    }

    void buyTool(int i) //Func to buy new tool
    {
        Tools t = World.Instance.ToolsInGame[i - 1];
        if (money - t.cost < 0) //If tool is to expensive end function
            return;

        t.durability = t.maxDurability;
        money -= t.cost;
        tools.Add(t);
    }

    void Sell() //Func to sell wheat
    {
        money += Wheat * 3;
        Wheat = 0;
    }

    public int DamageOnFarm() //Used to calculate how much wheat will be picked up
    {
        int calculating = 1;
        if (Tool != null)
        {
            if (Tool.type == "Scythe")
                calculating += 2;
            calculating += Tool.level;
        }
        return calculating;
    }

    void ChangeTool() //Func to set Tool as the tool in the hotbar index and to display correct mesh
    {
        if (tools.Count > hotbarIndex)
        {
            Tool = tools[hotbarIndex];
            HandMesh.mesh = Tool.Mesh;
            HandMeshRenderer.materials = Tool.Mats.ToArray();
        }
        else
        {
            Tool = null;
            HandMesh.mesh = null;
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

        if (other.tag == "Sell")
            Sell();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Barier")
            area = null;
    }
}