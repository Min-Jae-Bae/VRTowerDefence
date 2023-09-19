using UnityEngine;

//���콺 ������ ��ư�� ������
//���� ��ġ���� ���� �չ������� Ray�� �߻��ϰ�
//�ε������� �ִٸ� �÷��̾ �װ����� �̵��ϰ�ʹ�.
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
        //���콺 ������ ��ư�� ������ ready ���°��ǰ�

        Ray ray = new Ray(hand.position, hand.forward);
        lineRenderer.SetPosition(0, ray.origin);
        RaycastHit hitInfo;
        bool isHit = Physics.Raycast(ray, out hitInfo);

        if (isHit)
        {
            //��� �ε�����.
            lineRenderer.SetPosition(1, hitInfo.point);
            marker.position = hitInfo.point;
            marker.up = hitInfo.normal;
            marker.localScale = Vector3.one * kAdjust * hitInfo.distance;
        }
        else
        {
            //����̴�. hand�� 100M ��
            lineRenderer.SetPosition(1, ray.origin + ray.direction * 100f);
            marker.position = ray.origin + ray.direction * 100f;
            marker.up = -ray.direction;
            marker.localScale = Vector3.one * kAdjust * 100f;
        }

#if Oculus
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
#else
        if (Input.GetButtonDown("Fire2"))
#endif
        {
            //��� �ε���
            lineRenderer.enabled = true;
        }

#if Oculus
        else if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.LTouch))
#else
            else if (Input.GetButtonUp("Fire2"))
#endif
        {
            //���� ���� �Ⱥ���ʹ�.
            lineRenderer.enabled = false;
            if (isHit)
            {
                //���� �������� Ÿ����� �� Ÿ���� ��ġ�� �̵��ϰ�
                if (hitInfo.collider.CompareTag("Tower"))
                {
                    player.position = hitInfo.collider.transform.position;
                }
                else
                {
                    //�׷��� �ʴٸ� �������� �̵��ϰ�ʹ�.
                    player.position = hitInfo.point;
                }
            }
        }
    }
}