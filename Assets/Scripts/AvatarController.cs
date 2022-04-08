using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AvatarController : MonoBehaviour
{
    private bool isMyself = false;
    private Animator animator;
    private CharacterController characterController;
    public float moveSpeed = 1.0f;
    public float rotateSpeed = 10.0f;

    private float lastTime, curTime;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMyself) {
            if (!characterController.isGrounded) {
                Vector3 direction = Vector3.zero;
                direction.y -= 10f * Time.deltaTime;
                characterController.Move(direction*Time.deltaTime);
            }
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
            SendMessage();
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

    public void HandleMessage(byte[] data) {
        Debug.Log("HandleMessage: " + BitConverter.ToString(data));
        int index = 0;
        Vector3 localPosition = transform.localPosition;
        int state = BitConverter.ToInt32(data,index);//4
        animator.SetInteger("state",state);
        index += sizeof(int);
        localPosition.x = BitConverter.ToSingle(data,index);//4
        index += sizeof(float);
        localPosition.y = BitConverter.ToSingle(data, index);//4
        index += sizeof(float);
        localPosition.z = BitConverter.ToSingle(data, index);//4
        transform.localPosition = localPosition;

        Vector3 eulerAngles = transform.eulerAngles;
        index += sizeof(float);
        eulerAngles.y = BitConverter.ToSingle(data, index);//4
        transform.eulerAngles = eulerAngles;
    }

    public void setMySelf(bool myself) {
        isMyself = myself;
    }

    private void SendMessage() {
        curTime = Time.time;
        if ((curTime - lastTime) < 0.04) {
            return;
        }
        lastTime = curTime;
        byte[] data = new byte[sizeof(int)+sizeof(float)*4];
        int state = animator.GetInteger("state");
        int offset = 0;
        Buffer.BlockCopy(BitConverter.GetBytes(state),0,data,offset,sizeof(int));
        offset += sizeof(int);
        Buffer.BlockCopy(BitConverter.GetBytes(transform.localPosition.x), 0, data, offset, sizeof(float));
        offset += sizeof(float);
        Buffer.BlockCopy(BitConverter.GetBytes(transform.localPosition.y), 0, data, offset, sizeof(float));
        offset += sizeof(float);
        Buffer.BlockCopy(BitConverter.GetBytes(transform.localPosition.z), 0, data, offset, sizeof(float));

        offset += sizeof(float);
        Buffer.BlockCopy(BitConverter.GetBytes(transform.eulerAngles.y), 0, data, offset, sizeof(float));

        Home home  = GameObject.Find("GameController").GetComponent<Home>();
        if (!ReferenceEquals(home, null)) {
            Debug.Log("SendMessage: " + BitConverter.ToString(data));
            home.sendMessage(data);
        }
    }
}
