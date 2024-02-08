using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyText : MonoBehaviour
{
    private void Update()
    {
        GetComponent<TMP_Text>().text = PlayerController.money.ToString() + "$";
    }
}
