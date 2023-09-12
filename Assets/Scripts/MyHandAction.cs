using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GŰ�� ������ ī�޶� ��ġ���� �� �չ������� Ray�� �����
//�ٶ󺸺� ���� ���� Grenade��� �����ؼ� ���տ� ���ʹ�.
//GŰ�� ���� ���� Grenade�� �ִٸ� �� �� �������� ������ʹ�.
//�ʿ��� : ��, ������ü
public class MyHandAction : MonoBehaviour
{
    public Transform hand;
    private GameObject grabObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
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
                    grabObject.transform.position = hand.transform.position;
                    grabObject.transform.SetParent(hand.transform.parent);
                }
            }
        }

        //GŰ�� ���� ���� Grenade�� �ִٸ�
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (grabObject)
            {
                //�� �� �������� ������ʹ�.
                var grabRB = grabObject.GetComponent<Rigidbody>();
                grabRB.isKinematic = false;
                grabRB.useGravity = true;
                grabRB.AddForce(hand.forward * 30, ForceMode.Impulse);
                grabObject = null;
            }
        }
    }
}