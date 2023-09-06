using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ð����� ������ ��ġ�� ���ϰ� �װ��� �����忡�� ���� ���� ��ġ�ϰ�ʹ�.
//�ʿ���: ����ð�, �����ð�, ������
public class CloudSpawnManager : MonoBehaviour
{
    private float currentTime;
    public float makeTime = 2f;
    public GameObject enemyFactory;

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > makeTime)
        {
            currentTime = 0;

            MakeEnemy(GetRandomPosition());
        }
    }

    private Vector3 GetRandomPosition()
    {
        //�簢���� ���ʿ� ������ �� �ϳ��� ��
        float minX, maxX, minZ, maxZ;
        Vector3 p = transform.position;
        float width = transform.localScale.x;
        float height = transform.localScale.z;
        minX = p.x - width / 2;
        maxX = p.x + width / 2;
        minZ = p.z - height / 2;
        maxZ = p.z + height / 2;

        float x = UnityEngine.Random.Range(minX, maxX);
        float y = p.y;
        float z = UnityEngine.Random.Range(minZ, maxZ);
        //�� ��ġ���� �Ʒ��� ���ϴ� Ray�� �����
        Vector3 origin = new Vector3(x, y, z);
        //�� ��ġ���� �Ʒ��� ���ϴ� Ray�� �����.
        Ray ray = new Ray(origin, Vector3.down);
        //���� ���� ����� �ƴϰ�, �װ��� tag�� "Ground"��� �� ��ġ�� ��ȯ�ϰ� �ʹ�.
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.CompareTag("Ground"))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }

    private void MakeEnemy(Vector3 position)
    {
        //�����忡�� ���� ���� pos��ġ�� ��ġ�ϰ�ʹ�.
        var enemy = Instantiate(enemyFactory);
        enemy.transform.position = position;
    }
}