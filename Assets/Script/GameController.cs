using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameController : MonoBehaviour
{
    public const int LIFE_LENGTH = 6;
    public const int STATION_NUM = 3;
    public const int DETECTOR_NUM = 3;
    public AnimationClip clear;//清除动画
    public Gemstone gemstone;
    public int rowNum = 7; //宝石列数  
    public int columNum = 10; //宝石行数  
    public ArrayList gemstoneList; //宝石列表  
    private Gemstone currentGemstone;
    private ArrayList matchesGemstone;
    public AudioClip match3Clip;
    public AudioClip swapClip;
    public AudioClip errorClip;
    int[] array;
    public Text txt_over;
    public Text[] scoreText;
    public Button btn_next;
    public AnimationClip next;
    public AudioClip clearAudio;
    public AudioClip successAudio;

    internal void Start()
    {
        Archive archive = Archive.GetInstance();
        scoreText = GameObject.Find("txt_score_detail").GetComponentsInChildren<Text>();
        if (archive.HasArchive && !archive.HasRead)
        {
            LoadGameController(archive.Load());
        }
        else InitGameController();
    }

    public void SaveGame()
    {
        Archive archive = Archive.GetInstance();
        archive.Save();
    }

    public void DelArchive()
    {
        Archive archive = Archive.GetInstance();
        archive.Del();
    }

    public void InitGameController()
    {
        /*初始化通用变量*/
        array = new int[10];
        for (int i = 0; i < scoreText.Length; i++)
        {
            if (i == 0)
                scoreText[0].text = LIFE_LENGTH.ToString();
            else
                scoreText[i].text = "0";
        }
        btn_next.interactable = false;

        /*清除GameController下的所有子物体*/
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
            //Debug.Log("Destroy:" + transform.GetChild(i).gameObject.name);
        }

        /* 初始化游戏，生成宝石 */
        gemstoneList = new ArrayList();
        //gemstoneList.Clear();
        matchesGemstone = new ArrayList();
        //matchesGemstone.Clear();
        for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
        {
            ArrayList temp = new ArrayList();
            for (int columIndex = 0; columIndex < columNum; columIndex++)
            {
                Gemstone c = AddGemstone(rowIndex, columIndex);
                temp.Add(c);
            }
            gemstoneList.Add(temp);
        }
        // 开始检测匹配消除 
        if (CheckHorizontalMatches() || CheckVerticalMatches()) RemoveMatches();
    }

    public void LoadGameController(JObject jObject)
    {
        JArray scoreTextArr = JArray.FromObject(jObject["scoreTextArr"]);
        /*初始化通用变量*/
        array = new int[10];
        for (int i = 0; i < scoreText.Length; i++)
        {
            if (i == 0)
                scoreText[0].text = (LIFE_LENGTH - int.Parse(scoreTextArr[i].ToString())).ToString();
            else
                scoreText[i].text = scoreTextArr[i].ToString();
        }

        //JToken token = jObject.SelectToken("positionList");
        JArray positonList = JArray.FromObject(jObject["positionList"]);
        gemstoneList = new ArrayList();
        matchesGemstone = new ArrayList();
        for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
        {
            ArrayList temp = new ArrayList();
            gemstoneList.Add(temp);
        }
        foreach(JToken k in positonList)
        {
            int rowIndex = k.Value<int>("rowIndex");
            int columIndex = k.Value<int>("columIndex");
            int type = k.Value<int>("gemstoneType");
            //Gemstone c = AddGemstone(rowIndex, columIndex);
            ArrayList x = (ArrayList)gemstoneList[rowIndex];
            x.Insert(columIndex, AddGemstone(rowIndex, columIndex, type));
        }
        GameManager gameManager = (GameManager)GameObject.Find("GameController").GetComponent("GameManager");
        gameManager.gameTime = jObject.Value<float>("timeText");
        gameManager.currentScore = jObject.Value<int>("playerScore");
        // 开始检测匹配消除 
        if (CheckHorizontalMatches() || CheckVerticalMatches()) RemoveMatches();
    }


    /// <summary>
    /// 生成宝石
    /// </summary>
    /// <param name="rowIndex">行位置</param>
    /// <param name="columIndex">列位置</param>
    /// <returns></returns>
    public Gemstone AddGemstone(int rowIndex, int columIndex)
    {
        Gemstone stone = Instantiate(gemstone, transform) as Gemstone;// 生成宝石作为GameController子物体
        stone.GetComponent<Gemstone>().RandomCreateGemstoneBg();
        stone.GetComponent<Gemstone>().UpdatePosiImmi(rowIndex, columIndex);
        return stone;
    }

    public Gemstone AddGemstone(int rowIndex, int columIndex, int type)
    {
        Gemstone stone = Instantiate(gemstone, transform) as Gemstone;// 生成宝石作为GameController子物体
        stone.GetComponent<Gemstone>().CreateGemstoneBg(type);
        stone.GetComponent<Gemstone>().UpdatePosiImmi(rowIndex, columIndex);
        return stone;
    }

    // Update is called once per frame  
    internal void Update()
    {
        int count = 0;
        if (array[0] >= LIFE_LENGTH)
            gameObject.GetComponent<GameManager>().MakeGameOver();
        if(GameManager.level == 3 && array[9]>0)
            gameObject.GetComponent<GameManager>().MakeGameOver();

        for (int i = 0; i < array.Length; i++)
        {
            if (i == 0)
                continue;
            else if (array[i] >= 6)
                count++;
        }
        if (GameManager.level == 0 && count >= 5 && GameManager.success == false)
        {
            txt_over.text = "下一关";
            GameManager.success = true;
            //应该显示通关提示：声音加按钮
            AudioSource.PlayClipAtPoint(successAudio, transform.position);
            btn_next.interactable = true;
            Animator animator = btn_next.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play(next.name);
                animator.speed = 1;
            }
        }
        else if (GameManager.level == 1 && GameManager.score > 500 && GameManager.success == false)
        {
            txt_over.text = "下一关";
            GameManager.success = true;
            //应该显示通关提示：声音加按钮
            AudioSource.PlayClipAtPoint(successAudio, transform.position);
            btn_next.interactable = true;
            Animator animator = btn_next.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play(next.name);
                animator.speed = 1;
            }
        }
        else if(GameManager.level == 2 && array[9] >= STATION_NUM && GameManager.success == false)
        {
            txt_over.text = "下一关";
            GameManager.success = true;
            //应该显示通关提示：声音加按钮
            AudioSource.PlayClipAtPoint(successAudio, transform.position);
            btn_next.interactable = true;
            Animator animator = btn_next.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play(next.name);
                animator.speed = 1;
            }
        }
        else if (GameManager.level == 3 && array[8] >= DETECTOR_NUM && GameManager.success == false)
        {
            txt_over.text = "下一关";
            GameManager.success = true;
            //应该显示通关提示：声音加按钮
            AudioSource.PlayClipAtPoint(successAudio, transform.position);
            btn_next.interactable = true;
            Animator animator = btn_next.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play(next.name);
                animator.speed = 1;
            }
            //应该显示最终通关动画
        }
    }

    /// <summary>
    /// 选择宝石
    /// </summary>
    /// <param name="c"></param>
    public void Select(Gemstone c)
    {
        if (currentGemstone == null) // 没有选中任何宝石
        {
            currentGemstone = c;
            currentGemstone.isSelected = true;
            return;
        }
        else // 已经选中了宝石
        {
            if (Mathf.Abs(currentGemstone.rowIndex - c.rowIndex) + Mathf.Abs(currentGemstone.columIndex - c.columIndex) == 1) // 两颗宝石距离正确
            {
                //ExangeAndMatches(currentGemstone,c);  
                StartCoroutine(ExangeAndMatches(currentGemstone, c));//开启一个协程
            }
            else
            {
                gameObject.GetComponent<AudioSource>().PlayOneShot(errorClip);
            }
            currentGemstone.isSelected = false;
            currentGemstone = null;
        }
    }

    /// <summary>
    /// 实现宝石交换并且检测匹配消除
    /// </summary>
    /// <param name="c1">第一颗宝石</param>
    /// <param name="c2">第二颗宝石</param>
    /// <returns></returns>
    IEnumerator ExangeAndMatches(Gemstone c1, Gemstone c2)
    {
        Exchange(c1, c2);
        yield return new WaitForSeconds(0.5f);
        if (CheckHorizontalMatches() || CheckVerticalMatches())
        {
            RemoveMatches();
        }
        else
        {
            Exchange(c1, c2);
        }
    }

    /// <summary>
    /// 实现检测水平方向的匹配
    /// </summary>
    /// <returns>是否匹配</returns>
    bool CheckHorizontalMatches()
    {
        bool isMatches = false;
        for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
        {
            for (int columIndex = 0; columIndex < columNum - 2; columIndex++)
            {
                if ((GetGemstone(rowIndex, columIndex).gemstoneType == GetGemstone(rowIndex, columIndex + 1).gemstoneType)
                    && (GetGemstone(rowIndex, columIndex).gemstoneType == GetGemstone(rowIndex, columIndex + 2).gemstoneType))
                {
                    //Debug.Log ("发现行相同的宝石");  
                    AddMatches(GetGemstone(rowIndex, columIndex));
                    AddMatches(GetGemstone(rowIndex, columIndex + 1));
                    AddMatches(GetGemstone(rowIndex, columIndex + 2));
                    isMatches = true;
                }
            }
        }
        return isMatches;
    }

    /// <summary>
    /// 实现检测垂直方向的匹配
    /// </summary>
    /// <returns>是否匹配</returns>
    bool CheckVerticalMatches()
    {
        bool isMatches = false;
        for (int columIndex = 0; columIndex < columNum; columIndex++)
        {
            for (int rowIndex = 0; rowIndex < rowNum - 2; rowIndex++)
            {
                if ((GetGemstone(rowIndex, columIndex).gemstoneType == GetGemstone(rowIndex + 1, columIndex).gemstoneType) && (GetGemstone(rowIndex, columIndex).gemstoneType == GetGemstone(rowIndex + 2, columIndex).gemstoneType))
                {
                    //Debug.Log("发现列相同的宝石");  
                    AddMatches(GetGemstone(rowIndex, columIndex));
                    AddMatches(GetGemstone(rowIndex + 1, columIndex));
                    AddMatches(GetGemstone(rowIndex + 2, columIndex));
                    isMatches = true;
                }
            }
        }
        return isMatches;
    }

    void AddMatches(Gemstone c)
    {
        if (matchesGemstone == null) matchesGemstone = new ArrayList();
        int index = matchesGemstone.IndexOf(c); //检测宝石是否已在数组当中  
        if (index == -1)
        {
            matchesGemstone.Add(c);
        }
    }

    /// <summary>
    /// 删除匹配的宝石
    /// </summary>
    void RemoveMatches()
    {
        for (int i = 0; i < matchesGemstone.Count; i++)
        {
            Gemstone c = matchesGemstone[i] as Gemstone;
            //调用清除动画以及清除宝石
            AudioSource.PlayClipAtPoint(clearAudio, transform.position);
            StartCoroutine(ClearWithAnimation(c));
        }
        matchesGemstone = new ArrayList();
        StartCoroutine(WaitForCheckMatchesAgain());
    }

    /// <summary>
    /// 连续检测匹配消除
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForCheckMatchesAgain()
    {
        yield return new WaitForSeconds(clear.length);
        if (CheckHorizontalMatches() || CheckVerticalMatches())
        {
            RemoveMatches();
        }
    }

    /// <summary>
    /// 删除宝石
    /// </summary>
    /// <param name="c">要删除的宝石</param>
    void RemoveGemstone(Gemstone c)
    {
        c.Dispose();
        gameObject.GetComponent<AudioSource>().PlayOneShot(match3Clip);
        for (int i = c.rowIndex + 1; i < rowNum; i++)
        {
            Gemstone temGemstone = GetGemstone(i, c.columIndex);
            temGemstone.rowIndex--;
            SetGemstone(temGemstone.rowIndex, temGemstone.columIndex, temGemstone);
            temGemstone.UpdatePosition(temGemstone.rowIndex, temGemstone.columIndex);
        }
        Gemstone newGemstone = AddGemstone(rowNum, c.columIndex);
        newGemstone.rowIndex--;
        SetGemstone(newGemstone.rowIndex, newGemstone.columIndex, newGemstone);
        newGemstone.UpdatePosition(newGemstone.rowIndex, newGemstone.columIndex);
    }

    private IEnumerator ClearWithAnimation(Gemstone c)
    {
        //播放清除动画
        Animator animator = c.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play(clear.name);
            yield return new WaitForSeconds(clear.length);

            int type = c.gemstoneType;
            ++array[type];
            if (type == 0)
            {
                scoreText[0].text = (LIFE_LENGTH - array[0]).ToString();
            }
            else
            {
                scoreText[type].text = array[type].ToString();
            }

            RemoveGemstone(c);
            //每删除一个宝石加10分
            GameManager.score += 10;
        }
    }

    /// <summary>
    /// 通过行号和列号，获取对应位置的宝石 
    /// </summary>
    /// <param name="rowIndex">行号</param>
    /// <param name="columIndex">列号</param>
    /// <returns>宝石对象</returns>
    public Gemstone GetGemstone(int rowIndex, int columIndex)
    {
        ArrayList temp = gemstoneList[rowIndex] as ArrayList;
        Gemstone c = temp[columIndex] as Gemstone;
        return c;
    }

    public void SetGemstone(int rowIndex, int columIndex, Gemstone c)
    {//设置所对应行号和列号的宝石  
        ArrayList temp = gemstoneList[rowIndex] as ArrayList;
        temp[columIndex] = c;
    }

    /// <summary>
    /// 实现宝石交换位置
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    public void Exchange(Gemstone c1, Gemstone c2)
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(swapClip);
        SetGemstone(c1.rowIndex, c1.columIndex, c2);
        SetGemstone(c2.rowIndex, c2.columIndex, c1);
        //交换c1，c2的行号  
        int tempRowIndex;
        tempRowIndex = c1.rowIndex;
        c1.rowIndex = c2.rowIndex;
        c2.rowIndex = tempRowIndex;
        //交换c1，c2的列号  
        int tempColumIndex;
        tempColumIndex = c1.columIndex;
        c1.columIndex = c2.columIndex;
        c2.columIndex = tempColumIndex;

        c1.UpdatePosition(c1.rowIndex, c1.columIndex);
        c2.UpdatePosition(c2.rowIndex, c2.columIndex);
        //c1.TweenToPostion(c1.rowIndex, c1.columIndex);
        //c2.TweenToPostion(c2.rowIndex, c2.columIndex);
    }
}