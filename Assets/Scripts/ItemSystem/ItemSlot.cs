using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 인벤토리의 InvenItemSlot에 붙어서 사용되는 컴포넌트.
/// </summary>
public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject panelUse;
    public GameObject panelDiscard;
    public GameObject panelDescription;
    public Text description;
    public Inventory inventory;
    public int indexInventory;
    private bool mouse_over = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(eventData.position);
        mouse_over = true;
        if (inventory.slotItem[indexInventory] != null && !panelDiscard.activeSelf)
        {
            panelDescription.SetActive(true);
            description.text = inventory.slotItem[indexInventory].description;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        panelDescription.SetActive(false);
        panelUse.SetActive(false);
        panelDiscard.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (inventory.slotItem[indexInventory] != null)
            {
                if (panelDiscard.activeSelf)
                {
                    panelDescription.SetActive(true);
                    panelDiscard.SetActive(false);
                }
                else
                {
                    panelDescription.SetActive(false);
                    panelDiscard.SetActive(true);
                }
                if (inventory.slotItem[indexInventory].type == Type.Consumable)
                {
                    if (panelUse.activeSelf)
                    {
                        panelUse.SetActive(false);
                    }
                    else
                    {
                        panelUse.SetActive(true);
                        Consumable consumable = (Consumable)inventory.slotItem[indexInventory];
                        if(consumable.IsConsumeEnable())
                        {
                            panelUse.transform.GetChild(0).GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            panelUse.transform.GetChild(0).GetComponent<Button>().interactable = false;
                        }
                    }
                }
            }
            else
            {
                panelDescription.SetActive(false);
                panelDiscard.SetActive(false);
                panelUse.SetActive(false);
            }
        }
        else
        {
            panelDescription.SetActive(true);
            panelDiscard.SetActive(false);
            panelUse.SetActive(false);
        }
    }

    public void ButtonUseOnClick(int indexButton)
    {
        Consumable consumable;
        consumable = (Consumable)inventory.slotItem[indexButton];
        consumable.UseItem();
        panelDiscard.SetActive(false);
        panelUse.SetActive(false);
    }

    public void ButtonDiscardClicked(int indexButton)
    {
        StorageManager.Inst.DeleteFromInven(indexButton);
        panelDiscard.SetActive(false);
        panelUse.SetActive(false);
    }
}
