using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    [SerializeField] private GameObject end;//终点预制体获取
    [SerializeField] private GameObject wall;//墙壁预制体获取
    [SerializeField] private Transform _canvas;//画布获取
    [SerializeField] private GameObject player;//玩家预制体获取

    private int cols = 21;//地图列数
    private int rows = 21;//地图行数

    //面板层级管理
    private Transform mapHolder;
    private Transform outerWallHolder;

    //随机生成一个渗透的系统
    private Percolation pc;
 

    private void Start()
    {
        mapHolder = new GameObject("map_holder").transform;
        mapHolder.SetParent(_canvas);//将mapholder的父对象设为画布
        pc = new Percolation(cols-1);

        InitWall();

        InitStartPoint();

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
        GameObject startPointGo = GameObject.Instantiate(player);
        GameObject endPointGo = GameObject.Instantiate(end);
        startPointGo.transform.position = new Vector3(startX - .5f, startY - .5f);
        endPointGo.transform.position = new Vector3(endX-.5f, endY - .5f);

    }


}
