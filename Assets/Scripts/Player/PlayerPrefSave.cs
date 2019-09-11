using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Normally General Game Settings like Sound
public class PlayerPrefSave : MonoBehaviour
{
    public PlayerHandler player;
    float x, y, z;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Loaded"))
        {
            PlayerPrefs.DeleteAll();
            FirstLoad();
            PlayerPrefs.SetInt("Loaded", 0);
            Save();
            // Save Binary Data
        }
        else
        {
            // Load Binary shiz
            Load();
        }
    }
    // Start is called before the first frame update
    public void Save()
    {
        PlayerSaveToBinary.SavePlayerData(player);
        /* Health, Mana, Stamina
        PlayerPrefs.SetFloat("CurHealth", player.curHealth);
        PlayerPrefs.SetFloat("CurMana", player.curMana);
        PlayerPrefs.SetFloat("CurStamina", player.curStamina);
        // Position
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);
        // Rotation
        PlayerPrefs.SetFloat("PlayerRotX", player.transform.rotation.x);
        PlayerPrefs.SetFloat("PlayerRotY", player.transform.rotation.y);
        PlayerPrefs.SetFloat("PlayerRotZ", player.transform.rotation.z);
        PlayerPrefs.SetFloat("PlayerRotW", player.transform.rotation.w);
        */
    }

    // Update is called once per frame
    public void Load()
    {
        PlayerDataToSave data = PlayerSaveToBinary.LoadData(player);
        player.name = data.playerName;
        player.curCheckPoint = GameObject.Find(data.checkPoint).GetComponent<Transform>();
        player.maxHealth = data.maxHealth;
        player.maxMana = data.maxMana;
        player.maxStamina = data.maxStamina;

        player.curHealth = data.curHealth;
        player.curMana = data.curMana;
        player.curStamina = data.curStamina;

        player.transform.position = new Vector3(data.pX, data.pY, data.pZ);
        player.transform.rotation = new Quaternion(data.rX, data.rY, data.rZ, data.rW);

        /* players current health is set to PlayerPrefs saved float called CurHealth, else set it to MaxHealth
        player.curHealth = PlayerPrefs.GetFloat("CurHealth", player.maxHealth);
        player.curMana = PlayerPrefs.GetFloat("CurMana", player.maxMana);
        player.curStamina = PlayerPrefs.GetFloat("CurStamina", player.maxStamina);
        // Position
       /* x = PlayerPrefs.GetFloat("PlayerX", 1);
        y = PlayerPrefs.GetFloat("PlayerY", 1);
        z = PlayerPrefs.GetFloat("PlayerZ", 1);

        player.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX", 29.51f), PlayerPrefs.GetFloat("PlayerY", 51f), PlayerPrefs.GetFloat("PlayerZ", 59.14f));
        player.transform.rotation = new Quaternion(PlayerPrefs.GetFloat("PlayerRotX", 0), PlayerPrefs.GetFloat("PlayerRotY", 0), PlayerPrefs.GetFloat("PlayerRotZ", 0), PlayerPrefs.GetFloat("PlayerRotW", 0));
        Debug.Log("Last player position");
        */
    }
    void FirstLoad()
    {
        player.name = "Kevin";

        player.maxHealth = 100;
        player.maxMana = 100;
        player.maxStamina = 100;
        player.curCheckPoint = GameObject.Find("First Checkpoint").GetComponent<Transform>();

        player.curHealth = player.maxHealth;
        player.curMana = player.maxMana;
        player.curStamina = player.maxStamina;

        player.transform.position = new Vector3(29.51f, 51f, 59.14f);
        player.transform.rotation = new Quaternion(0,0,0,0);
    }
}
