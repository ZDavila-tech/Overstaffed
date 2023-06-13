using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionUiController : MonoBehaviour
{
    [SerializeField] Image potionImage;
    [SerializeField] Image potionIndicator;
    [SerializeField] PlayerController player;
    public bool selected;
    public bool used;
    //[SerializeField] PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        potionIndicator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        selected = player.items[player.itemSelected].GetComponent<Item>().isSelected;
        if (selected)
        {
            potionIndicator.enabled = true;
        }
        else
        {
            potionIndicator.enabled = false;
        }
        used = player.items[player.itemSelected].GetComponent<Item>().used;
        if (used)
        {
            potionImage.enabled = false;
        }
        else
        {
            potionImage.enabled = true;
        }
    }
}
