using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // 플레이어 체력
    public int hp = 20;

    // 최대 체력
    int maxHp = 20;

    // hp 슬라이더
    public Slider hpSlider;

    public GameObject hitEffect;

    // 애니메이터 변수
    Animator anim;


    private void Start()
    {
        cc = GetComponent<CharacterController>();

        // 애니메이터 받아오기
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // 게임 상태가 ‘게임 중’ 상태일 때만 조작할 수 있게 한다.
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // 이동 블렌딩 트리를 호출하고 벡터의 크기 값을 넘겨준다
        anim.SetFloat("MoveMotion", dir.magnitude);

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

        hpSlider.value = (float)hp / (float)maxHp;
    }

    // 플레이어 피격 함수
    public void DamageAction(int damage)
    {
        // 에너미의 공격력만큼 플레이어의 체력을 깎는다
        hp -= damage;

        // 만일, 플레이어의 체력이 0보다 크면 피격 효과를 출력한다.
        if (hp > 0)
        {
            // 피격 이펙트 코루틴을 시작한다.
            StartCoroutine(PlayHitEffect());
        }
    }

    // 피격 효과 코루틴 함수
    IEnumerator PlayHitEffect()
    {
        // 1. 피격 UI를 활성화한다.
        hitEffect.SetActive(true);

        // 2. 0.3초간 대기한다.
        yield return new WaitForSeconds(0.3f);

        // 3. 피격 UI를 비활성화한다.
        hitEffect.SetActive(false);
    }

}
