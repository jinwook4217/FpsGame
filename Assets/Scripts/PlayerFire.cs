using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    public GameObject firePosition;

    public GameObject bombFactory;

    public float throwPower = 15f;

    // 피격 이펙트 오브젝트
    public GameObject bulletEffect;

    // 피격 이펙트 파티클 시스템
    ParticleSystem ps;

    // 발사 무기 공격력
    public int weaponPower = 5;

    // 애니메이터 변수
    Animator anim;

    // 무기 모드 변수
    enum WeaponMode
    {
        Normal,
        Sniper
    }

    // 현재 무기 모드
    WeaponMode wMode;

    // 카메라 축소 확대 확인용 변수
    bool ZoomMode = false;

    // 무기 모드 텍스트
    public Text wModeText;

    // 총 발사 효과 오브젝트 배열
    public GameObject[] eff_Flash;


    void Start()
    {
        // 피격 이펙트 오브젝트에서 파티클 시스템 컴포넌트 가져오기
        ps = bulletEffect.GetComponent<ParticleSystem>();

        // 애니메이터 가져오기
        anim = GetComponentInChildren<Animator>();

        // 무기 모드 초기화
        wMode = WeaponMode.Normal;
    }

    void Update()
    {
        // 게임 상태가 ‘게임 중’ 상태일 때만 조작할 수 있게 한다.
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            switch (wMode)
            {
                case WeaponMode.Normal:
                    // 수류탄 발사
                    GameObject bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.transform.position;

                    Rigidbody rb = bomb.GetComponent<Rigidbody>();
                    rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                    break;
                case WeaponMode.Sniper:
                    // 화면 확대
                    // 만약 줌 모드 상태가 아니라, 카메라를 확대하고 줌 모드 상태로 변경
                    if (!ZoomMode)
                    {
                        Camera.main.fieldOfView = 15f;
                        ZoomMode = true;
                    }
                    // 그게 아니라면 카메라를 원래 상태로 되돌리, 줌 모드 상태를 해제
                    else
                    {
                        Camera.main.fieldOfView = 60f;
                        ZoomMode = false;
                    }
                    
                    break;
            }
        }

        // 마우스 왼쪽 버튼을 입력받는다
        if (Input.GetMouseButtonDown(0))
        {
            // 만일 이동 블렌드 트리 파라미터 값이 0이라, 공격 애니메이션을 실시한다
            if (anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }

            // 레이를 생성한 후 발사될 위치와 진행 방향을 설정한다
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // 레이가 부딪힌 대상의 정보를 저장할 변수를 생성한다
            RaycastHit hitInfo = new RaycastHit();

            // 레이를 발사한 후 만일 부딪힌 물체가 있으면 피격 이펙트를 표시한다
            if (Physics.Raycast(ray, out hitInfo))
            {
                // 만일 레이에 부딪힌 대상의 레이어가 'Enemy'라면 데미지 함수를 실행
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }
                else
                {
                    // 피격 이펙트의 위치를 레이가 부딪힌 지점으로 이동시킨다
                    bulletEffect.transform.position = hitInfo.point;

                    // 피격 이벤트의 forward 방향을 레이가 부딪힌 지점의 법선 벡터와 일치시킨다
                    bulletEffect.transform.forward = hitInfo.normal;

                    // 피격 이펙트를 플레이한다
                    ps.Play();
                }
            }

            // 총 이펙트를 실시
            StartCoroutine(ShootEffectOn(0.05f));
        }

        // 만약 키보드의 숫자 1번을 입력 받으, 무기 모드를 일반 모드로 변경
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            wMode = WeaponMode.Normal;

            Camera.main.fieldOfView = 60f;

            // 일반 모드 텍스트
            wModeText.text = "Normal Mode";
        }
        // 만약 키보드의 숫자 2번을 입력 받으, 무기 모드를 스나이퍼 모드로 변경
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Sniper;

            // 스나이퍼 모드 텍스트
            wModeText.text = "Sniper Mode";
        }
    }

    // 총구 이펙트 코루틴 함수
    IEnumerator ShootEffectOn(float duration)
    {
        // 랜덤하게 숫자를 뽑는다
        int num = Random.Range(0, eff_Flash.Length - 1);
        // 이펙트 오브젝트 배열에서 뽑힌 숫자에 해당하는 이펙트 오브젝트를 활성화
        eff_Flash[num].SetActive(true);
        // 지정한 시간만큼 대기
        yield return new WaitForSeconds(duration);
        // 이펙트 오브젝트를 다시 비활성화
        eff_Flash[num].SetActive(false);
    }
}
