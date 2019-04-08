using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleQuest : QuestBase
{

    public override void Initiate()
    {
        isComplete = false;
        if (permanentQuest)
        {
            transform.position = questObject.transform.position;
            transform.SetParent(questObject.transform);

            QuestObject qo = questObject.GetComponent<QuestObject>();
            qo.SetQuest(this);

            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 20, Vector3.down, out hit))
        {
            GameObject temp = Instantiate(questObject, hit.point + Vector3.up, Quaternion.identity);

            temp.transform.SetParent(transform);

            QuestObject qo = temp.GetComponent<QuestObject>();
            qo.SetQuest(this);
        }
    }

    public override void Advance()
    {
        isComplete = true;
        Complete();
    }

    public override void Complete()
    {
        base.Complete();
        Debug.Log("Quest Complete!");
    }

    private void OnGUI()
    {
        if(!isComplete) GUI.Label(new Rect(10, 10, 300, 20), "Quest: " + questText);
    }
}
