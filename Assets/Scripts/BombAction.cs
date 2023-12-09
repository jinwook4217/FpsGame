using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;

    // 수류탄 데미지
    public int attackPower = 10;

    // 수류탄 폭발 효과 범위
    public float explosionRadius = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        // 폭발 효과 반경 내에서 레이어가 'Enemy'인 모든 게임 오브젝트의 Collider 컴포넌트를 배열에 저장
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

        // 저장된 Collider 배열에 있는 모든 에너미에게 수류탄 데미지를 적용
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
        }

        GameObject effect = Instantiate(bombEffect);
        effect.transform.position = transform.position;
        Destroy(gameObject);
    }
}
