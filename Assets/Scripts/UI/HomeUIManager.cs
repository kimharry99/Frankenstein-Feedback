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
    public GameObject panelBlackOut;

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
    public int[] indexHoldingChest = new int[Chest.CAPACITY];
    public int[] indexUsingHolding = new int[BodyDisassembly.ITEM_CAPACITY];

    [Header("Crafting UI")]
    public GameObject[] buttonCraftUsing;
    public GameObject[] buttonCraftHolding;
    public Text textCraftEnergy;
    public Scrollbar scrollbarCraft;

    [Header("Assemble UI")]
    public Image imageAssembleUsing;
    public GameObject[] buttonAssembleHolding;
    public Text textAssembleEnergy;
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
        for (int i = 0; i < imageChestSlot.Length; i++)
        {
            UpdateChestSlotImage(i);
        }
    }

    private void UpdateChestSlotImage(int iImageChestSlot)
    {
        int indexItem = StorageManager.Inst.GetIndexFromTable(_currentChestType, iImageChestSlot);
        if (chest.IsSotrageSlotNotNull(indexItem))
            imageChestSlot[iImageChestSlot].image.sprite = chest.slotItem[indexItem].itemImage;
        else
            imageChestSlot[iImageChestSlot].image.sprite = null;
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
        int itemSlot = StorageManager.Inst.GetIndexFromTable(_currentChestType, uiSlot);
        Item item = chest.slotItem[itemSlot];
        Inventory inven = StorageManager.Inst.inventory;
        if(inven.CanMoveItemToStorage(item))
            StorageManager.Inst.MoveItemToInven(itemSlot);
    }

    // 승윤 TODO : 메소드 구현, chest mehtods region안에 옮겨놓기
    /// <summary>
    /// 창고의 아이템 개수를 Update한다.
    /// </summary>
    private void UpdateChestText()
    {
        Text txt;

        for(int i = 0; i < chest.Capacity; i++)
        {
            Transform chestSlot = GetChestSlotTransform(i);
            Transform slotButton = chestSlot.GetChild(0);
            txt = slotButton.GetChild(0).GetComponent<Text>();

            int indexItem = StorageManager.Inst.GetIndexFromTable(_currentChestType, i);
            if (chest.IsSotrageSlotNotNull(indexItem))
                txt.text = chest.slotItemNumber[indexItem].ToString();
            else
                txt.text = "";
        }
    }

    private Transform GetChestSlotTransform(int index)
    {
        Transform viewPort = panelChest.transform.GetChild(2);
        return viewPort.GetChild(0).GetChild(index);
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
        UpdateResearchPanel();
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

        UpdateCrafting();
        scrollbarCraft.value = 1;
    }

    public void ButtonAssembleClicked()
    {
        panelHome.SetActive(false);
        panelAssemble.SetActive(true);
        GameManager.Inst.bodyAssembly.HoldBodyPartsFromChest();
        UpdateAssembleEnergy(0);
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
    /// <summary>
    /// 분해 패널에 창고의 사체 아이템을 업데이트 한다.
    /// </summary>
    public void UpdateDisassemble()
    {
        DisableAllCheckImages();
        UpdateImageBodyPart();
        ResetUsingPanel();

        disassembleEnergy = 0;
        UpdateDiasmEnergyTxt(textDisassembleEnergy, disassembleEnergy);
    }

    /// <summary>
    /// holding panel에 창고의 모든 사체이미지를 업데이트 한다.
    /// </summary>
    private void UpdateImageBodyPart()
    {
        int indexItem = 0, indexHoldingItem = 0;

        for (; indexItem < chest.Capacity; indexItem++)
        {
            if (indexItem < StorageManager.Inst.GetNumOfItemsByType(Type.BodyPart))
            {
                if (chest.IsSotrageSlotNotNull(indexItem))
                {
                    Item item = chest.slotItem[indexItem];
                    if (item.type == Type.BodyPart)
                    {
                        imageDisassembleHolding[indexHoldingItem++].sprite = item.itemImage;
                    }
                }
            }
            else
            {
                imageDisassembleHolding[indexHoldingItem++].sprite = emptyImage;
            }
        }
    }

    private void DisableAllCheckImages()
    {
        for(int i=0;i<imageCheck.Length;i++)
        {
            imageCheck[i].SetActive(false);
        }
    }

    /// <summary>
    /// holding panel의 아이템이 업로드 되지 않은 slot을 초기화 한다.
    /// </summary>
    /// <param name="indexHoldingItem">holding된 아이템의 개수</param>
    private void ResetUsingPanel()
    {
        for(int i=0;i< BodyDisassembly.ITEM_CAPACITY;i++)
        {
            imageDisassembleUsing[i].sprite = emptyImage;
            indexUsingHolding[i] = -1;
        }
    }

    private void UpdateDiasmEnergyTxt(Text energyRewardText, int energy)
    {
        energyRewardText.text = "추출 에너지 [ " + energy.ToString() + " ]";
    }

    public void DisassembleButtonHoldingClicked(int slotHoldingNum)
    {
        if (imageDisassembleHolding[slotHoldingNum].sprite == emptyImage)
            return;

        if (imageCheck[slotHoldingNum].activeSelf == false) // The Case that Slot not been Selected Before
        {
            int slotUsingNum = GetEmptyUsingSlot();
            
            if (slotUsingNum != -1)
            {
                EnableHoldingItem(slotHoldingNum, slotUsingNum);
            }
            else
            {
                panelNotice.SetActive(true);
                textNotice.text = "분해 슬롯이 가득 찼습니다.";
            }
        }
        else // The Case that Slot has been Selected Before
        {
            DisableHoldingItem(slotHoldingNum);
        }
    }

    private void EnableHoldingItem(int slotHoldingNum, int slotUsingNum)
    {
        imageCheck[slotHoldingNum].SetActive(true); // Mark the Selected Slot

        int indexItem = StorageManager.Inst.GetIndexFromTable(Type.BodyPart, slotHoldingNum);

        Item clickedItem = chest.slotItem[indexItem];
        imageDisassembleUsing[slotUsingNum].sprite = clickedItem.itemImage; // Update Corpse at the Using Slot
        indexUsingHolding[slotUsingNum] = slotHoldingNum; // Record the Index of Corpse (Using Slot to Holding Slot)

        disassembleEnergy += clickedItem.energyPotential;
        UpdateDiasmEnergyTxt(textDisassembleEnergy, disassembleEnergy);

        Debug.Log(slotHoldingNum + "select;" +
                  "    +" + clickedItem.energyPotential + "energy" + "" +
                  "\ntotal:" + disassembleEnergy.ToString());
    }

    private int GetEmptyUsingSlot()
    {
        int i = 0;
        while (i < 6 && imageDisassembleUsing[i].sprite != emptyImage) // Find Empty Slot in 'Panel Using Items'
            i++;
        if (i < 6)
            return i;
        else
            return -1;
    }

    private void DisableHoldingItem(int slotHoldingNum)
    {
        imageCheck[slotHoldingNum].SetActive(false); // Unmark the Selected Slot

        int slotUsingNum = GetIndexFromUsingPanel(slotHoldingNum);

        imageDisassembleUsing[slotUsingNum].sprite = emptyImage; // Remove Corpse in the Using Slot
        indexUsingHolding[slotUsingNum] = -1; // Initialize

        int indexItem = StorageManager.Inst.GetIndexFromTable(Type.BodyPart, slotHoldingNum);
        Item clickedItem = chest.slotItem[indexItem];
        disassembleEnergy -= clickedItem.energyPotential;
        UpdateDiasmEnergyTxt(textDisassembleEnergy, disassembleEnergy);

        Debug.Log(slotHoldingNum + "select cancel;" +
                  "    -" + chest.slotItem[indexItem].energyPotential + "energy" +
                  "\ntotal:" + disassembleEnergy.ToString());
    }

    private int GetIndexFromUsingPanel(int slotHoldingNum)
    {
        int i = 0;
        for (; i < 6; i++) // Find the Corpse's Index in Using Slot
        {
            if (indexUsingHolding[i] == slotHoldingNum)
                break;
        }
        return i;
    }

    public void DisassembleButtonUsingClicked(int slotNumber)
    {
        if (imageDisassembleUsing[slotNumber].sprite != emptyImage)
        {
            int indexHolding = indexUsingHolding[slotNumber];

            imageCheck[indexHolding].SetActive(false); // Unmark the Holding Slot where Selected Corpse is

            imageDisassembleUsing[slotNumber].sprite = emptyImage; // Remove Corpse in the Using Slot
            indexUsingHolding[slotNumber] = -1; // Initialize

            int indexItem = StorageManager.Inst.GetIndexFromTable(Type.BodyPart, indexHolding);
            disassembleEnergy -= chest.slotItem[indexItem].energyPotential;
            textDisassembleEnergy.text = "추출 에너지 [ " + disassembleEnergy.ToString() + " ]";
        }
    }

    public void DisassembleButtonCreateClicked()
    {
        GameManager.Inst.bodyDisassembly.DisassembleItem();
    }
    #endregion

    #region CraftingPanel methods

    public int craftEnergy = 0;
    public int[] itemHoldingCount = new int[Chest.CAPACITY];
    public int[] itemUsingCount = new int[6];

    public void UpdateCrafting()
    {
        int indexHoldingItem = 0;
        indexHoldingItem = UpdateImageItem();
        ResetRemainingCraftSlots(indexHoldingItem);
        UpdateItemCount();

        craftEnergy = 0;
        textCraftEnergy.text = "필요 에너지 [ " + craftEnergy.ToString() + " ]";
    }

    /// <summary>
    /// 창고의 아이템 목록을 갱신한다.
    /// </summary>
    /// <returns></returns>
    private int UpdateImageItem()
    {
        int indexItem = 0, indexHoldingItem = 0;

        for (; indexItem < chest.slotItem.Length; indexItem++)
        {
            if (chest.slotItem[indexItem] != null && chest.slotItem[indexItem].type == Type.Ingredient)
            {
                Image imageCraftHolding = buttonCraftHolding[indexHoldingItem].transform.GetChild(0).GetComponent<Image>();
                imageCraftHolding.sprite = chest.slotItem[indexItem].itemImage; // Update Item
                indexHoldingChest[indexHoldingItem] = indexItem; // Record the Index of Item (Holding Slot to Chest Slot)

                if (indexHoldingItem < 6) // Reset the Using Slot
                {
                    Image imageCraftUsing = buttonCraftUsing[indexHoldingItem].transform.GetChild(0).GetComponent<Image>();
                    imageCraftUsing.sprite = emptyImage;
                    indexUsingHolding[indexHoldingItem] = -1;
                    GameManager.Inst.craftingTable.SetIndexUsingChest(indexHoldingItem, -1);
                }

                indexHoldingItem++;
            }

            if (indexHoldingItem >= Chest.CAPACITY) // Prevent the Overflow (Now It's not Needed)
                break;
        }
        return indexHoldingItem;
    }

    private void ResetRemainingCraftSlots(int indexHoldingItem)
    {
        while (indexHoldingItem < chest.slotItem.Length) // Reset the Remaining Slots
        {
            Image imageCraftHolding = buttonCraftHolding[indexHoldingItem].transform.GetChild(0).GetComponent<Image>();
            imageCraftHolding.sprite = emptyImage;
            indexHoldingChest[indexHoldingItem] = -1;

            if (indexHoldingItem < 6)
            {
                Image imageCraftUsing = buttonCraftUsing[indexHoldingItem].transform.GetChild(0).GetComponent<Image>();
                imageCraftUsing.sprite = emptyImage;
                indexUsingHolding[indexHoldingItem] = -1;
                GameManager.Inst.craftingTable.SetIndexUsingChest(indexHoldingItem, -1);
            }

            indexHoldingItem++;
        }
    }


    private void UpdateItemCount()
    {
        for (int i = 0; i < Chest.CAPACITY; i++)
        {
            if (indexHoldingChest[i] != -1)
                itemHoldingCount[i] = chest.slotItemNumber[indexHoldingChest[i]];

            else
                itemHoldingCount[i] = 0;

            Text textHoldingCount = buttonCraftHolding[i].transform.GetChild(1).GetComponent<Text>();
            textHoldingCount.text = itemHoldingCount[i].ToString();

            if (i < 6)
            {
                itemUsingCount[i] = 0;
                GameManager.Inst.craftingTable.SetUsingItemCount(i, 0);
                Text textUsingCount = buttonCraftUsing[i].transform.GetChild(1).GetComponent<Text>();
                textUsingCount.text = itemUsingCount[i].ToString();
            }
        }
    }

    public void CraftButtonHoldingClicked(int slotNumber)
    {
        if (itemHoldingCount[slotNumber] > 0) // If Clicked Slot is not Empty Slot
        {
            int i = 0;
            for (; i < 6; i++)
            {
                if (indexUsingHolding[i] == slotNumber)
                    break;
            }

            if (i < 6)
            {
                itemHoldingCount[slotNumber]--;
                Text textHoldingCount = buttonCraftHolding[slotNumber].transform.GetChild(1).GetComponent<Text>();
                textHoldingCount.text = itemHoldingCount[slotNumber].ToString();

                itemUsingCount[i]++;
                GameManager.Inst.craftingTable.AddUsingItemCount(i, 1);
                Text textUsingCount = buttonCraftUsing[i].transform.GetChild(1).GetComponent<Text>();
                textUsingCount.text = itemUsingCount[i].ToString();

                Item resultItem = GameManager.Inst.craftingTable.FindRecipie();
                if (resultItem)
                    craftEnergy = resultItem.energyPotential;
                else
                    craftEnergy = 0;
                textCraftEnergy.text = "필요 에너지 [ " + craftEnergy.ToString() + " ]";

                Debug.Log(slotNumber + "select;" +
                          "    +" + chest.slotItem[indexHoldingChest[slotNumber]].energyPotential + "energy" + "" +
                          "\ntotal:" + craftEnergy.ToString());
            }

            else
            {
                for (i = 0; i < 6; i++)
                {
                    if (itemUsingCount[i] == 0)
                        break;
                }

                if (i < 6)
                {
                    itemHoldingCount[slotNumber]--;
                    Text textHoldingCount = buttonCraftHolding[slotNumber].transform.GetChild(1).GetComponent<Text>();
                    textHoldingCount.text = itemHoldingCount[slotNumber].ToString();

                    itemUsingCount[i]++;
                    GameManager.Inst.craftingTable.AddUsingItemCount(i, 1);
                    Text textUsingCount = buttonCraftUsing[i].transform.GetChild(1).GetComponent<Text>();
                    textUsingCount.text = itemUsingCount[i].ToString();

                    Image imageCraftUsing = buttonCraftUsing[i].transform.GetChild(0).GetComponent<Image>();
                    imageCraftUsing.sprite = chest.slotItem[indexHoldingChest[slotNumber]].itemImage; // Update Item at the Using Slot
                    indexUsingHolding[i] = slotNumber; // Record the Index of Item (Using Slot to Holding Slot)
                    GameManager.Inst.craftingTable.SetIndexUsingChest(i, indexHoldingChest[slotNumber]);

                    Item resultItem = GameManager.Inst.craftingTable.FindRecipie();
                    if (resultItem)
                        craftEnergy = resultItem.energyPotential;
                    else
                        craftEnergy = 0;
                    textCraftEnergy.text = "필요 에너지 [ " + craftEnergy.ToString() + " ]";

                    Debug.Log(slotNumber + "select;" +
                              "    +" + chest.slotItem[indexHoldingChest[slotNumber]].energyPotential + "energy" + "" +
                              "\ntotal:" + craftEnergy.ToString());
                }

                else
                {
                    panelNotice.SetActive(true);
                    textNotice.text = "제작 슬롯이 가득 찼습니다.";
                }
            }
        }
    }

    public void CraftButtonUsingClicked(int slotNumber)
    {
        if (itemUsingCount[slotNumber] > 0)
        {
            int indexHolding = indexUsingHolding[slotNumber];

            itemUsingCount[slotNumber]--;
            GameManager.Inst.craftingTable.AddUsingItemCount(slotNumber, -1);
            Text textUsingCount = buttonCraftUsing[slotNumber].transform.GetChild(1).GetComponent<Text>();
            textUsingCount.text = itemUsingCount[slotNumber].ToString();

            if (itemUsingCount[slotNumber] == 0)
            {
                Image imageCraftUsing = buttonCraftUsing[slotNumber].transform.GetChild(0).GetComponent<Image>();
                imageCraftUsing.sprite = emptyImage; // Remove Item in the Using Slot
                indexUsingHolding[slotNumber] = -1; // Initialize
                GameManager.Inst.craftingTable.SetIndexUsingChest(slotNumber, -1);
            }

            itemHoldingCount[indexHolding]++;
            Text textHoldingCount = buttonCraftHolding[indexHolding].transform.GetChild(1).GetComponent<Text>();
            textHoldingCount.text = itemHoldingCount[indexHolding].ToString();

            Item resultItem = GameManager.Inst.craftingTable.FindRecipie();
            if (resultItem)
                craftEnergy = resultItem.energyPotential;
            else
                craftEnergy = 0;
            textCraftEnergy.text = "필요 에너지 [ " + craftEnergy.ToString() + " ]";
        }
    }
    
    public void CraftButtonCreateClicked()
    {
        int i = 0;
        for (; i < 6; i++)
        {
            if (itemUsingCount[i] > 0)
                break;
        }

        if (i == 6)
        {
            panelNotice.SetActive(true);
            textNotice.text = "제작에 사용할 아이템을 선택하세요.";
        }
        else
            GameManager.Inst.craftingTable.CraftItem();
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
            int indexItem = StorageManager.Inst.GetIndexFromTable(Type.BodyPart, i);
            Image imageAssembleHolding = buttonAssembleHolding[i].transform.GetChild(0).GetComponent<Image>();
            if (indexItem != -1)
            {
                imageAssembleHolding.sprite = chest.slotItem[indexItem].itemImage;
                continue;
            }
            imageAssembleHolding.sprite = emptyImage;
        }

        imageAssembleUsing.sprite = emptyImage;
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
        if (imageAssembleUsing.sprite != emptyImage)
            buttonAssembleHolding[slotNumber].transform.GetChild(1).gameObject.SetActive(true);
        _lastSlotNumber = slotNumber;
    }

    public void ButtonAssemblyHoldingItemClicked(int slotNumber)
    {
        DisplayIsSelected(slotNumber);
        GameManager.Inst.bodyAssembly.SelectBodyPart(slotNumber);
    }

    public void UpdateAssembleEnergy(int energyValue)
    {
        textAssembleEnergy.text = "필요 에너지 [ " + energyValue.ToString() + " ]";
    }

    // TODO : 이름변경해야 함
    public void ButtonDoAssembleyClicked()
    {
        if (imageAssembleUsing.sprite != emptyImage)
            GameManager.Inst.bodyAssembly.AssemleBody();
        else
        {
            panelNotice.SetActive(true);
            textNotice.text = "장착할 사체를 선택하세요.";
        }
    }

    #endregion

    [Header("Research Panel Field")]
    public GameObject panelResearchProgress;
    private int _selectedIndex = -1;
    public Text[] progressTexts = new Text[4];
    public Text requireEnergy;
    #region ResearchPanel methods
    public void UpdateResearchPanel()
    {
        Research research = GameManager.Inst.research;
        progressTexts[0].text = "진행률 " + (research.GoblinLevel * 10).ToString() + "%";
        progressTexts[1].text = "진행률 " + (research.ElfLevel * 10).ToString() + "%";
        progressTexts[2].text = "진행률 " + (research.OakLevel* 10).ToString() + "%";
        progressTexts[3].text = "진행률 " + (research.MachineLevel * 10).ToString() + "%";

    }
    public void ResearchIconClick(int index)
    {
        if (progressTexts[index].text != "진행률 100%")
        {
            panelResearchProgress.SetActive(true);
            _selectedIndex = index;
            requireEnergy.text = GameManager.Inst.research.ResearchCost(_selectedIndex).ToString();
        }
    }

    public void ResearchProcessClicked()
    {
        Research research = GameManager.Inst.research;
        if (GameManager.Inst.energy.value >= research.ResearchCost(_selectedIndex))
        {
            Debug.Log("연구 성공");
            GameManager.Inst.research.ResearchRace(_selectedIndex);
            UpdateResearchPanel();
        }
        panelResearchProgress.SetActive(false);
    }
    #endregion

    /// <summary>
    /// 수면에 대한 에너지 변화를 안내한다.
    /// </summary>
    public void RecordResultsForSleep(GameManager.SleepResultInfo sleepResultInfo)
    {
        textNotice.text = "에너지를 " + sleepResultInfo.SpendEnergy + "소모하여 내구도를 " + Mathf.Ceil(sleepResultInfo.RegenedDurabilty*10)/10 
            + "." + Mathf.Ceil(sleepResultInfo.RegenedDurabilty * 10) % 10 + "%\n 회복했습니다.";
    }

    public void RecordResultsForOverwork(GameManager.OverworkResultInfo overworkResultInfo)
    {
        if (overworkResultInfo.IsOverwork)
        {
            textNotice.text += "\n\n수면시간이 " + overworkResultInfo.Time + "시간 부족하여 내구도가 " + Mathf.Ceil(overworkResultInfo.OverworkPenalty) / 10 + "% 감소되었습니다.";
        }
    }


    public void ButtonDebugClicked()
    {
        Debug.Log("atk:" + Player.Inst.Atk + "  def:" + Player.Inst.Def + "   dex:" + Player.Inst.Dex + "   mana:" + Player.Inst.Mana + "   endurance:" + Player.Inst.Endurance);
        //Debug.Log("2번 아이테 사용");
        //if (StorageManager.Inst.inventory.slotItem[1].type == Type.Consumable)
        //{
        //    var consumable = (Consumable)StorageManager.Inst.inventory.slotItem[1];
        //    if (!consumable.IsConsumeEnable())
        //        return;
        //    consumable.UseItem();
        //    StorageManager.Inst.DeleteFromInven(1);
        //}
    }
    // for debugging
    public void ButtonSetTimePanelClicked(GameObject panel)
    {
        panel.SetActive(true);
    }

    /// <summary>
    /// for debugging, 가장 가까운 time 시각 까지 게임을 진행시킨다.
    /// </summary>
    /// <param name="time"></param>
    public void ButtonSetTimeClicked(int time)
    {
        if(GeneralUIManager.Inst.time.runtimeTime < time)
        {
            GameManager.Inst.OnTurnOver(time - GeneralUIManager.Inst.time.runtimeTime);
        }
        else
        {
            GameManager.Inst.OnTurnOver(24 + time - GeneralUIManager.Inst.time.runtimeTime);
        }
    }

    private bool _isBlackOut = false;
    private float _blackOutTime = 3.0f;
    /// <summary>
    /// 수면에 대한 UI효과와 알림을 출력한다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator PutToSleep(GameManager.SleepResultInfo sleepResultInfo, GameManager.OverworkResultInfo overworkResultInfo)
    {
        Debug.Log("black out start");
        panelBlackOut.SetActive(true);
        _isBlackOut = true;
        yield return new WaitForSeconds(_blackOutTime + 0.5f);

        GeneralUIManager.Inst.UpdateTextDurability();
        GeneralUIManager.Inst.UpdateTextTime();
        GeneralUIManager.Inst.UpdateEnergy();
        
        Debug.Log("안내 패널 출력");
        panelNotice.SetActive(true);
        RecordResultsForSleep(sleepResultInfo);
        RecordResultsForOverwork(overworkResultInfo);

        _isBlackOut = false;
        panelBlackOut.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panelBlackOut.SetActive(false);
        Debug.Log("black out end");
    }

    private void Update()
    {
        if(_isBlackOut)
        {
            if(panelBlackOut.activeSelf)
            {
                panelBlackOut.GetComponent<Image>().color = new Color(0, 0, 0, panelBlackOut.GetComponent<Image>().color.a + UnityEngine.Time.deltaTime / _blackOutTime);
            }
        }
    }


}
