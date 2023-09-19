using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HandTriggerŰ�� ������ ī�޶� ��ġ���� �� �չ������� Ray�� �����
//�ٶ󺸺� ���� ���� Grenade��� �����ؼ� ���տ� ���ʹ�.
//HandTriggerŰ�� ���� ���� Grenade�� �ִٸ� �� �� �������� ������ʹ�.
//�ʿ��� : ��, ������ü, ��Ʈ�ѷ�
public class MyHandAction : MonoBehaviour
{
    public Transform hand;
    private GameObject grabObject;
    public OVRInput.Controller controller;

    private void Update()
    {
        if (null != grabObject)
        {
            //grabObject�� ������ ��� �̵���Ű��ʹ�.
            grabObject.transform.position = Vector3.Lerp
                (grabObject.transform.position, hand.position, Time.deltaTime * 5f);
        }

        // GŰ�� ������
        // 1. ���� �� �� �ݰ� 1M �ȿ� �浹ü���� �˻��ؼ� �տ� ���ʹ�.
        // ���� �տ� �� ��ü�� ���ٸ�
        // 2. Ray�� �̿��ؼ� ��� �ʹ�.
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
                //�� �� �������� ������ʹ�.
                var grabRB = grabObject.GetComponent<Rigidbody>();
                grabRB.isKinematic = false;
                grabRB.useGravity = true;
                //��Ʈ�ѷ��� �ӵ��� grabRB�� �ݿ��ϰ�ʹ�.
                //grabRB.velocity = GetHandVelocity;
                //grabRB.angularVelocity = GetHandAngularVelocity;
                //��Ʈ�ѷ��� ���ӵ��� grabRB�� �ݿ��ϰ�ʹ�.

                grabRB.velocity = OVRInput.GetLocalControllerAngularVelocity(controller) * 20;
                grabRB.angularVelocity = OVRInput.GetLocalControllerAngularVelocity(controller) * 20;
                grabObject = null;
            }
    }

    //���� ���������� �ӵ��� ���� ��ȯ�ϰ�ʹ�.
    //�ʿ��� ���: ���� ������ġ, ������ġ
    private Vector3 prevPosition;

    private float kAdjustHandVelocity = 300f;
    private Vector3 handVelocity;

    private Vector3 GetHandVelocity
    {
        get { return handVelocity * kAdjustHandVelocity; }
    }

    //���� ���������� ������ �缭 ���ӵ��� ���ϰ�ʹ�.
    //�ʿ���: ������, ����
    private Vector3 handAngularVelocity;

    private Quaternion prevAngle;
    public float kAdjustHandAngularVelocity = 20f;

    private Vector3 GetHandAngularVelocity
    {
        get { return -handAngularVelocity * kAdjustHandAngularVelocity; }
    }

    private void FixedUpdate()
    {
        //���� ������ġ�� �̿��ؼ� ���� �ӵ��� ����ϰ�ʹ�.
        handVelocity = hand.position - prevPosition;
        prevPosition = hand.position;

        //���� �������� �̿��ؼ� ���� ���ӵ��� ����ϰ�ʹ�.
        //deltaAngle�� ���ϰ�ʹ�.
        Quaternion deltaAngle = hand.rotation * Quaternion.Inverse(prevAngle);
        prevAngle = hand.rotation;

        //deltaAngle���� ������ �������� ����ʹ�.
        float degree;
        Vector3 axis;
        deltaAngle.ToAngleAxis(out degree, out axis);

        //�� ������ radian���� ��ȯ�ϰ�ʹ�.
        float radian = Mathf.Deg2Rad * degree;

        //radian / t
        handAngularVelocity = radian / Time.fixedDeltaTime * axis;
    }

    private void GridpByRay()
    {
        Ray ray = new Ray(hand.position, hand.forward);
        //�ٶ󺸰�
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            //���� ���� Grenade��� �����ؼ�
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Grenade"))
            {
                //���տ� ���ʹ�.
                grabObject = Instantiate(hitInfo.transform.gameObject);
            }
        }
    }

    private bool GripByOverlap()
    {
        //���� �߽����� �ݰ� 1M���� Grenade�� ã��ʹ�.
        // ���� ����� Grenade�� �����ϰ�ʹ�.
        // ���� ���õ� Grenade�� �ִٸ�
        // �� ���ӿ�����Ʈ�� �ν��Ͻ�ȭ�ؼ� grabObject�� �����ϰ�ʹ�.
        // true�� ��ȯ�ϰ�ʹ�.
        // �׷��� ������ (���õ� Grenade�� ���ٸ�)
        // else �׷��� ������ ���õ� Grenade�� ���ٸ�
        // false�� ��ȯ�ϰ�ʹ�.

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