using System;
using UnityEngine;

//마우스 왼쪽버튼을 누르면 총을 쏘고싶다.
//필요한요소: 손, 총알자국 VFX
public class PlayerFire : MonoBehaviour
{
    public Transform hand;
    public GameObject bulletImpactFactory;
    private LineRenderer lr;
    public Camera cameraScope;
    public float zoomOutFOV = 90f;
    public float zoomInFOV = 15f;
    public float zoomSpeed = 30f;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        ZoomProcess();
        //만약 마우스 왼쪽버튼을 누르면
        //손위치에서 손의 앞방향으로 Ray를 만들고
        Ray ray = new Ray(hand.position, hand.forward);
        lr.SetPosition(0, ray.origin);
        RaycastHit hitInfo;
        bool isHit = Physics.Raycast(ray, out hitInfo);

        if (isHit)
        {
            lr.SetPosition(1, hitInfo.point);

#if Oculus
            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
#else
            if (Input.GetButtonDown("Fire1"))
#endif
            {                    //총알자국 생성
                GameObject bulletImpact = Instantiate(bulletImpactFactory);
                //총알자국을 부딪힌 곳에 생성
                bulletImpact.transform.position = hitInfo.point;
                Destroy(bulletImpact, 2f);

                //만약 닿은 것이 Enemy라면
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    //Enemy에게 "파괴되어줘" 라고 하고싶다.
                    hitInfo.transform.GetComponent<Enemy>().DiePlz(1, hand.transform.forward);
                }
            }
        }
        else
        {
            lr.SetPosition(1, ray.origin + ray.direction * 100f);
        }
    }

    private void ZoomProcess()
    {
        //컨트롤러의 thumbstick을 밀면 축소, 당기면 확대하고싶다. CameraScope의 FOV를 건드리겠다.
        Vector2 axis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

        cameraScope.fieldOfView -= axis.y * zoomSpeed * Time.deltaTime;
        cameraScope.fieldOfView = Mathf.Clamp(cameraScope.fieldOfView, zoomInFOV, zoomOutFOV);
    }
}