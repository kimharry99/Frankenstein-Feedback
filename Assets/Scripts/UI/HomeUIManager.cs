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
    public GameObject panelNotice;

    [Header("Inventory")]
    public Image[] imageInventory;

    [Header("Disassemble UI")]
    public Image[] slotDisassembleUsing;
    public Image[] imageDisassembleUsing;
    public Image[] imageDisassembleHolding;
    public GameObject[] imageCheck;
    public Text textDisassembleEnergy;
    public int[] indexHoldingChest;
    public int[] indexUsingHolding;

    [Header("Notice")]
    public Text textNotice;

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
        sliderEnergy.value = energy.runtimeValue;
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
                imageCheck[j].SetActive(false);
                if (j < 6)
                {
                    imageDisassembleUsing[j].sprite = emptyImage;
                    indexUsingHolding[j] = -1;
                }
                indexHoldingChest[j++] = i;
            }

            if (j > 8)
                break;
        }

        while (j <= 8)
        {
            imageDisassembleHolding[j].sprite = emptyImage;
            imageCheck[j].SetActive(false);
            if (j < 6)
            {
                imageDisassembleUsing[j].sprite = emptyImage;
                indexUsingHolding[j] = -1;
            }
            indexHoldingChest[j++] = -1;
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

    public void PanelNoticeClicked()
    {
        panelNotice.SetActive(false);
    }

    #region DisassemblePanel methods
    public void DisassembleButtonHoldingClicked(int slotNumber)
    {
        if (imageCheck[slotNumber].activeSelf == false && imageDisassembleHolding[slotNumber].sprite != emptyImage)
        {
            int i = 0;
            while (i < 6 && imageDisassembleUsing[i].sprite != emptyImage)
                i++;

            if (i < 6)
            {
                imageCheck[slotNumber].SetActive(true);

                indexUsingHolding[i] = slotNumber;
                imageDisassembleUsing[i].sprite = chest.slotItem[indexHoldingChest[slotNumber]].itemImage;
                disassembleEnergy += chest.slotItem[indexHoldingChest[slotNumber]].energyPotential;
                textDisassembleEnergy.text = "추출 에너지 [ " + disassembleEnergy.ToString() + " ]";
            }
            else
            {
                panelNotice.SetActive(true);
                textNotice.text = "분해 슬롯이 가득 찼습니다.";
            }
        }

        else if (imageCheck[slotNumber].activeSelf == true)
        {
            imageCheck[slotNumber].SetActive(false);

            int i = 0;
            for (; i < 6; i++)
            {
                if (indexUsingHolding[i] == slotNumber)
                    break;
            }

            indexUsingHolding[i] = -1;
            imageDisassembleUsing[i].sprite = emptyImage;
            disassembleEnergy -= chest.slotItem[indexHoldingChest[slotNumber]].energyPotential;
            textDisassembleEnergy.text = "추출 에너지 [ " + disassembleEnergy.ToString() + " ]";
        }
    }

    public void DisassembleButtonUsingClicked(int slotNumber)
    {
        if (imageDisassembleUsing[slotNumber].sprite != emptyImage)
        {
            int indexHolding = indexUsingHolding[slotNumber];

            imageCheck[indexHolding].SetActive(false);

            indexUsingHolding[slotNumber] = -1;
            imageDisassembleUsing[slotNumber].sprite = emptyImage;
            disassembleEnergy -= chest.slotItem[indexHoldingChest[indexHolding]].energyPotential;
            textDisassembleEnergy.text = "추출 에너지 [ " + disassembleEnergy.ToString() + " ]";
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
