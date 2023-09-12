using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //���� ���̾ ã�´�.
        int layerMask = 1 << LayerMask.NameToLayer("Enemy");
        //���� �ݰ� 5M�ȿ� �ִ� ������ ��� ã�´�.
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 5f, transform.forward, 0.001f);

        for (int i = 0; i < hits.Length; i++)
        {
            // �ݰ� 5M���� Enemy�鿡�� �������� 2�� �ְ� �ʹ�.
            Enemy enemy = hits[i].collider.GetComponent<Enemy>();
            enemy.DiePlz(2, Vector3.forward);
        }
    }
}