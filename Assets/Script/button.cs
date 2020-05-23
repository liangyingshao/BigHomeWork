using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class button : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    public Text txt_tip;
    public GameObject img_tip;

    // Start is called before the first frame update
    void Awake()
    {
        img_tip.SetActive(false);
        txt_tip.text = "";
        button1.onClick.AddListener(delegate () {
            OnClick(button1.gameObject);
        });

        button2.onClick.AddListener(delegate () {
            OnClick(button2.gameObject);
        });

        button3.onClick.AddListener(delegate () {
            OnClick(button3.gameObject);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickTest()
    {
        print("ChinarOnClickTest");
    }

    public void OnClick(GameObject go)
    {
        Debug.Log("test");
        if(go == button2.gameObject)
        {
            if (txt_tip.text.Length == 0)
            {
                txt_tip.text = "生存是文明的第一要义：收集五个不同星球的图标各6个，每消去一个地球生命值减一，生命值为零则游戏失败。";
                img_tip.SetActive(true);
            }
            else
            {
                txt_tip.text = "";
                img_tip.SetActive(false);
            }
        }
        if (go == button3.gameObject)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
