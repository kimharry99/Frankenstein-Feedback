using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeUIManager : SingletonBehaviour<HomeUIManager>
{
    [Header("Sub Panels")]
    public GameObject panelHome;
    public GameObject panelResearch;
    public GameObject panelDisassemble;
    public GameObject panelCrafting;
    public GameObject panelAssemble;
    public GameObject panelChest;
    public GameObject panelNotice;

    [Header("Inventory")]
    public Button[] imageChestSlot = new Button[Chest.CAPACITY];
    public Sprite emptyImage;

    public Chest chest;

    [Header("Disassemble UI")]
    public Image[] imageDisassembleUsing;
    public Image[] imageDisassembleHolding;
    public GameObject[] imageCheck;
    public Text textDisassembleEnergy;
    public Scrollbar scrollbarDisassemble;
    public int[] indexHoldingChest;
    public int[] indexUsingHolding;

    [Header("Assemble UI")]
    public Image imageAssembleUsing;
    public GameObject[] buttonAssembleHolding;
    public Scrollbar scrollbarAssemble;

    [Header("Notice")]
    public Text textNotice;

    #region chest methods
    //public void UpdateChestSlot(int slotNumber)
    //{
    //    if (chest.slotItem[slotNumber] != null)
    //        imageChestSlot[slotNumber].image.sprite = chest.slotItem[slotNumber].itemImage;
    //    else
    //        imageChestSlot[slotNumber].image.sprite = emptyImage;
    //}

    /// <summary>
    /// 창고 정렬, 창고 패널 표시에서 호출되는 함수이다. 
    /// </summary>
    public void UpdateChest()
    {
        UpdateChestImage();
        UpdateChestText();
    }

    /// <summary>
    /// 창고 패널의 이미지를 업데이트한다.
    /// TODO : If, else 깔끔하게
    /// </summary>
    private void UpdateChestImage()
    {
        for(int i=0;i<imageChestSlot.Length;i++)
        {
            int indexItem = StorageManager.Inst.GetIndexTable(_currentChestType, i);
            if (indexItem != -1)
            {
                if (chest.slotItem[indexItem] != null)
                {
                    imageChestSlot[i].image.sprite = chest.slotItem[indexItem].itemImage;
                    continue;
                }
            }
            imageChestSlot[i].image.sprite = null;
        }
    }

    private Type _currentChestType = Type.All;
    public void ButtonChestCategoryClicked(int intItemType)
    {
        _currentChestType = (Type)intItemType;
        HighlightCategoryButton();
        UpdateChest();

    }
    public void ButtonChestSlotClidked(int uiSlot)
    {
        int itemSlot = StorageManager.Inst.GetIndexTable(_currentChestType, uiSlot);
        StorageManager.Inst.MoveItemToInven(itemSlot);
    }

    //public void InitialUpdateChest()
    //{
    //    chest = StorageManager.Inst.chests[0];
    //    for (int i = 0; i < chest.slotItem.Length; i++)
    //    {
    //        if (chest.slotItem[i] != null)
    //            imageChestSlot[i].image.sprite = chest.slotItem[i].itemImage;
    //        else
    //            imageChestSlot[i].image.sprite = emptyImage;
    //    }
    //}
    //public void SortItem(int num)
    //{
    //    chest = StorageManager.Inst.chests[num];
    //    for (int i = 0; i < chest.slotItem.Length; i++)
    //    {
    //        if (chest.slotItem[i] != null)
    //            imageChestSlot[i].image.sprite = chest.slotItem[i].itemImage;
    //        else
    //            imageChestSlot[i].image.sprite = emptyImage;
    //    }
    //}
    // 승윤 TODO : 메소드 구현, chest mehtods region안에 옮겨놓기
    /// <summary>
    /// 창고의 아이템 개수를 Update한다.
    /// </summary>
    private void UpdateChestText()
    {
        Text txt;
        for(int i = 0; i < Chest.CAPACITY; i++)
        {
            int indexItem = StorageManager.Inst.GetIndexTable(_currentChestType, i);
            if (indexItem != -1)
            {
                if (chest.slotItem[indexItem] != null)
                {
                    txt = panelChest.transform.GetChild(1).GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>();
                    txt.text = chest.slotItemNumber[indexItem].ToString();
                }
                else
                {
                    txt = panelChest.transform.GetChild(1).GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>();
                    txt.text = "";
                }
            }
        }
    }

    // 승윤 TODO : 메소드 구현, chest methods region안에 옮겨놓기
    /// <summary>
    /// 창고가 어떤 카테고리의 아이템을 display하고 있는지 표시하기 위해 카테고리 버튼의 색을 변경한다.
    /// </summary>
    private void HighlightCategoryButton()
    {
        Image selectorButton;
        for (int i = 0; i < 5; i++)
        {
            selectorButton = panelChest.transform.GetChild(0).GetChild(i).GetComponent<Image>();
            selectorButton.color = Color.white;
        }
        selectorButton = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        selectorButton.color = Color.grey;
    }
    #endregion

  

    #region HomePanel methods

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
        scrollbarDisassemble.value = 1;
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
        GameManager.Inst.bodyAssembly.HoldBodyPartsFromChest();
        UpdateBodyAssemblyHoldingImages();
        scrollbarAssemble.value = 1;
    }

    public void ButtonChestClicked()
    {
        panelHome.SetActive(false);
        panelChest.SetActive(true);
        UpdateChest();
    } 
    // Parameter panel means a panel to be closed.
    public void ButtonCloseClicked(GameObject panel)
    {
        panel.SetActive(false);
        panelHome.SetActive(true);
    }

    #endregion


    public void PanelNoticeClicked()
    {
        panelNotice.SetActive(false);
    }

    #region DisassemblePanel methods
    public int disassembleEnergy = 0;
    public void UpdateDisassemble()
    {
        int indexHoldingItem = 0;
        indexHoldingItem = UpdateImageBodyPart();
        ResetRemainingSlots(indexHoldingItem);

        disassembleEnergy = 0;
        textDisassembleEnergy.text = "추출 에너지 [ " + disassembleEnergy.ToString() + " ]";
    }
    private int UpdateImageBodyPart()
    {
        int indexItem = 0, indexHoldingItem = 0;

        for (; indexItem < chest.slotItem.Length; indexItem++)
        {
            if (chest.slotItem[indexItem] != null && chest.slotItem[indexItem].type == Type.BodyPart)
            {
                imageDisassembleHolding[indexHoldingItem].sprite = chest.slotItem[indexItem].itemImage; // Update Corpse at the jth Holding Slot
                indexHoldingChest[indexHoldingItem] = indexItem; // Record the Index of Corpse (Holding Slot to Chest Slot)

                imageCheck[indexHoldingItem].SetActive(false);

                if (indexHoldingItem < 6) // Reset the jth Using Slot
                {
                    imageDisassembleUsing[indexHoldingItem].sprite = emptyImage;
                    indexUsingHolding[indexHoldingItem] = -1;
                }

                indexHoldingItem++;
            }

            if (indexHoldingItem > 29) // Prevent the Overflow (Now It's not Needed)
                break;
        }
        return indexHoldingItem;
    }

    private void ResetRemainingSlots(int indexHoldingItem)
    {
        while (indexHoldingItem < chest.slotItem.Length) // Reset the Remaining Slots
        {
            imageDisassembleHolding[indexHoldingItem].sprite = emptyImage;
            indexHoldingChest[indexHoldingItem] = -1;

            imageCheck[indexHoldingItem].SetActive(false);

            if (indexHoldingItem < 6)
            {
                imageDisassembleUsing[indexHoldingItem].sprite = emptyImage;
                indexUsingHolding[indexHoldingItem] = -1;
            }

            indexHoldingItem++;
        }
    }


    public void DisassembleButtonHoldingClicked(int slotNumber)
    {
        if (imageCheck[slotNumber].activeSelf == false && imageDisassembleHolding[slotNumber].sprite != emptyImage) // The Case that Slot not been Selected Before
        {
            int i = 0;
            while (i < 6 && imageDisassembleUsing[i].sprite != emptyImage) // Find Empty Slot in 'Panel Using Items'
                i++;

            if (i < 6)
            {
                imageCheck[slotNumber].SetActive(true); // Mark the Selected Slot

                imageDisassembleUsing[i].sprite = chest.slotItem[indexHoldingChest[slotNumber]].itemImage; // Update Corpse at the Using Slot
                indexUsingHolding[i] = slotNumber; // Record the Index of Corpse (Using Slot to Holding Slot)

                disassembleEnergy += chest.slotItem[indexHoldingChest[slotNumber]].energyPotential;
                textDisassembleEnergy.text = "추출 에너지 [ " + disassembleEnergy.ToString() + " ]";

                Debug.Log(slotNumber + "select;" +
                          "    +" + chest.slotItem[indexHoldingChest[slotNumber]].energyPotential + "energy" + "" +
                          "\ntotal:" + disassembleEnergy.ToString());
            }

            else
            {
                panelNotice.SetActive(true);
                textNotice.text = "분해 슬롯이 가득 찼습니다.";
            }
        }

        else if (imageCheck[slotNumber].activeSelf == true) // The Case that Slot has been Selected Before
        {
            imageCheck[slotNumber].SetActive(false); // Unmark the Selected Slot


            int i = 0;
            for (; i < 6; i++) // Find the Corpse's Index in Using Slot
            {
                if (indexUsingHolding[i] == slotNumber)
                    break;
            }

            imageDisassembleUsing[i].sprite = emptyImage; // Remove Corpse in the Using Slot
            indexUsingHolding[i] = -1; // Initialize

            disassembleEnergy -= chest.slotItem[indexHoldingChest[slotNumber]].energyPotential;
            textDisassembleEnergy.text = "추출 에너지 [ " + disassembleEnergy.ToString() + " ]";

            Debug.Log(slotNumber + "select cancel;" +
                      "    -" + chest.slotItem[indexHoldingChest[slotNumber]].energyPotential + "energy" +
                      "\ntotal:" + disassembleEnergy.ToString());
        }
    }

    public void DisassembleButtonUsingClicked(int slotNumber)
    {
        if (imageDisassembleUsing[slotNumber].sprite != emptyImage)
        {
            int indexHolding = indexUsingHolding[slotNumber];

            imageCheck[indexHolding].SetActive(false); // Unmark the Holding Slot where Selected Corpse is

            imageDisassembleUsing[slotNumber].sprite = emptyImage; // Remove Corpse in the Using Slot
            indexUsingHolding[slotNumber] = -1; // Initialize

            disassembleEnergy -= chest.slotItem[indexHoldingChest[indexHolding]].energyPotential;
            textDisassembleEnergy.text = "추출 에너지 [ " + disassembleEnergy.ToString() + " ]";
        }
    }

    public void DisassembleButtonCreateClicked()
    {
        GameManager.Inst.bodyDisassembly.DisassembleItem();
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
    // 진웅 TODO : 메소드 구현, chest methods의 UpdateChestImage 참고
    /// <summary>
    /// chest에서 BodyPart에 해당하는 아이템의 이미지를 BodyAssemble 패널에 업데이트한다.
    /// </summary>
    public void UpdateBodyAssemblyHoldingImages() // 완료
    {
        for (int i = 0; i < Chest.CAPACITY; i++)
        {
            int indexItem = StorageManager.Inst.GetIndexTable(Type.BodyPart, i);
            Image imageAssembleHolding = buttonAssembleHolding[i].transform.GetChild(0).GetComponent<Image>();
            if (indexItem != -1)
            {
                imageAssembleHolding.sprite = chest.slotItem[indexItem].itemImage;
                continue;
            }
            imageAssembleHolding.sprite = null;
        }

        imageAssembleUsing.sprite = null;
        buttonAssembleHolding[_lastSlotNumber].transform.GetChild(1).gameObject.SetActive(false);
        _lastSlotNumber = 0;
    }

    // 진웅 TODO : 메소드 구현, image check를 사용하거나, slot의 이미지를 흐리게 표현
    private int _lastSlotNumber = 0;
    /// <summary>
    /// slotNumber번째의 slot의 아이템이 선택되었는지 표시한다. 
    /// 동시에 다른 slot의 아이템이 선택 해제되었음을 표시한다.
    /// </summary>
    /// <param name="slotNumber"></param>
    private void DisplayIsSelected(int slotNumber) // 완료
    {
        imageAssembleUsing.sprite = buttonAssembleHolding[slotNumber].transform.GetChild(0).GetComponent<Image>().sprite;
        buttonAssembleHolding[_lastSlotNumber].transform.GetChild(1).gameObject.SetActive(false);
        if (imageAssembleUsing.sprite != null)
            buttonAssembleHolding[slotNumber].transform.GetChild(1).gameObject.SetActive(true);
        _lastSlotNumber = slotNumber;
    }

    public void ButtonAssemblyHoldingItemClicked(int slotNumber)
    {
        DisplayIsSelected(slotNumber);
        GameManager.Inst.bodyAssembly.SelectBodyPart(slotNumber);
    }

    // TODO : 이름변경해야 함
    public void ButtonDoAssembleyClicked()
    {
        if (imageAssembleUsing.sprite != null)
            GameManager.Inst.bodyAssembly.AssemleBody();
        else
        {
            panelNotice.SetActive(true);
            textNotice.text = "장착할 사체를 선택하세요.";
        }
    }
    #endregion
}
