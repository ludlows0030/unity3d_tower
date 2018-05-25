using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 2f;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _CurrentPosition;
    private Vector2 _PreviousPosition;
    private Vector2 _NextMovement;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _CurrentPosition = _rigidbody2D.position;
        _PreviousPosition = _rigidbody2D.position;
    }

    private void FixedUpdate()
    {
        _PreviousPosition = _rigidbody2D.position;

        Vector2 movement = Vector2.zero;
        float deltaX = Input.GetAxis("Horizontal")*speed;
        float deltaY = Input.GetAxis("Vertical")*speed;
        movement += new Vector2(deltaX, deltaY);
        movement *= Time.deltaTime;
        _NextMovement += movement;

        _CurrentPosition = _PreviousPosition + _NextMovement;
        _rigidbody2D.MovePosition(_CurrentPosition);

        _NextMovement = Vector2.zero;
    }
}
