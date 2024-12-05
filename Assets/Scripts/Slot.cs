using UnityEngine;
using UnityEngine.UIElements;

public class Slot : MonoBehaviour
{
    private Inventory inventory;
    public int i;
    private GameObject player;

    private void Awake()
    {
        DontDestroyOnLoad(this);

    }
    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (transform.childCount <= 0)
        {
            inventory.isFull[i] = false;
        }
    }

    public void DeleteItem()
    {
        ItemButton itemButton = GetComponentInChildren<ItemButton>();
        Destroy(itemButton.gameObject);
        inventory.isFull[i] = false;
    }

    public void DropItem()
    {
        ItemButton itemButton = GetComponentInChildren<ItemButton>();

        if (itemButton != null)
        {
            if (itemButton.gameObject.name != "Ancient Rune Stone")
            {
                itemButton.StackCount--;
            }

            if (itemButton.StackCount <= 0)
            {
                if (itemButton.gameObject.name == "Ancient Rune Stone")
                {
                    inventory.isFull[i] = false;
                }
                else
                {
                    Destroy(itemButton.gameObject);
                }
            }



            foreach (Transform child in transform)
            {
                Spawn spawnComponent = child.GetComponent<Spawn>();

                print("Name of Object: " + spawnComponent.gameObject.name);

                if (spawnComponent.gameObject.name == "Damage Boost")
                {
                    player.GetComponent<PlayerController>()._enemyDamage += 1;
                }
                else if (spawnComponent.gameObject.name == "Health Potion")
                {
                    if (player.GetComponent<PlayerHealth>()._health < player.GetComponent<PlayerHealth>()._maxHealth)
                        player.GetComponent<PlayerHealth>()._health += 2;
                    player.GetComponent<PlayerHealth>().playerHealthSlider.value = player.GetComponent<PlayerHealth>()._health;
                }
                else if(spawnComponent.gameObject.name== "Speed Boost")
                {
                    player.GetComponent<PlayerController>().origninalSpeed += 1;
                }
            }
        }
    }
}

//using UnityEngine;
//using UnityEngine.UIElements;

//public class Slot : MonoBehaviour
//{
//    private Inventory inventory;
//    public int i;
//    private GameObject player;

//    private void Start()
//    {
//        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
//        player = GameObject.FindGameObjectWithTag("Player");
//    }

//    private void Update()
//    {
//        if (transform.childCount <= 0)
//        {
//            inventory.isFull[i] = false;
//        }
//    }

//    public void DeleteItem()
//    {
//        ItemButton itemButton = GetComponentInChildren<ItemButton>();
//        if (itemButton != null)
//        {
//            Destroy(itemButton.gameObject);
//            inventory.itemCounts[itemButton.name]--;
//            if (inventory.itemCounts[itemButton.name] <= 0)
//            {
//                inventory.itemCounts.Remove(itemButton.name);
//            }
//            inventory.isFull[i] = false;
//        }
//    }

//    public void DropItem()
//    {
//        ItemButton itemButton = GetComponentInChildren<ItemButton>();

//        if (itemButton != null)
//        {
//            if (itemButton.gameObject.name != "Ancient Rune Stone")
//            {
//                itemButton.StackCount--;
//            }

//            if (itemButton.StackCount <= 0)
//            {
//                if (itemButton.gameObject.name == "Ancient Rune Stone")
//                {
//                    inventory.isFull[i] = false;
//                }
//                else
//                {
//                    Destroy(itemButton.gameObject);
//                }
//            }

//            foreach (Transform child in transform)
//            {
//                Spawn spawnComponent = child.GetComponent<Spawn>();

//                print("Name of Object: " + spawnComponent.gameObject.name);

//                if (spawnComponent.gameObject.name == "Damage Boost")
//                {
//                    player.GetComponent<PlayerController>()._enemyDamage += 1;
//                }
//                else if (spawnComponent.gameObject.name == "Health Potion")
//                {
//                    if (player.GetComponent<PlayerHealth>()._health < player.GetComponent<PlayerHealth>()._maxHealth)
//                        player.GetComponent<PlayerHealth>()._health += 2;
//                }
//            }
//        }
//    }
//}

