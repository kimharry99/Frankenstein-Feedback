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
    public Image[] slotDisassembleUsing;
    public Image[] imageDisassembleUsing;
    public Image[] slotDisassembleHolding;
    public Image[] imageDisassembleHolding;
    public Text textDisassembleEnergy;
    public int[] itemIndex;

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
    public Sprite noneImage;
    public Sprite checkImage;
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

    public int disassembleEnergy = 0;
    public void UpdateDisassemble()
    {
        disassembleEnergy = 0;

        int i = 0, j = 0;
        for (; i < chest.slotItem.Length; i++)
        {
            if (chest.slotItem[i] != null && chest.slotItem[i].type == 0)
            {
                imageDisassembleHolding[j].sprite = chest.slotItem[i].itemImage;
                slotDisassembleHolding[j].sprite = noneImage;
                if (j < 6)
                    imageDisassembleUsing[j].sprite = emptyImage;
                itemIndex[j++] = i;
            }

            if (j > 8)
                break;
        }

        while (j <= 8)
        {
            imageDisassembleHolding[j].sprite = emptyImage;
            slotDisassembleHolding[j].sprite = noneImage;
            if (j < 6)
                imageDisassembleUsing[j].sprite = emptyImage;
            itemIndex[j++] = -1;
        }

        textDisassembleEnergy.text = "추출 에너지 [ " + disassembleEnergy.ToString() + " ]";
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

        UpdateDisassemble();
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

    #region DisassemblePanel methods
    public void DisassembleButtonItemClicked(int slotNumber)
    {
        Image imgslot = slotDisassembleHolding[slotNumber];
        Image imgitem = imageDisassembleHolding[slotNumber];

        if (imgslot.sprite == noneImage && imgitem.sprite != emptyImage)
        {
            int i = 0;
            while (i < 6 && imageDisassembleUsing[i].sprite != emptyImage)
                i++;

            if (i < 6)
            {
                imgslot.sprite = checkImage;

                imageDisassembleUsing[i].sprite = chest.slotItem[itemIndex[slotNumber]].itemImage;
                disassembleEnergy += chest.slotItem[itemIndex[slotNumber]].energyPotential;
                textDisassembleEnergy.text = "추출 에너지 [ " + disassembleEnergy.ToString() + " ]";
            }
            else
                Debug.Log("Full");
        }

        else if (imgslot.sprite == checkImage)
        {
            // TODO
        }
    }

    public void DisassembleButtonCreateClicked()
    {
        GameManager.Inst.DisassembleItem();
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
