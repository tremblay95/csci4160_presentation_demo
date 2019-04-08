using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float health = 100;
    bool dead = false;

    public Slider healthBar;

    void ApplyDamage(int amount)
    {
        if (dead) return;
        health -= amount;

        healthBar.value = Mathf.Max(0f, health / 100.0f);

        if(health <= 0)
        {
            dead = true;
            SendMessage("Die", SendMessageOptions.DontRequireReceiver);
            healthBar.gameObject.SetActive(false);
        }
    }
}
