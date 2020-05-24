using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //UI显示的内容
    public Text timeText;
    private float gameTime = 60;
    public static bool gameOver = false;
    public static int score = 0;
    private int currentScore = 0;
    private float addScoreTime = 0;
    public Text playerScore;
    public Button btn_back;
    public GameObject panel;
    public Text txt_tip;
    public GameObject img_tip;

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
    }

    /// <summary>
    /// 回到主菜单
    /// </summary>
    public void ToMenu()
    {
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void ReStartGame()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void ChangeGameSpeed(float targetSpeed)
    {
        Time.timeScale = targetSpeed;
    }

    public void ShowTip()
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

    // Update is called once per frame
    void Update()
    {
        if(gameOver)
        {
            //显示游戏结束面板（失败？失败动画？）
            panel.SetActive(true);
            return;
        }
        gameTime -= Time.deltaTime;
        if(gameTime<=0)
        {
            gameTime = 0;
            gameOver = true;
            return;
        }
        timeText.text = gameTime.ToString("0");
        if(addScoreTime<=0.05)
        {
            addScoreTime += Time.deltaTime;
        }
        else
        {
            if (currentScore < score)
            {
                currentScore++;
                playerScore.text = currentScore.ToString();
                addScoreTime = 0;
            }
        }
    }
}
