using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 2f;//角色移动速度

    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaY = Input.GetAxis("Vertical") * speed;
        if(deltaX != 0 || deltaY != 0)
        {
            Vector2 movement = new Vector2(deltaX, deltaY);//两个方向的移动
            movement = Vector2.ClampMagnitude(movement, speed);//限制对角线移动速度

            movement *= Time.deltaTime;//固定帧率
            movement = transform.TransformDirection(movement);//本地变化为全局坐标
            _rigidbody2D.velocity = movement;
        }
    }
}
