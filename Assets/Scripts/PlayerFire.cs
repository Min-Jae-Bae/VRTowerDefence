using UnityEngine;

//���콺 ���ʹ�ư�� ������ ���� ���ʹ�.
//�ʿ��ѿ��: ��, �Ѿ��ڱ� VFX
public class PlayerFire : MonoBehaviour
{
    public Transform hand;
    public GameObject bulletImpactFactory;
    private LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        //���� ���콺 ���ʹ�ư�� ������
        //����ġ���� ���� �չ������� Ray�� �����
        Ray ray = new Ray(hand.position, hand.forward);
        lr.SetPosition(0, ray.origin);
        RaycastHit hitInfo;
        bool isHit = Physics.Raycast(ray, out hitInfo);

        if (isHit)
        {
            lr.SetPosition(1, hitInfo.point);
            if (Input.GetButtonDown("Fire1"))
            {                    //�Ѿ��ڱ� ����
                GameObject bulletImpact = Instantiate(bulletImpactFactory);
                //�Ѿ��ڱ��� �ε��� ���� ����
                bulletImpact.transform.position = hitInfo.point;
                Destroy(bulletImpact, 2f);

                //���� ���� ���� Enemy���
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    //Enemy���� "�ı��Ǿ���" ��� �ϰ�ʹ�.
                    hitInfo.transform.GetComponent<Enemy>().DiePlz(1);
                }
            }
        }
        else
        {
            lr.SetPosition(1, ray.origin + ray.direction * 100f);
        }
    }
}