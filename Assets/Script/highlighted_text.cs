using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class highlighted_text : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private Text btn_text;
    private Color original_color;

    void Start()
    {
        btn_text = GetComponentInChildren<Text>();
        original_color = btn_text.color;
    }
    
    public void OnPointerEnter (PointerEventData eventData)
    {
        btn_text.color = Color.yellow;
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        btn_text.color = original_color;
    }
}
