using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public GameObject slot;
    public GameObject durability;
    PlayerController player;
    Tools tool = null;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        for (int x = 0; x < player.HotbarSlots.Count; x++)
            if (player.HotbarSlots[x] == gameObject)
                if (player.tools.Count > x)
                {
                    tool = player.tools[x];
                    slot.GetComponent<Image>().enabled = true;
                    slot.GetComponent<Image>().sprite = player.tools[x].HotbarTexture;
                }
                else
                {
                    slot.GetComponent<Image>().enabled = false;
                    tool = null;
                }

        if (tool != null)
        {
            durability.SetActive(true);
            durability.GetComponent<Slider>().value = tool.durability / tool.maxDurability;
            if (durability.GetComponent<Slider>().value > 0.66f)
                durability.GetComponentInChildren<Image>().color = new Color(0, 255, 0, 255);
            
            if (durability.GetComponent<Slider>().value < 0.66f && durability.GetComponent<Slider>().value > 0.33f)
                durability.GetComponentInChildren<Image>().color = new Color(255, 150, 0, 255);

            if (durability.GetComponent<Slider>().value < 0.33f && durability.GetComponent<Slider>().value > 0)
                durability.GetComponentInChildren<Image>().color = new Color(255, 0, 0, 255);
        }
        else
            durability.SetActive(false);
    }

}