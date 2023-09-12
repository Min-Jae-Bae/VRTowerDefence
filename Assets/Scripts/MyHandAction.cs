using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//G키를 누르면 카메라 위치에서 손 앞방향으로 Ray를 만들고
//바라보보 닿은 것이 Grenade라면 복제해서 내손에 쥐고싶다.
//G키를 때면 잡은 Grenade가 있다면 손 앞 방향으로 던지고싶다.
//필요요소 : 손, 잡은물체
public class MyHandAction : MonoBehaviour
{
    public Transform hand;
    private GameObject grabObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
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
                    grabObject.transform.position = hand.transform.position;
                    grabObject.transform.SetParent(hand.transform.parent);
                }
            }
        }

        //G키를 때면 잡은 Grenade가 있다면
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (grabObject)
            {
                //손 앞 방향으로 던지고싶다.
                var grabRB = grabObject.GetComponent<Rigidbody>();
                grabRB.isKinematic = false;
                grabRB.useGravity = true;
                grabRB.AddForce(hand.forward * 30, ForceMode.Impulse);
                grabObject = null;
            }
        }
    }
}