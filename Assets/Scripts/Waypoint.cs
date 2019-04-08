using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waypoint : MonoBehaviour
{

    public Transform target;
    Image sprite;
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
        sprite = GetComponent<Image>();
    }


    private void Update()
    {
        if (!target) return;
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
        sprite.rectTransform.position = screenPos;
        sprite.enabled = (screenPos.z > 0);
    }
}
