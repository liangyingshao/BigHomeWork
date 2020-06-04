using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    //UI显示的内容
    public Text timeText;
    static public float gameTime;
    public static bool success;
    public static bool newStart = true;
    public static int score;
    public int currentScore;
    private float addScoreTime;
    public Text playerScore;
    public Button btn_back;
    public GameObject panel;
    public Text txt_over;
    public Text txt_tip;
    public GameObject img_tip;
    static public int level = 0;
    public AudioClip nextAudio;
    public AudioClip timeAudio;
    static private VideoPlayer videoPlayer;
    void Awake()
    {
        if(newStart)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
            videoPlayer.prepareCompleted += HideUI;
            videoPlayer.loopPointReached += ShowUI;
        }
        gameTime = 60;
        score = 0;
        success = false;
        currentScore = 0;
        addScoreTime = 0;
        playerScore.text = "0";
    }

    private void ShowUI(VideoPlayer source)
    {
        source.Stop();
        Destroy(videoPlayer);
        Canvas canvas = FindObjectOfType<Canvas>();
        canvas.enabled = true;
        ChangeGameSpeed(1);
        ReStartGame();
    }

    private void HideUI(VideoPlayer source)
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        canvas.enabled = false;
        ChangeGameSpeed(0);
        
    }

    // Start is called before the first frame update
    internal void Start()
    {
        panel.SetActive(false);
        if (level == 0 && newStart == true)
        {
            videoPlayer.url = "Assets/Video/2020.04.30-16.29.mp4";
            videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            videoPlayer.Prepare();
            videoPlayer.targetCamera = FindObjectOfType<Camera>();
            videoPlayer.Play();
            newStart = false;
        }
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
        StopAllCoroutines();
        SceneManager.LoadScene("Scene1");
    }
    public IEnumerator ReStartGame2()
    {
        yield return new WaitForSeconds(0.4f);
        if (success)
        {
            level++;
            if (level == 2)
            {
                videoPlayer = gameObject.AddComponent<VideoPlayer>();
                videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
                videoPlayer.url = "Assets/Video/2020.06.03-19.35.mp4";
                videoPlayer.Prepare();
                videoPlayer.targetCamera = FindObjectOfType<Camera>();
                videoPlayer.prepareCompleted += HideUI;
                videoPlayer.loopPointReached += ShowUI;
                videoPlayer.Play();
                yield return new WaitForSeconds((float)videoPlayer.clip.length);
            }
            else if (level == 3)
            {
                videoPlayer = gameObject.AddComponent<VideoPlayer>();
                videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
                videoPlayer.url = "Assets/Video/2020.06.03-19.49.mp4";
                videoPlayer.Prepare();
                videoPlayer.targetCamera = FindObjectOfType<Camera>();
                videoPlayer.prepareCompleted += HideUI;
                videoPlayer.loopPointReached += ShowUI;
                videoPlayer.Play();
                yield return new WaitForSeconds((float)videoPlayer.clip.length);
            }
        }
        ReStartGame();
    }
    
    public void ButtonOverFunc()
    {
        //gameObject.GetComponent<GameController>().InitGameController();
        //initGameManager();
        AudioSource.PlayClipAtPoint(nextAudio, transform.position);
        StartCoroutine(ReStartGame2());
    }

    public void ChangeGameSpeed(float targetSpeed)
    {
        Time.timeScale = targetSpeed;
    }

    public void ShowTip()
    {
        if (txt_tip.text.Length == 0)
        {
            switch(level)
            {
                case 0:
                    txt_tip.text = "level 1\n收集五个不同星球的图标各6个，每消去一个地球生命值减一，生命值为零则游戏失败。";break;
                case 1:
                    txt_tip.text = "level 2\n在60秒内消尽可能多的星球，分数达到500即可通关，每消去一个地球生命值减一，生命值为零则游戏失败。"; break;
                case 2:
                    txt_tip.text = "level 3\n收集6个太阳周围的能量站"; break;
                case 3:
                    txt_tip.text = "level 4\n60s内不允许消除能量站"; break;
                default:
                    break;
            }
            
            img_tip.SetActive(true);
        }
        else
        {
            txt_tip.text = "";
            img_tip.SetActive(false);
        }
    }

    // Update is called once per frame
    internal void Update()
    {
        if (Input.GetMouseButtonDown(0) && videoPlayer != null)
        {
            ShowUI(videoPlayer);
            videoPlayer = null;
        }
        if (panel.activeSelf)
        {
            return;
        }
        gameTime -= Time.deltaTime;
        if(gameTime<=0)
        {
            gameTime = 0;
            if (level == 3)
            {
                gameObject.GetComponent<GameController>().Update();
            }
            MakeGameOver();
            return;
        }
        int count = 10;
        timeText.text = gameTime.ToString("0");
        if((int)(gameTime) - count ==0)
        {
            count--;
            AudioSource.PlayClipAtPoint(timeAudio, transform.position);
        }
        if (addScoreTime <= 0.05)
        {
            addScoreTime += Time.deltaTime;
        }
        else
        {
            if (currentScore < score)
            {
                currentScore += 10;
                playerScore.text = currentScore.ToString();
                addScoreTime = 0;
            }
            else
            {
                score = currentScore;
                playerScore.text = score.ToString();
            }
        }
    }

    public void MakeGameOver()
    {
        if (success)
        {
            switch(level)
            {
                case 0:
                    txt_over.text = "生存是文明的第一要义";break;
                case 1:
                    txt_over.text = "“当一个文明发现了另一个文明，但是不清楚这个文明的状况和态度，最安全的方式就是毁掉这个文明。”" +
                        "—黑暗森林法则。";break;
                case 2:
                    txt_over.text = "在这一期间人们的应用科技有了飞跃的发展，同时面壁者雷迪亚兹在太阳系周围建立了大量的能量站，" +
                        "等待质子穿过的时候引爆能量站形成大片雾面获得质子的位置，为人们的逃亡留有时间准备。";break;
                case 3:
                    txt_over.text = "恭喜通关!";break;
            }
        }
        else
        {
            txt_over.text = "继续加油吧";
        }
        panel.SetActive(true);
    }
}
