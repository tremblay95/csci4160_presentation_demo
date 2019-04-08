using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaQuest : QuestBase
{
    //must have QuestObject script
   
    public float questRadius = 30.0f;

    public int numObjects = 10;
    [SerializeField] int questProgress = 0;


    public override void Initiate()
    {
        numObjects = Random.Range(5, 15);
        for(int i = 0; i < numObjects;)
        {
            Vector2 spawnPoint = Random.insideUnitCircle * questRadius;
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);

            RaycastHit hit;
            if(Physics.Raycast(transform.position + new Vector3(spawnPoint.x, 20, spawnPoint.y), Vector3.down, out hit))
            {
                GameObject temp = Instantiate(questObject, hit.point + Vector3.up, rot);

                temp.transform.SetParent(transform);

                QuestObject qo = temp.GetComponent<QuestObject>();
                qo.SetQuest(this);

                i++;
            }
        }
    }

    public override void Advance()
    {
        ++questProgress;
        Debug.Log("Quest Progress: " + questProgress + "/" + numObjects);
        if (questProgress >= numObjects)
        {
            Complete();
        }
    }

    public override void Complete()
    {
        base.Complete();
        Debug.Log("Quest Complete!");
    }

    private void OnGUI()
    {
        if (!isComplete) GUI.Label(new Rect(10, 10, 300, 20), string.Format("Quest: " + questText, questProgress, numObjects));
    }
}
