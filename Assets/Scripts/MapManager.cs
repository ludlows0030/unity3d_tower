using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    [SerializeField] private GameObject end;//终点预制体获取
    [SerializeField] private GameObject wall;//墙壁预制体获取
    [SerializeField] private Transform _canvas;//画布获取

    private int cols = 21;//地图列数
    private int rows = 21;//地图行数

    //面板层级管理
    private Transform mapHolder;
    private Transform outerWallHolder;
   

    private void Start()
    {
        mapHolder = new GameObject("map_holder").transform;
        mapHolder.SetParent(_canvas);//将mapholder的父对象设为画布

        InitWall();

        InitEnd();
    }


    /// <summary>
    /// 初始化终点
    /// </summary>
    private void InitEnd()
    {
        GameObject endGo = GameObject.Instantiate(end);
        //endGo.GetComponent<RectTransform>().position = new Vector3(cols - 5, rows - 5);
        endGo.transform.SetParent(mapHolder);
    }

    /// <summary>
    /// 初始化墙壁
    /// </summary>
    private void InitWall()
    {
        //随机生成一个渗透的系统
        Percolation pc = new Percolation(cols);
        while (!pc.percolates())
        {
            int row = Random.Range(0, rows + 1);
            int col = Random.Range(0, cols + 1);
            pc.open(col, row);
        }
        for(int x = 1; x < cols; x++)
        {
            for(int y = 1; y < rows; y++)
            {
                if(!pc.isOpen(x, y))
                {
                    GameObject wallGo = GameObject.Instantiate(wall);
                    wallGo.transform.position = new Vector3(x-.5f,y-.5f);
                    wallGo.transform.SetParent(mapHolder);
                }
            }
        }
        
    }
}
