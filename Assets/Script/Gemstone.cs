using System.Collections;
using UnityEngine;

public class Gemstone : MonoBehaviour
{
    public float xOffset; //x方向的偏移  
    public float yOffset; //y方向的偏移  
    public int rowIndex = 0;
    public int columIndex = 0;
    public GameObject[] gemstoneBgs; //宝石预制体数组  
    public int gemstoneType; //宝石类型  
    private GameObject gemstoneBg;
    private GameController gameController;
    //private SpriteRenderer spriteRenderer;
    public bool isSelected;
    // Use this for initialization  
    internal void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        //spriteRenderer = gemstoneBg.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame  
    internal void Update()
    {
        transform.position = Vector2.Lerp(transform.position, new Vector2(columIndex + xOffset, rowIndex + yOffset), 0.1F);
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
    }

    public void UpdatePosiImmi(int _rowIndex, int _columIndex)
    {
        rowIndex = _rowIndex;
        columIndex = _columIndex;
        transform.position = new Vector2(columIndex + xOffset, rowIndex + yOffset);
    }

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
        gameController = null;
        Destroy(gameObject);
        Destroy(gemstoneBg.gameObject);
    }
}