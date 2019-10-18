using System.Collections;
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
    public EquippedItem[] equippedItem;

    #endregion

    void Start()
    {
        inv.Add(ItemData.CreateItem(0));
        inv.Add(ItemData.CreateItem(1));
        inv.Add(ItemData.CreateItem(2));
        inv.Add(ItemData.CreateItem(100));
        inv.Add(ItemData.CreateItem(101));
        inv.Add(ItemData.CreateItem(102));
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
            for (int i = 0; i < inv.Count; i++)
            {
                if (GUI.Button(new Rect(0.5f * scr.x,0.25f * scr.y + i *(0.25f * scr.y),3 * scr.x,0.25f * scr.y),inv[i].Name))
                {
                    selectedItem = inv[i];
                }              
            }
            if (selectedItem == null)
            {
                return;
            }
            else
            {
                GUI.Box(new Rect(6.5f * scr.x, 3 * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.IconName);
            }
        }
    }
}
