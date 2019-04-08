using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestOrb : MonoBehaviour
{
    public int DebugIndex = 0;

    public GameObject[] questPrefabs;
    public Transform[] questPoints;
    [SerializeField] QuestBase currentQuest;


    public Waypoint waypoint;
    public Text questComplete;

    private void Update()
    {
        if(currentQuest)
        {
            waypoint.target = currentQuest.transform;
            if (currentQuest.isComplete)
            {
                if (!currentQuest.permanentQuest) Destroy(currentQuest.gameObject);
                currentQuest = null;
                StartCoroutine(QuestComplete());
            }
        }
        else
        {
            waypoint.target = transform;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (currentQuest == null && other.CompareTag("Player") && Input.GetButtonDown("Use"))
        {
            int questIndex = (DebugIndex >= questPrefabs.Length || DebugIndex < 0) ? Random.Range(0, questPrefabs.Length) : DebugIndex;
            if (questPrefabs[questIndex].transform.parent != null)
            {
                currentQuest = questPrefabs[questIndex].GetComponent<QuestBase>();
                currentQuest.Initiate();
            }
            else
            {

                Transform point = questPoints[Random.Range(0, questPoints.Length)];
                GameObject quest = Instantiate(questPrefabs[questIndex], point.position, Quaternion.identity);
                quest.transform.SetParent(point);
                currentQuest = quest.GetComponent<QuestBase>();
                currentQuest.Initiate();
            }
        }
    }

    private void OnGUI()
    {
        if (currentQuest == null) GUI.Label(new Rect(10, 10, 300, 20), "Tip: Visit the Quest Orb for a new quest!");
    }

    IEnumerator QuestComplete()
    {
        //show the quest complete text
        questComplete.enabled = true;
        Color c = questComplete.color;
        c.a = 1.0f;
        questComplete.color = c;

        float t = 0.0f;

        yield return new WaitForSeconds(2.0f);

        while(t < 1.0f)
        {
            float alpha = Mathf.Lerp(1.0f, 0.0f, t);

            //set text alpha
            c.a = alpha;
            questComplete.color = c;

            t += Time.deltaTime;
            yield return null;
        }

        questComplete.enabled = false;
    }
}
