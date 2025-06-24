using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    [Tooltip("The multiplier for the vertical movement speed")]
    public float verticalOffset = 1.3f;
    private Rigidbody rb;
    private Vector3 _input;
    private Vector3 skewedInput;
    private MobileJoystick joystick;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        joystick = GameObject.FindObjectOfType<MobileJoystick>();
    }

    private void Update()
    {
        GatherInput();
    }
    private void FixedUpdate()
    {
        Move();
    }
    void GatherInput()
    {
        if (joystick && (joystick.axisValue.x != 0 || joystick.axisValue.y != 0))
        {
            _input = new Vector3(joystick.axisValue.x, 0, joystick.axisValue.y * verticalOffset);
        }
        else
        {
            _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical") * verticalOffset);
        }
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        skewedInput = matrix.MultiplyPoint3x4(_input); 
    }
    void Move()
    {
        rb.MovePosition(transform.position + skewedInput * moveSpeed * Time.deltaTime);
    }
}
