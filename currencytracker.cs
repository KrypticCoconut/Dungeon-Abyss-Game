using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class currencytracker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpdateCurrency();
    }

    // Update is called once per frame
    public void UpdateCurrency()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = livegamedata.currentdata.coins + " <sprite=0>" + livegamedata.currentdata.shards + " <sprite=1>";
    }
}
