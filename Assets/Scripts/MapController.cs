using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    public ManagerStatus status { get; private set; }

    [SerializeField] private GameObject end;//终点预制体获取
    [SerializeField] private GameObject wall;//墙壁预制体获取
    [SerializeField] private Transform _canvas;//画布获取
    [SerializeField] private GameObject player;//玩家预制体获取
    [SerializeField] private GameObject enemy;//敌人预制体获取
    [SerializeField] private GameObject chest;//宝箱预制体获取

    private int cols = 21;//地图列数
    private int rows = 21;//地图行数

    private GameObject startPointGo;

    private int enemyNum;//敌人数量
    private int chestNum;//宝箱数量

    //面板层级管理
    private Transform mapHolder;
    private Transform outerWallHolder;

    //随机生成一个渗透的系统
    private Percolation pc;
 

    public void Start()
    {

        mapHolder = new GameObject("map_holder").transform;
        mapHolder.SetParent(_canvas);//将mapholder的父对象设为画布
        pc = new Percolation(cols-1);
        enemyNum = Managers.Mission.curLevel;

        InitWall();

        InitStartPoint();

        InitEnemy();

        InitChest();

        status = ManagerStatus.Started;

    }



    /// <summary>
    /// 初始化墙壁
    /// </summary>
    private void InitWall()
    {
        while (!pc.percolates())
        {
            int row = Random.Range(1, rows);
            int col = Random.Range(1, cols);
            pc.open(row, col);
        }
        for(int x = 1; x < cols; x++)
        {
            for(int y = 1; y < rows; y++)
            {
                if(!pc.isOpen(y, x))
                {
                    GameObject wallGo = GameObject.Instantiate(wall);
                    wallGo.transform.position = new Vector3(x-.5f,y-.5f);
                    wallGo.transform.SetParent(mapHolder);
                }
            }
        }
        
    }

    /// <summary>
    /// 初始化起点和终点
    /// </summary>
    private void InitStartPoint()
    {
        int startX = 1;
        int startY = 1;
        int endX = 1;
        int endY = 20;
        for (int i = 1; i < cols; i++)
        {
            for(int j= 1; j < cols; j++)
            {
                startX = i;
                endX = j;
                if (pc.isConnected(startY, startX, endY, endX)) break;
            }
            if (pc.isConnected(startY, startX, endY, endX)) break;

        }
        startPointGo = GameObject.Instantiate(player);
        GameObject endPointGo = GameObject.Instantiate(end);
        startPointGo.transform.position = new Vector3(startX - .5f, startY - .5f);
        endPointGo.transform.position = new Vector3(endX-.5f, endY - .5f);

    }

    ///<summary>
    ///初始化敌人
    ///</summary>
    private void InitEnemy()
    {
        //四周为开放的点，初始化敌人
        for(int i = 1;i<cols;i++)
        {
            for(int j = 1;j < rows; j++)
            {
                if (enemyNum <= 0) return;
                int col = i;
                int row = j;
                if (pc._isOpen(row, col))
                {
                    if (pc._isOpen(row + 1, col) && pc._isOpen(row-1,col) && pc._isOpen(row, col + 1) && pc._isOpen(row, col - 1))
                    {
                        GameObject enemyGo = GameObject.Instantiate(enemy);
                        enemyGo.transform.position = new Vector3(col - .5f, row - .5f);
                        enemyNum--;
                    }
                }
            }
        }
    }

    ///<summary>
    ///初始化宝箱
    ///</summary>
    private void InitChest()
    {
        //三面有墙的点，初始化宝箱
        chestNum = Random.Range(1, 2);
        for (int i = cols-1; i>0; i--)
        {
            for (int j = rows-1; j >0; j--)
            {
                if (chestNum <= 0) return;
                int col = i;
                int row = j;
                if (pc._isOpen(row, col))
                {
                    if (pc.isConer(row,col))
                    {
                        GameObject chestGo = GameObject.Instantiate(chest);
                        chestGo.transform.position = new Vector3(col - .5f, row - .5f);
                        chestNum--;
                    }
                }
            }
        }
    }


}
