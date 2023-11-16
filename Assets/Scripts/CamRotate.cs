using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // 회전 속도 변수
    public float rotateSpeed = 200f;

    // 회전 값 변수
    float mx = 0;
    float my = 0;

    void Update()
    {
        // 사용자의 마우스 입력을 받아 물체를 회전시키고 싶다
        // 1. 마우스 입력을 받는다
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 1-1. 회전 값 변수에 마우스 입력 값만큼 미리 누적시킨다
        mx += mouseX * rotateSpeed * Time.deltaTime;
        my += mouseY * rotateSpeed * Time.deltaTime;

        // 1-2. 마우스 상하 이동 이동 회전 변수의 값을 -90도 ~ 90도 사이로 제한한다
        my = Mathf.Clamp(my, -90f, 90f);

        // 2. 물체를 회전 방향으로 회전시킨다
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
