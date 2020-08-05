using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeUIManager : SingletonBehaviour<HomeUIManager>
{
    [Header("Time UI")]
    public Text textDay;
    public Text textTime;

    [Header("Energy UI")]
    public Slider sliderEnergy;

    [Header("Durability UI")]
    public Text textDurabilty;

    [Header("Sub Panels")]
    public GameObject panelSetting;
    public GameObject panelHome;
    public GameObject panelExploration;
    public GameObject panelResearch;
    public GameObject panelDisassemble;
    public GameObject panelCrafting;
    public GameObject panelAssemble;
    public GameObject panelChest;

    [Header("Inventory")]
    public Image[] imageInventory;

    [Header("Disassemble UI")]
    public GameObject[] slotDisassembleUsing;
    public Image[] imageDisassembleUsing;
    public GameObject[] slotDisassembleHolding;
    public Image[] imageDisassembleHolding;
    

    public Time time;
    public IntVariable durability;
    public IntVariable energy;
    public Inventory inventory;
    public Chest chest;
    #region Unity Functions
    protected override void Awake()
    {
        base.Awake();
    }
    #endregion
    // Update textTime and textDay
    public void UpdateTextTime()
    {
        textDay.text = "Day" + time.runtimeDay.ToString();
        textTime.text = time.runtimeTime.ToString() + "시";
    }

    public void UpdateTextDurability()
    {
        textDurabilty.text = durability.runtimeValue.ToString() + "%";
    }

    public void UpdateEnergy()
    {
        // to implement
    }
    public Sprite emptyImage;
    public void UpdateInventory()
    {
        for(int i=0;i<inventory.slotItem.Length;i++)
        {
            if (inventory.slotItem[i] != null)
                imageInventory[i].sprite = inventory.slotItem[i].itemImage;
            else
                imageInventory[i].sprite = emptyImage;
        }
    }
    public void UpdateChest(int constraint)
    {

    }
    // for debugging
    public void ClickedSendTime()
    {
        GameManager.Inst.OnTurnOver(1);
    }

    #region HomePanel methods
    public void ButtonSettingClicked()
    {
        panelHome.SetActive(false);
        panelSetting.SetActive(true);
    }

    public void ButtonExploreClicked()
    {
        GameManager.Inst.StartExploration();
    }

    public void ButtonResearchClicked()
    {
        panelHome.SetActive(false);
        panelResearch.SetActive(true);
    }
    public void ButtonDisassembleClicked()
    {
        panelHome.SetActive(false);
        panelDisassemble.SetActive(true);

        for (int i = 0; i < chest.slotItem.Length; i++)
        {
            if (chest.slotItem[i] != null && chest.slotItem[i].type == 0)
                imageDisassembleHolding[i].sprite = chest.slotItem[i].itemImage;
            else
                imageDisassembleHolding[i].sprite = emptyImage;
        }
    }

    public void ButtonCraftClicked()
    {
        panelHome.SetActive(false);
        panelCrafting.SetActive(true);
    }

    public void ButtonAssembleClicked()
    {
        panelHome.SetActive(false);
        panelAssemble.SetActive(true);
    }

    public void ButtonChestClicked()
    {
        panelHome.SetActive(false);
        panelChest.SetActive(true);
    }
    #endregion

    // Parameter panel means a panel to be closed.
    public void ButtonCloseClicked(GameObject panel)
    {
        panel.SetActive(false);
        panelHome.SetActive(true);
    }

    public Sprite noneImage;
    public Sprite checkImage;
    #region DisassemblePanel methods
    public void DisassembleButtonItemClicked(int i)
    {
        Image imgslt = slotDisassembleHolding[i].GetComponent<Image>();
        Image imgimg = imageDisassembleHolding[i].GetComponent<Image>();

        if (imgslt.sprite == noneImage && imgimg.sprite != emptyImage)
        {
            imgslt.sprite = checkImage;

            int j = 0;
            while (imageDisassembleUsing[j].sprite != emptyImage)
                j++;
            if (j < 6)
                imageDisassembleUsing[j].sprite = imgimg.sprite;
            else
                Debug.Log("Full");
        }

        else if (imgslt.sprite == checkImage)
        {
            //should complete
        }
    }

    public void DisassembleButtonCreateClicked()
    {
        print("ButtonCreateClicked");
    }
    #endregion

    #region CraftingPanel methods
    public void CraftingButtonItemClicked()
    {
        print("ButtonItemClicked");
    }

    public void CraftingButtonCreateClicked()
    {
        print("ButtonCreateClicked");
    }
    #endregion

    #region AssemblePanel methods
    public void ButtonDeadBodyClicked()
    {
        print("ButtonDeadBodyClicked");
    }

    public void ButtonEquipClicked()
    {
        print("ButtonEquipClicked");
    }
    #endregion
}
