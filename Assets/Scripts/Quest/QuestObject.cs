using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    QuestBase quest;

    public void SetQuest(QuestBase qb)
    {
        quest = qb;
    }

    public void AdvanceQuest()
    {
        quest.Advance();
    }

    public bool HasQuest()
    {
        return quest != null;
    }
}
