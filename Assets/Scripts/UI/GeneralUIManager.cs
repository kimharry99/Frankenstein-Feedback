using System.Collections;
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
    public Sprite emptyImage;
    public void UpdateInventory()
    {
        for (int i = 0; i < inventory.slotItem.Length; i++)
        {
            if (inventory.slotItem[i] != null)
                imageInventory[i].sprite = inventory.slotItem[i].itemImage;
            else
                imageInventory[i].sprite = emptyImage;
        }
    }
    public void ButtonSettingClicked()
    {
        panelSetting.SetActive(true);
    }
}
