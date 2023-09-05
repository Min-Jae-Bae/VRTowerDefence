using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//마우스 오른쪽 버튼을 누르면
//손의 위치에서 손의 앞방향으로 Ray를 발사하고
//부딪힌곳이 있다면 플레이어를 그곳으로 이동하고싶다.
public class Teleport : MonoBehaviour
{
    public Transform hand;
    public Transform player;
    private LineRenderer lineRenderer;
    public Transform marker;
    public float kAdjust = 1f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        marker.localScale = Vector3.one * kAdjust;
    }

    private void Update()
    {
        //마우스 오른쪽 버튼을 누르면 ready 상태가되고

        Ray ray = new Ray(hand.position, hand.forward);
        lineRenderer.SetPosition(0, ray.origin);
        RaycastHit hitInfo;
        bool isHit = Physics.Raycast(ray, out hitInfo);

        if (isHit)
        {
            //어딘가 부딪혔다.
            lineRenderer.SetPosition(1, hitInfo.point);
            marker.position = hitInfo.point;
            marker.up = hitInfo.normal;
            marker.localScale = Vector3.one * kAdjust * hitInfo.distance;
        }
        else
        {
            //허공이다. hand의 100M 앞
            lineRenderer.SetPosition(1, ray.origin + ray.direction * 100f);
            marker.position = ray.origin + ray.direction * 100f;
            marker.up = -ray.direction;
            marker.localScale = Vector3.one * kAdjust * 100f;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            //어딘가 부딪힘
            lineRenderer.enabled = true;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            //떼면 선을 안보고싶다.
            lineRenderer.enabled = false;
            if (isHit)
            {
                //어딘가 부딪힘
                player.position = hitInfo.point;
            }
        }
    }
}