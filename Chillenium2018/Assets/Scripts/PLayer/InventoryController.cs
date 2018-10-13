using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inven : MonoBehaviour {

    public GameObject[] InvSlotsUI = new GameObject[5];

    // Update is called once per frame
    public void UpdateSlot(int slotNum, Sprite invSprite)
    {
        //InvSlotsUI[slotNum].GetComponent<Text>().text = invName;
        InvSlotsUI[slotNum].GetComponent<Image>().sprite = TestSprite;
    }
}
