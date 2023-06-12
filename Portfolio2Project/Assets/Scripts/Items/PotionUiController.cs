using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionUiController : MonoBehaviour
{
    [SerializeField] Image potionImage;
    [SerializeField] Image potionIndicator;
    [SerializeField] Item item;
    public bool selected;
    //[SerializeField] PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        potionIndicator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        selected = item.isSelected;
        if (item.isSelected)
        {
            potionIndicator.enabled = true;
        }
        else
        {
            potionIndicator.enabled = false;
        }

        if(item.used)
        {
            potionImage.enabled = false;
        }
        else
        {
            potionImage.enabled = true;
        }
    }
}
