using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//일정시간마다 랜덤한 위치를 정하고 그곳에 적공장에서 적을 만들어서 배치하고싶다.
//필요요소: 현재시간, 생성시간, 적공장
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
        //사각형의 안쪽에 임의의 점 하나를 찍어서
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
        //그 위치에서 아래를 향하는 Ray를 만들고
        Vector3 origin = new Vector3(x, y, z);
        //그 위치에서 아래를 향하는 Ray를 만든다.
        Ray ray = new Ray(origin, Vector3.down);
        //닿은 곳이 허공이 아니고, 그곳의 tag가 "Ground"라면 그 위치를 반환하고 싶다.
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.CompareTag("Ground"))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }

    private void MakeEnemy(Vector3 position)
    {
        //적공장에서 적을 만들어서 pos위치에 배치하고싶다.
        var enemy = Instantiate(enemyFactory);
        enemy.transform.position = position;
    }
}