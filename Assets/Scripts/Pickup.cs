using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    QuestObject questObj;

    void Start()
    {
        questObj = GetComponent<QuestObject>();
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && Input.GetButtonDown("Use"))
        {
            questObj.AdvanceQuest();
            Destroy(gameObject); //or add it to your inventory
        }
    }
}
