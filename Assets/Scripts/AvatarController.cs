using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    public float moveSpeed = 1.0f;
    public float rotateSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Walk(KeyCode.W);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Walk(KeyCode.S);
        }
        else
        {
            Idle();
        }
        if (Input.GetKey(KeyCode.A))
        {
            Rotate(KeyCode.A);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            Rotate(KeyCode.D);
        }
    }

    private void Walk(KeyCode code)
    {
        Vector3 direction;
        switch (code)
        {
            case KeyCode.W:// 按W键，向前移动
                direction = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up) * Vector3.forward;
                characterController.Move(direction * Time.deltaTime * moveSpeed);
                break;
            case KeyCode.S:// 按S键，向后移动
                direction = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up) * Vector3.back;
                characterController.Move(direction * Time.deltaTime * moveSpeed);
                break;
            default: break;
        }
        animator.SetInteger("state", 1);
    }

    private void Rotate(KeyCode code)
    {
        Vector3 eulerAngles = transform.eulerAngles;
        switch (code)
        {
            case KeyCode.A:// 按A键，向左旋转
                eulerAngles.y -= rotateSpeed * Time.deltaTime;
                break;
            case KeyCode.D:// 按D键，向右旋转
                eulerAngles.y += rotateSpeed * Time.deltaTime;
                break;
            default: break;
        }
        transform.eulerAngles = eulerAngles;
    }
    private void Idle()
    {
        animator.SetInteger("state", 0);
    }
}
