using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerHandler : MonoBehaviour
{
    [Header("Value Variables")]
    public float maxHealth;
    public float maxMana, maxStamina;
    public float curHealth;
    public float curMana, curStamina;
    [Header("Value Variables")]
    public Slider healthBar;
    public Slider staminaBar, manaBar;
    [Header("Damage Effect Variables")]
    public Image damageImage;
    public Image deathImage;
    public AudioClip deathClip;
    public float flashSpeed = 5;
    public Color flashColor = new Color(1, 0, 0, 0.2f);
    AudioSource playerAudio;
    public static bool isDead;
    bool damaged;

    private void Start()
    {
        // Display health
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar.value != Mathf.Clamp01(curHealth / maxHealth))
        {
            curHealth = Mathf.Clamp(curHealth, 0, maxHealth);
            healthBar.value = Mathf.Clamp01(curHealth / maxHealth);
        }
        if (healthBar.value != Mathf.Clamp01(curMana / maxMana))
        {
            curMana = Mathf.Clamp(curMana, 0, maxMana);
            manaBar.value = Mathf.Clamp01(curMana / maxMana);
        }
        if (healthBar.value != Mathf.Clamp01(curStamina / maxStamina))
        {
            curStamina = Mathf.Clamp(curStamina, 0, maxStamina);
            staminaBar.value = Mathf.Clamp01(curStamina / maxStamina);
        }
        if (curHealth <= 0 && !isDead)
        {
            Death();
        }
        // Damage op
        if (Input.GetKeyDown(KeyCode.X))
        {
            damaged = true;
            curHealth -= 5;
        }
        if(damaged && !isDead)
        {
            damageImage.color = flashColor;
            damaged = false;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }
    void Death()
    {
        // Set the death flag to this function isn't called again
        isDead = true;

        // Set the AudioSource to play the death clip
        playerAudio.clip = deathClip;
        playerAudio.Play();
        deathImage.gameObject.GetComponent<Animator>().SetTrigger("isDead");
        Invoke("Revive", 9f);
    }
    void Revive()
    {
        deathImage.gameObject.GetComponent<Animator>().SetTrigger("Alive");
    }
}
