using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButton;

    private void Awake()
    {
        DontDestroyOnLoad(this);

    }
    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ItemButton existingItemButton = FindExistingItemButton();

            if (existingItemButton != null)
            {
                if (existingItemButton.StackCount < existingItemButton.maxStackSize)
                {
                    existingItemButton.StackCount++;
                    Destroy(gameObject);
                }
                else
                {
                    AddNewItemButton();
                }
            }
            else
            {
                AddNewItemButton();
            }
        }
    }

    private ItemButton FindExistingItemButton()
    {
        foreach (GameObject slot in inventory.slots)
        {
            Transform slotTransform = slot.transform;
            ItemButton[] itemButtons = slotTransform.GetComponentsInChildren<ItemButton>();

            foreach (ItemButton itemButton in itemButtons)
            {
                if (itemButton.gameObject.name == gameObject.name && itemButton.StackCount < itemButton.maxStackSize)
                {
                    return itemButton;
                }
            }
        }

        return null;
    }

    private void AddNewItemButton()
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (!inventory.isFull[i])
            {
                inventory.isFull[i] = true;

                GameObject newItemButton = Instantiate(itemButton, inventory.slots[i].transform, false);
                ItemButton newButtonComponent = newItemButton.GetComponent<ItemButton>();
                newButtonComponent.InitializeStackCount(1);
                newButtonComponent.maxStackSize = itemButton.GetComponent<ItemButton>().maxStackSize;
                newButtonComponent.slotIndex = i;
                newItemButton.name = gameObject.name;

                Destroy(gameObject);
                break;
            }
        }
    }
}


//using Unity.VisualScripting;
//using UnityEngine;

//public class Pickup : MonoBehaviour
//{
//    private Inventory inventory;
//    public GameObject itemButton;

//    private void Start()
//    {
//        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            AddToInventory();
//        }
//    }

//    private void AddToInventory()
//    {
//        string itemName = gameObject.name;

//        if (inventory.itemCounts.ContainsKey(itemName))
//        {
//            int itemCount = inventory.itemCounts[itemName];
//            if (itemCount < itemButton.GetComponent<ItemButton>().maxStackSize)
//            {
//                inventory.itemCounts[itemName]++;
//                UpdateItemButton(itemName);
//                Destroy(gameObject);
//            }
//            // If the item's stack is already at max, do nothing
//        }
//        else
//        {
//            if (inventory.IsFull())
//            {
//                Debug.Log("Inventory is full!");
//                return;
//            }

//            inventory.itemCounts.Add(itemName, 1);
//            GameObject newItemButton = Instantiate(itemButton, GetAvailableSlot().transform, false);
//            newItemButton.GetComponent<ItemButton>().InitializeStackCount(1);
//            newItemButton.GetComponent<ItemButton>().slotIndex = inventory.GetSlotIndex(newItemButton.transform.parent.gameObject);
//            newItemButton.name = itemName;
//            Destroy(gameObject);
//        }
//    }

//    private GameObject GetAvailableSlot()
//    {
//        foreach (GameObject slot in inventory.slots)
//        {
//            if (!slot.GetComponentInChildren<ItemButton>())
//            {
//                return slot;
//            }
//        }
//        return null;
//    }

//    private void UpdateItemButton(string itemName)
//    {
//        foreach (GameObject slot in inventory.slots)
//        {
//            ItemButton itemButton = slot.GetComponentInChildren<ItemButton>();
//            if (itemButton != null && itemButton.name == itemName)
//            {
//                itemButton.StackCount++;
//                break;
//            }
//        }
//    }
//}



