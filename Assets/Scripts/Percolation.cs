using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Percolation{

    private bool[] opened;//所有结点是否开放的布尔值数组
    private int n; // n*n大小的grid;
    WeightedQuickUnionUF uf;

    public Percolation(int n)
    {
        this.n = n;//储存传入的参数n
        uf = new WeightedQuickUnionUF(n * n + 2);//建立n*n+2大小的序列，第一位id为0，最后一位id为n*n-1

        //模拟开始时，除第一个点和最后一个点（两个）虚拟点位默认为开放，其余都默认关闭
        opened = new bool[n * n + 2];
        for(int i = 1; i < n * n + 1; i++)
        {
            opened[i] = false;
        }
        opened[0] = true;
        opened[n * n + 1] = true;
    }

    //将传入的二维定位（行、列）转化为一维的id
    private int index(int row,int col)
    {
        //当传入的行数为0时，返回第一个虚拟点位的id
        if(row == 0)
        {
            return 0;
        }

        //当传入的行数为n+1时，返回最后一个点（虚拟点位）的id
        else if(row == n + 1)
        {
            return n * n + 1;
        }

        else
        {
            int p = (row - 1) * n + col;
            return p;
        }
    }

    //使传入坐标点开放
    public void open(int row,int col)
    {
        int p = index(row, col);
        opened[p] = true;
        //检查该点上下左右是否开放，如果开放则将两者连接
        bool up = isOpen(row - 1, col);
        bool right = isOpen(row, col + 1);
        bool left = isOpen(row, col - 1);
        bool down = isOpen(row + 1, col);
        if (up) uf.union(p, index(row - 1, col));
        if (right) uf.union(p, index(row, col + 1));
        if (left) uf.union(p, index(row, col - 1));
        if (down) uf.union(p, index(row + 1, col));
    }

    //检查传入坐标的点是否开放
    public bool isOpen(int row,int col)
    {
        //当传入行数为0时，返回第一个虚拟点位的开放性
        if(row == 0){
            return opened[0];
        }

        //当传入行数为n+1时，返回第二个虚拟点位（位于最后）的开放性
        else if (row == n + 1)
        {
            return opened[n * n + 1];
        }

        //当行数超出边界时，返回fasle
        else if (row < 0 || col <= 0 || row > n + 1 || col > n)
        {
            return false;
        }

        //其余情况通过计算p
        else
        {
            int p = index(row, col);
            return opened[p];
        }

    }


    //该系统是否渗透
    public bool percolates()
    {
        return uf.connected(0, n * n + 1);
    }

    //如果系统不渗透，持续随机打开系统中的点，直到系统渗透为止
    public void doPercolation(int n)
    {
        while (!percolates())
        {
            int row = Random.Range(0,n+1);
            int col = Random.Range(0,n+1);
            open(row, col);
        }
    }
}
