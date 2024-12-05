using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ItemButton : MonoBehaviour, IPointerDownHandler
{
    public TextMeshProUGUI stackText;
    public int maxStackSize = 1;
    private int stackCount = 1; public int slotIndex;

    public int StackCount
    {
        get { return stackCount; }
        set
        {
            stackCount = value;
            stackText.text = stackCount.ToString();
        }
    }

    public void InitializeStackCount(int count)
    {
        stackCount = count;
        //stackText.text = stackCount.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        FindObjectOfType<Inventory>().SetSelected(slotIndex);
    }

    public void RemoveOne()
    {
        StackCount--;
        if (stackCount <= 0)
        {
            FindObjectOfType<Inventory>().slots[slotIndex].GetComponent<Slot>().DeleteItem();
        }
    }
}

//using UnityEngine;
//using TMPro;
//using UnityEngine.EventSystems;

//public class ItemButton : MonoBehaviour, IPointerDownHandler
//{
//    public TextMeshProUGUI stackText;
//    public int maxStackSize = 1;
//    public int stackCount = 1;
//    public int slotIndex;

//    public int StackCount
//    {
//        get { return stackCount; }
//        set
//        {
//            stackCount = value;
//            stackText.text = stackCount.ToString();
//        }
//    }

//    public void InitializeStackCount(int count)
//    {
//        stackCount = count;
//        stackText.text = stackCount.ToString();
//    }

//    public void OnPointerDown(PointerEventData eventData)
//    {
//        FindObjectOfType<Inventory>().SetSelected(slotIndex);
//    }
//}

