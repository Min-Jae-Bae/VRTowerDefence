using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//앞으로 이동하고싶다. 물리기반으로
public class EnemyBullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 10f;
    private bool isHit;
    private Quaternion hitRotation;
    private Vector3 hitPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        if (isHit)
        {
            transform.rotation = hitRotation;
            transform.position = hitPosition;
        }
        else
        {
            //리지드 바디의 속력으로 날린다.
            rb.transform.forward = rb.velocity.normalized;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isHit)
        {
            isHit = true;
            hitRotation = transform.rotation;
            hitPosition = transform.position;
            rb.isKinematic = true;
            rb.useGravity = false;
            GetComponent<Collider>().enabled = false;
            StartCoroutine(CoFadeOut());
            //만약 부딪힌 상대가 타워라면
            //타워한테 체력1감소해 함수를 호출 하고싶다.
            Tower tower = collision.gameObject.GetComponent<Tower>();
            if (tower != null)
            {
                tower.OnDamageProcess();
            }
        }
    }

    private IEnumerator CoFadeOut()
    {
        yield return new WaitForSeconds(1f);
        float alpha = 1;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for (float time = 0; time < 1; time += Time.deltaTime)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                Color c = renderers[i].material.color;
                c.a = alpha;
                renderers[i].material.color = c;
            }
            alpha -= Time.deltaTime;
            yield return 0;
        }
        Destroy(gameObject);
    }
}