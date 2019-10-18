﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearInventory : MonoBehaviour
{
    #region Variables
    public static List<Item> inv = new List<Item>();
    public static bool showInv;
    public Item item, selectedItem;
    public Vector2 scr;

    public int money;
    public Vector2 scrollPos;

    public string sortType = "All";

    public Transform dropLocation;
    [System.Serializable]
    public struct EquippedItem
    {
        public string slotName;
        public Transform location;
        public GameObject equippedItem;
    }
    public EquippedItem[] equippedItems;

    #endregion

    void Start()
    {
        inv.Add(ItemData.CreateItem(0));
        inv.Add(ItemData.CreateItem(1));
        inv.Add(ItemData.CreateItem(2));
        inv.Add(ItemData.CreateItem(3));
        inv.Add(ItemData.CreateItem(100));
        inv.Add(ItemData.CreateItem(101));
        inv.Add(ItemData.CreateItem(102));
        inv.Add(ItemData.CreateItem(103));
        inv.Add(ItemData.CreateItem(200));
        inv.Add(ItemData.CreateItem(201));
        inv.Add(ItemData.CreateItem(202));
        inv.Add(ItemData.CreateItem(300));
        inv.Add(ItemData.CreateItem(301));
        inv.Add(ItemData.CreateItem(302));
        inv.Add(ItemData.CreateItem(400));
        inv.Add(ItemData.CreateItem(401));
        inv.Add(ItemData.CreateItem(402));
        inv.Add(ItemData.CreateItem(500));
        inv.Add(ItemData.CreateItem(501));
        inv.Add(ItemData.CreateItem(502));
        inv.Add(ItemData.CreateItem(600));
        inv.Add(ItemData.CreateItem(601));
        inv.Add(ItemData.CreateItem(602));
        showInv = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory") && !PauseMenu.isPaused)
        {
            showInv = !showInv;
            if (showInv)
            {
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                selectedItem = null;
            }
        }
        if (Input.GetKey(KeyCode.I))
        {
            inv.Add(ItemData.CreateItem(Random.Range(0, 3)));
        }
    }
    void OnGUI()
    {
        if (showInv)
        {
            scr = new Vector2(Screen.width / 16, Screen.height / 9);
            DisplayInv();
            if (selectedItem == null)
            {
                return;
            }
            else
            {
                UseItem();
            }
        }
    }

    void DisplayInv()
    {
        // If we have a Type selected (filter)
        if (!(sortType == "All" || sortType == ""))
        {

        }
        // All Items are shown
        else
        {
            if (inv.Count <= 34)
            {
                for (int i = 0; i < inv.Count; i++)
                {
                    if (GUI.Button(new Rect(0.5f * scr.x, 0.25f * scr.y + i * (0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name))
                    {
                        selectedItem = inv[i];
                    }
                }
            }
            else// We have more items than screen space
            {
                // Our move position of our scroll window
                scrollPos =
                // The start of our scroll view
                GUI.BeginScrollView(
                // Our position and size of our window
                new Rect(0,0.25f * scr.y,3.75f * scr.x,8.5f * scr.y),
                // Our current position in the scroll view
                scrollPos,
                // Viewable area
                new Rect(0, 0, 0, inv.Count * (0.25f * scr.y)),
                // Can we see our Horizontal bar?
                false,
                // Can we see our Vertical bar?
                true);
                #region Scrollable Space
                for (int i = 0; i < inv.Count; i++)
                {
                    if (GUI.Button(new Rect(0.5f * scr.x, i * (0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name))
                    {
                        selectedItem = inv[i];
                    }
                }
                #endregion
                GUI.EndScrollView();
            }
        }
    }

    void UseItem()
    {
        GUI.Box(new Rect(3.75f * scr.x, 0.25f * scr.y, 3 * scr.x, 0.4f * scr.y), selectedItem.Name);
        GUI.Box(new Rect(3.75f * scr.x, 0.65f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.IconName);
        GUI.Box(new Rect(3.75f * scr.x, 3.65f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.Description);
        switch (selectedItem.ItemType)
        {
            case ItemTypes.Armour:
                if(GUI.Button(new Rect(3.75f * scr.x, 6.8f * scr.y, 1.5f * scr.x, 0.25f * scr.y), "Wear"))
                {

                }
                break;
            case ItemTypes.Weapon:

                if (equippedItems[1].equippedItem == null || selectedItem.Name != equippedItems[1].equippedItem.name)
                {
                    if (GUI.Button(new Rect(3.75f * scr.x, 6.8f * scr.y, 1.5f * scr.x, 0.25f * scr.y), "Equip"))
                    {
                        if(equippedItems[1].equippedItem != null)
                        {
                            Destroy(equippedItems[1].equippedItem);
                        }
                        equippedItems[1].equippedItem = Instantiate(selectedItem.MeshName, equippedItems[1].location);
                        equippedItems[1].equippedItem.name = selectedItem.Name;
                    }
                    else
                    {
                        if (GUI.Button(new Rect(3.75f * scr.x, 6.8f * scr.y, 1.5f * scr.x, 0.25f * scr.y), "UnEquip"))
                        {
                            Destroy(equippedItems[1].equippedItem);
                            equippedItems[1].equippedItem = null;
                        }
                    }
                }
                break;
            case ItemTypes.Potion:
                if (GUI.Button(new Rect(3.75f * scr.x, 6.8f * scr.y, 1.5f * scr.x, 0.25f * scr.y), "Drink"))
                {

                }
                break;
            case ItemTypes.Food:
                if (GUI.Button(new Rect(3.75f * scr.x, 6.8f * scr.y, 1.5f * scr.x, 0.25f * scr.y), "Eat"))
                {

                }
                break;
            case ItemTypes.Ingredient:
                if (GUI.Button(new Rect(3.75f * scr.x, 6.8f * scr.y, 1.5f * scr.x, 0.25f * scr.y), "Use"))
                {

                }
                break;
            case ItemTypes.Craftable:
                if (GUI.Button(new Rect(3.75f * scr.x, 6.8f * scr.y, 1.5f * scr.x, 0.25f * scr.y), "Use"))
                {

                }
                break;
            default:
                break;
        }
        if (GUI.Button(new Rect(5.25f * scr.x, 6.8f * scr.y, 1.5f * scr.x, 0.25f * scr.y), "Discard"))
        {
            // Checks if the item is equipped
            for (int i = 0; i < equippedItems.Length; i++)
            {
                if (equippedItems[i].equippedItem != null && selectedItem.Name == equippedItems[i].equippedItem.name)
                {
                    // If so, destroy from scene
                    Destroy(equippedItems[i].equippedItem);
                }
            }
            // Spawn item at droplocation
            GameObject itemToDrop = Instantiate(selectedItem.MeshName, dropLocation.position, Quaternion.identity);
            // Apply gravity and make sure its named correctly
            itemToDrop.name = selectedItem.Name;
            itemToDrop.AddComponent<Rigidbody>().useGravity = true;
            // If so reduce from list
            if(selectedItem.Amount > 1)
            {
                selectedItem.Amount--;
            }
            // else remove from list
            else
            {
                inv.Remove(selectedItem);
                selectedItem = null;
                return;
            }
        }
    }
}
