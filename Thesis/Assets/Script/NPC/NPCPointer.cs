using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPointer : MonoBehaviour
{
    [Header("Image Counter Rotate")]
    [SerializeField] private RectTransform healthTransform;
    [SerializeField] private RectTransform avatarTransform;
    [SerializeField] private RectTransform emotionTransform;

    [SerializeField] private Camera UICam;

    private Vector3 targetPos;
    private Transform newbie;
    private RectTransform pointerTransform;

    void Start()
    {
        newbie = GameObject.FindGameObjectWithTag("Newbie").transform;
        pointerTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = newbie.position;
        Vector3 endPos = targetPos;
        Vector3 startPos = Camera.main.transform.position;
        startPos.z = -10f;
        Vector3 targetDir = (endPos - startPos).normalized;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        pointerTransform.localEulerAngles = new Vector3(0, 0, angle);

        // Counter Rotate
        healthTransform.localEulerAngles = new Vector3(0, 0, -angle);
        avatarTransform.localEulerAngles = new Vector3(0, 0, -angle);
        emotionTransform.localEulerAngles = new Vector3(0, 0, -angle);

        float border = 70f;
        Vector3 targetPosScreenPoint = Camera.main.WorldToScreenPoint(targetPos);
        bool isOffScreen = targetPosScreenPoint.x <= border || targetPosScreenPoint.x >= Screen.width - border || targetPosScreenPoint.y <= border || targetPosScreenPoint.y >= Screen.height - border;

        if (isOffScreen)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }

            Vector3 updateTargetScreenPos = targetPosScreenPoint;
            if (updateTargetScreenPos.x <= border) updateTargetScreenPos.x = border;
            if (updateTargetScreenPos.x >= Screen.width - border) updateTargetScreenPos.x = Screen.width - border;
            if (updateTargetScreenPos.y <= border) updateTargetScreenPos.y = border;
            if (updateTargetScreenPos.y >= Screen.height - border) updateTargetScreenPos.y = Screen.height - border;

            Vector3 pointerWorldPos = UICam.ScreenToWorldPoint(updateTargetScreenPos);
            pointerTransform.position = pointerWorldPos;
            pointerTransform.localPosition = new Vector3(pointerTransform.localPosition.x, pointerTransform.localPosition.y, -10f);
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
