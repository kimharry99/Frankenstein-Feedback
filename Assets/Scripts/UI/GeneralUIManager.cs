﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralUIManager : SingletonBehaviour<GeneralUIManager>
{
    [Header("Time UI")]
    public Text textDay;
    public Text textTime;

    [Header("Energy UI")]
    public Slider sliderEnergy;

    [Header("Durability UI")]
    public Text textDurabilty;

    [Header("Inventory")]
    public Image[] imageInventory = new Image[5];

    public GameObject panelSetting;
    public Time time;
    public FloatVariable durability;
    public IntVariable energy;
    public Inventory inventory;

    #region Unity Functions
    protected override void Awake()
    {
        base.Awake();
        // TODO inventory 와 chest private 변수로 변경
        // inventory = 
        // chest = StorageManager.Inst.chest;
        UpdateInventory();
        UpdateTextDurability();
        UpdateTextTime();
        UpdateEnergy();
    }
    #endregion
    
    public void UpdateTextTime()
    {
        textDay.text = "Day" + time.runtimeDay.ToString();
        textTime.text = time.runtimeTime.ToString() + "시";
    }

    public void UpdateTextDurability()
    {
        textDurabilty.text = durability.value.ToString() + "%";
    }

    public void UpdateEnergy()
    {
        sliderEnergy.value = energy.value;
    }

    #region Inventory
    public Sprite emptyImage;
    public void UpdateInventory()
    {
        UpdateInventoryImage();
        UpdateInventoryText();
    }

    private void UpdateInventoryImage()
    {
        for (int i = 0; i < inventory.slotItem.Length; i++)
        {
            if (inventory.slotItem[i] != null)
                imageInventory[i].sprite = inventory.slotItem[i].itemImage;
            else
                imageInventory[i].sprite = emptyImage;
        }
    }

    public void ButtonInvenItemClicked(int slotNumber)
    {
        if (HomeUIManager.Inst.panelChest.activeSelf)
        {
            Debug.Log("chest");
            StorageManager.Inst.MoveItemToChest(slotNumber);
        }
        else
        {
            Debug.Log("nothing");
        }
    }
    #endregion

    public void ButtonSettingClicked()
    {
        panelSetting.SetActive(true);

    }

    // 승윤 TODO : 메소드 구현, inventory region안에 옮겨놓기
    /// <summary>
    /// 인벤토리의 아이템 개수를 업데이트 한다. 
    /// </summary>
    private void UpdateInventoryText()
    {

    }
}
