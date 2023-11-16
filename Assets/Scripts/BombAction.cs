using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject effect = Instantiate(bombEffect);
        effect.transform.position = transform.position;
        Destroy(gameObject);
    }
}
