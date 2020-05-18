using System.Collections;
using UnityEngine;

public class Gemstone : MonoBehaviour
{
    public float xOffset = -60f; //x方向的偏移  
    public float yOffset = -2.0f; //y方向的偏移  
    public int rowIndex = 0;
    public int columIndex = 0;
    public GameObject[] gemstoneBgs; //宝石预制体数组  
    public int gemstoneType; //宝石类型  
    private GameObject gemstoneBg;
    private GameController gameController;
    private SpriteRenderer spriteRenderer;
    public bool isSelected
    {
        set
        {
            if (value)
            {
                spriteRenderer.color = Color.red;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
    }
    // Use this for initialization  
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        spriteRenderer = gemstoneBg.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame  
    void Update()
    {

    }

    /// <summary>
    /// 更新宝石的位置
    /// </summary>
    /// <param name="_rowIndex"></param>
    /// <param name="_columIndex"></param>
    public void UpdatePosition(int _rowIndex, int _columIndex)
    { 
        rowIndex = _rowIndex;
        columIndex = _columIndex;
        this.transform.position = new Vector3(columIndex + xOffset, rowIndex + yOffset, 0);
    }

    /// <summary>
    /// 调用iTween插件实现宝石滑动效果  
    /// </summary>
    /// <param name="_rowIndex"></param>
    /// <param name="_columIndex"></param>
    /*public void TweenToPostion(int _rowIndex, int _columIndex)
    { 
        rowIndex = _rowIndex;
        columIndex = _columIndex;
        iTween.MoveTo(this.gameObject, iTween.Hash("x", columIndex + xOffset, "y", rowIndex + yOffset, "time", 0.5f));
    }*/

    /// <summary>
    /// 生成随机宝石类型
    /// </summary>
    public void RandomCreateGemstoneBg()
    { 
        if (gemstoneBg != null)return;
        gemstoneType = Random.Range(0, gemstoneBgs.Length);
        gemstoneBg = Instantiate(gemstoneBgs[gemstoneType]) as GameObject;
        gemstoneBg.transform.parent = this.transform;
    }

    public void OnMouseDown()
    {
        gameController.Select(this);
    }

    public void Dispose()
    {
        Destroy(this.gameObject);
        Destroy(gemstoneBg.gameObject);
        gameController = null;
    }
}