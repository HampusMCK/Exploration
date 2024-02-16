using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public GameObject slot;
    PlayerController player;

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
                    slot.GetComponent<Image>().enabled = true;
                    slot.GetComponent<Image>().sprite = player.tools[x].HotbarTexture;
                }
                else
                    slot.GetComponent<Image>().enabled = false;
    }

}