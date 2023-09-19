using System;
using UnityEngine;

//���콺 ���ʹ�ư�� ������ ���� ���ʹ�.
//�ʿ��ѿ��: ��, �Ѿ��ڱ� VFX
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
        //���� ���콺 ���ʹ�ư�� ������
        //����ġ���� ���� �չ������� Ray�� �����
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
            {                    //�Ѿ��ڱ� ����
                GameObject bulletImpact = Instantiate(bulletImpactFactory);
                //�Ѿ��ڱ��� �ε��� ���� ����
                bulletImpact.transform.position = hitInfo.point;
                Destroy(bulletImpact, 2f);

                //���� ���� ���� Enemy���
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    //Enemy���� "�ı��Ǿ���" ��� �ϰ�ʹ�.
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
        //��Ʈ�ѷ��� thumbstick�� �и� ���, ���� Ȯ���ϰ�ʹ�. CameraScope�� FOV�� �ǵ帮�ڴ�.
        Vector2 axis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

        cameraScope.fieldOfView -= axis.y * zoomSpeed * Time.deltaTime;
        cameraScope.fieldOfView = Mathf.Clamp(cameraScope.fieldOfView, zoomInFOV, zoomOutFOV);
    }
}