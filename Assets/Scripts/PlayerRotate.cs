using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    // 회전 속도 변수
    public float rotateSpeed = 200f;

    // 회전 값 변수
    float mx = 0;

    void Update()
    {
        // 사용자의 마우스 입력을 받아 물체를 회전시키고 싶다
        // 1. 마우스 입력을 받는다
        float mouseX = Input.GetAxis("Mouse X");

        // 1-1. 회전 값 변수에 마우스 입력 값만큼 미리 누적시킨다
        mx += mouseX * rotateSpeed * Time.deltaTime;

        // 2. 물체를 회전 방향으로 회전시킨다
        transform.eulerAngles = new Vector3(0, mx, 0);
    }
}
