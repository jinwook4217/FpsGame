using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    // 에너미 상태 상수
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    // 에너미 체력
    public int hp = 15;

    // 에너미 최대 체력
    public int maxHp = 15;

    // 에너미 상태 변수
    EnemyState m_State;

    // 플레이어 발견 범위
    public float findDistance = 8f;

    // 공격 가능 범위
    public float attackDistance = 2f;

    // 이동 속도
    public float moveSpeed = 5f;

    // 누적 시간
    float currentTime = 0f;

    // 공격 딜레이 시간
    float attackDelay = 2f;

    // 에너미 공격력
    public int attackPower = 3;

    // 초기 위치 저장용 변수
    Vector3 originPos;

    // 이동 가능 범위
    public float moveDistance = 20f;

    // 캐릭터 컨트롤러 컴포넌트
    CharacterController cc;

    // 플레이어 트랜스폼
    Transform player;

    void Start()
    {
        // 최초의 상태는 대기
        m_State = EnemyState.Idle;

        // 플레이어 트랜스폼 컴포넌트 받아오기
        player = GameObject.Find("Player").transform;

        // 캐릭터 컨트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();

        // 자신의 초기 위치 저장하기
        originPos = transform.position;
    }

    void Update()
    {
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }
    }

    public void HitEnemy(int hitPower)
    {
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }

        // 플레이어의 공격력만큼 에너미의 체력을 감소
        hp -= hitPower;

        // 에너미의 체력이 0보다 크면 피격 상태로 전환
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any state -> Damaged");
            Damaged();
        }
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any state -> Die");
            Die();
        }
    } 

    private void Die()
    {
        // 진행 중인 피격 코루틴을 종료
        StopAllCoroutines();

        // 죽음 상태를 처리하기 위한 코루틴 실행
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // 캐릭터 컨트롤러 컴포넌트를 비활성시킴
        cc.enabled = false;

        // 2초 동안 기다린 후에 자기 자신을 제거
        yield return new WaitForSeconds(2f);
        print("소멸");
        Destroy(gameObject);
    }

    private void Damaged()
    {
        // 피격 상태를 처리하기 위한 코루틴을 실행한다
        StartCoroutine(DamageProcess());
    }

    // 데미지 처리용 코루틴 함수
    IEnumerator DamageProcess()
    {
        // 피격 모션 시간만큼 기다린다
        yield return new WaitForSeconds(0.5f);

        // 현재 상태를 이동 상태로 전환
        m_State = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }

    private void Return()
    {
        // 만약 초기 위치에서 거리가 0.1f 이상이라면 초기 위치 쪽으로 이동한다
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            // 자신의 위치를 초기 위치로 조정하고 현재 상태를 대기로 전환
            transform.position = originPos;

            // hp를 다시 회복
            hp = maxHp;
            m_State = EnemyState.Idle;
            print("상태 전환: Return -> Idle");
        }
    }

    private void Attack()
    {
        // 만약 플레이어가 공격 범위에 있으면 플레이어를 공격
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            // 일정한 시간마다 플레이어를 공격
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("Attack");
                currentTime = 0f;
            }
        }
        else
        {
            // 그렇지 않다면 현재 상태를 이동으로 전환
            m_State = EnemyState.Move;
            print("상태 전: Attack -> Move");
            currentTime = 0f;
        }
    }

    private void Move()
    {
        // 만약 현재 위치가 초기 위치에서 이동 가능 범위를 벗어난다면
        if (Vector3.Distance(transform.position, player.position) > moveDistance)
        {
            // 현재 상태를 복귀로 전환
            m_State = EnemyState.Return;
            print("상태 전환: Move -> Return");
        }
        // 플레이어와의 거리가 공격 범위 밖이라면 플레이어를 향해 이동한다
        else if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            // 이동 방향 설정
            Vector3 dir = (player.position - transform.position).normalized;

            // 캐릭터 컨트롤러를 이용해 이동하기
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            // 그렇지 않다면 현재 상태를 공격으로 전환
            m_State = EnemyState.Attack;
            print("상태 전환: Move -> Attack");

            // 누적 시간을 공격 딜레이 시간만큼 미리 진행시켜 놓는다
            currentTime = attackDelay;
        }
    }

    private void Idle()
    {
        // 플레이어와의 거리가 액션 시작 범위 이내라면 Move 상태로 전환
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환: Idle -> Move");
        }
    }
}
