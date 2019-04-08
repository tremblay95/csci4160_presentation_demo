using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCChat : MonoBehaviour
{
    QuestObject questObj;

    void Start()
    {
        questObj = GetComponent<QuestObject>();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetButtonDown("Use"))
        {
            if (questObj.HasQuest())
            {
                questObj.AdvanceQuest();
                //show quest dialog
            }
            else
            {
                //show random npc dialog
            }
            
        }
    }
}
