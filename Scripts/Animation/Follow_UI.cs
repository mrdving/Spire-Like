using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_UI : MonoBehaviour
{
    public float followSpeed;
    public RectTransform followTarget;
    private RectTransform thisTransform;
    public bool startFollow = true;
    public bool snap = false;

    private void Start()
    {
        thisTransform = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        if (!startFollow) return;
        if (snap) thisTransform.anchoredPosition = followTarget.anchoredPosition;
        else thisTransform.anchoredPosition = Vector2.MoveTowards(thisTransform.anchoredPosition, followTarget.anchoredPosition, followSpeed * Time.deltaTime);   
    }
}
