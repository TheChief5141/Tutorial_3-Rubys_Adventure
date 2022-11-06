using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIHealthScript instance{get; private set;}
    public Image mask;
    float originalSize;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    // Update is called once per frame
    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}
