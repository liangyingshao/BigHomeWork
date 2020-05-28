using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour
{
    public Slider mySlider;
    private int slideValue = 0;
    private void Awake()
    {
        Archive.GetInstance();
    }
    void FixedUpdate()
    {
        if (slideValue < 100)
        {
            slideValue++;
        }
        

        mySlider.value = slideValue / 100f;//实时更新滑动进度图片的fillAmount值  

        if (slideValue == 100)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
