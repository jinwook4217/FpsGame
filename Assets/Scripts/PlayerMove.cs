using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;

    public float jumpPower = 10f;

    public bool isJumping = false;

    // 중력 변수
    float gravity = -20f;

    // 수직 속력 변수
    public float yVelocity = 0f;

    CharacterController cc;

    int hp = 10;


    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        dir = Camera.main.transform.TransformDirection(dir);

        if (cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                isJumping = false;
            }
            
            yVelocity = 0f;
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);
    }

    // 플레이어 피격 함수
    public void DamageAction(int damage)
    {
        // 에너미의 공격력만큼 플레이어의 체력을 깎는다
        hp -= damage;
    }
}
