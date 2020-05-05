using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class button : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;

    // Start is called before the first frame update
    void Awake()
    {
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
        if (go == button3.gameObject)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
