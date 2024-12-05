using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool[] isFull;
    public GameObject[] slots;

    [SerializeField] private RectTransform _highlightedIndicator;
    private int _selected;
    internal object itemCounts;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        SetSelected(0);
    }

    public void SetSelected(int index)
    {
        _selected = index;
        _highlightedIndicator.position =
            slots[index].GetComponent<RectTransform>().position;
    }

    public Slot GetSelected()
    {
        return slots[_selected].GetComponent<Slot>();
    }
}

//using UnityEngine;
//using System.Collections.Generic;

//public class Inventory : MonoBehaviour
//{
//    public bool[] isFull;
//    public GameObject[] slots;

//    [SerializeField] private RectTransform _highlightedIndicator;
//    private int _selected;
//    public Dictionary<string, int> itemCounts = new Dictionary<string, int>(); // Corrected declaration

//    private void Start()
//    {
//        SetSelected(0);
//    }

//    public void SetSelected(int index)
//    {
//        _selected = index;
//        _highlightedIndicator.position =
//            slots[index].GetComponent<RectTransform>().position;
//    }

//    public Slot GetSelected()
//    {
//        return slots[_selected].GetComponent<Slot>();
//    }

//    public bool IsFull()
//    {
//        foreach (bool slotFull in isFull)
//        {
//            if (!slotFull)
//                return false;
//        }
//        return true;
//    }

//    public int GetSlotIndex(GameObject slot)
//    {
//        for (int i = 0; i < slots.Length; i++)
//        {
//            if (slots[i] == slot)
//                return i;
//        }
//        return -1;
//    }
//}

