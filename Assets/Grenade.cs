using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //적의 레이어를 찾는다.
        int layerMask = 1 << LayerMask.NameToLayer("Enemy");
        //적의 반경 5M안에 있는 적들을 모두 찾는다.
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 5f, transform.forward, 0.001f);

        for (int i = 0; i < hits.Length; i++)
        {
            // 반경 5M안의 Enemy들에게 데미지를 2점 주고 싶다.
            Enemy enemy = hits[i].collider.GetComponent<Enemy>();
            enemy.DiePlz(2, Vector3.forward);
        }
    }
}