using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_mouse : MonoBehaviour
{
    public RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        rectTransform.position = Input.mousePosition;
    }
}
