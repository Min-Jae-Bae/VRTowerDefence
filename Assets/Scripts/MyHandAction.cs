using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HandTrigger키를 누르면 카메라 위치에서 손 앞방향으로 Ray를 만들고
//바라보보 닿은 것이 Grenade라면 복제해서 내손에 쥐고싶다.
//HandTrigger키를 때면 잡은 Grenade가 있다면 손 앞 방향으로 던지고싶다.
//필요요소 : 손, 잡은물체, 컨트롤러
public class MyHandAction : MonoBehaviour
{
    public Transform hand;
    private GameObject grabObject;
    public OVRInput.Controller controller;

    private void Update()
    {
        if (null != grabObject)
        {
            //grabObject를 손으로 계속 이동시키고싶다.
            grabObject.transform.position = Vector3.Lerp
                (grabObject.transform.position, hand.position, Time.deltaTime * 5f);
        }

        // G키를 누르면
        // 1. 먼저 내 손 반경 1M 안에 충돌체들을 검사해서 손에 쥐고싶다.
        // 만약 손에 쥔 물체가 없다면
        // 2. Ray를 이용해서 쥐고 싶다.
#if Oculus
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controller))
#else
        if (Input.GetKeyDown(KeyCode.G))
#endif
        {
            if (!GripByOverlap())
            {
                GridpByRay();
            }
        }

#if Oculus
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, controller))
#else
        if (Input.GetKeyUp(KeyCode.G))
#endif
            if (grabObject)
            {
                //손 앞 방향으로 던지고싶다.
                var grabRB = grabObject.GetComponent<Rigidbody>();
                grabRB.isKinematic = false;
                grabRB.useGravity = true;
                //컨트롤러의 속도를 grabRB에 반영하고싶다.
                //grabRB.velocity = GetHandVelocity;
                //grabRB.angularVelocity = GetHandAngularVelocity;
                //컨트롤러의 각속도를 grabRB에 반영하고싶다.

                grabRB.velocity = OVRInput.GetLocalControllerAngularVelocity(controller) * 20;
                grabRB.angularVelocity = OVRInput.GetLocalControllerAngularVelocity(controller) * 20;
                grabObject = null;
            }
    }

    //손의 움직임으로 속도를 만들어서 반환하고싶다.
    //필요한 요소: 손의 현재위치, 이전위치
    private Vector3 prevPosition;

    private float kAdjustHandVelocity = 300f;
    private Vector3 handVelocity;

    private Vector3 GetHandVelocity
    {
        get { return handVelocity * kAdjustHandVelocity; }
    }

    //손의 움직임으로 각도를 재서 각속도를 구하고싶다.
    //필요요소: 이전각, 현재
    private Vector3 handAngularVelocity;

    private Quaternion prevAngle;
    public float kAdjustHandAngularVelocity = 20f;

    private Vector3 GetHandAngularVelocity
    {
        get { return -handAngularVelocity * kAdjustHandAngularVelocity; }
    }

    private void FixedUpdate()
    {
        //손의 이전위치를 이용해서 손의 속도를 계산하고싶다.
        handVelocity = hand.position - prevPosition;
        prevPosition = hand.position;

        //손의 이전각을 이용해서 손의 각속도를 계산하고싶다.
        //deltaAngle를 구하고싶다.
        Quaternion deltaAngle = hand.rotation * Quaternion.Inverse(prevAngle);
        prevAngle = hand.rotation;

        //deltaAngle에서 각도와 기준축을 얻어내고싶다.
        float degree;
        Vector3 axis;
        deltaAngle.ToAngleAxis(out degree, out axis);

        //얻어낸 각도를 radian으로 변환하고싶다.
        float radian = Mathf.Deg2Rad * degree;

        //radian / t
        handAngularVelocity = radian / Time.fixedDeltaTime * axis;
    }

    private void GridpByRay()
    {
        Ray ray = new Ray(hand.position, hand.forward);
        //바라보고
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            //닿은 것이 Grenade라면 복제해서
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Grenade"))
            {
                //내손에 쥐고싶다.
                grabObject = Instantiate(hitInfo.transform.gameObject);
            }
        }
    }

    private bool GripByOverlap()
    {
        //손을 중심으로 반경 1M안의 Grenade를 찾고싶다.
        // 가장 가까운 Grenade를 선택하고싶다.
        // 만약 선택된 Grenade가 있다면
        // 그 게임오브젝트를 인스턴스화해서 grabObject에 대입하고싶다.
        // true를 반환하고싶다.
        // 그렇지 않으면 (선택된 Grenade가 없다면)
        // else 그렇지 않으면 선택된 Grenade가 없다면
        // false를 반환하고싶다.

        int layerMask = 1 << LayerMask.NameToLayer("Grenade");
        Collider[] colliders = Physics.OverlapSphere(hand.position, 1f, layerMask);
        List<Collider> list = new List<Collider>(colliders);
        if (colliders.Length > 0)
        {
            list.Sort((a, b) =>
            {
                float aDist = Vector3.Distance(transform.position, a.transform.position);
                float bDist = Vector3.Distance(transform.position, b.transform.position);
                return aDist < bDist ? -1 : 1;
            });
            grabObject = Instantiate(list[0].gameObject);
            return true;
        }
        return false;
    }
}