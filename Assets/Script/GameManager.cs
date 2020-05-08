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
    private bool gameOver = false;
    public static int score = 0;
    private int currentScore = 0;
    private float addScoreTime = 0;
    public Text playerScore;
    public Button btn_back;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //返回主面板
    public void Onclick()
    {
        SceneManager.LoadScene("Main");
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver)
        {
            return;
        }
        gameTime -= Time.deltaTime;
        if(gameTime<=0)
        {
            gameTime = 0;
            gameOver = true;
            //显示游戏结束面板（失败？失败动画？）
            //如果游戏结束不允许继续操作
            return;
        }
        timeText.text = gameTime.ToString("0");
        if(addScoreTime<=0.05)
        {
            addScoreTime += Time.deltaTime;
        }
        else
        {
            if(currentScore<score)
            {
                currentScore++;
                playerScore.text = currentScore.ToString();
                addScoreTime = 0;
            }
        }
    }
}
