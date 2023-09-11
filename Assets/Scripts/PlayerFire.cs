using UnityEngine;

//마우스 왼쪽버튼을 누르면 총을 쏘고싶다.
//필요한요소: 손, 총알자국 VFX
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
        //만약 마우스 왼쪽버튼을 누르면
        //손위치에서 손의 앞방향으로 Ray를 만들고
        Ray ray = new Ray(hand.position, hand.forward);
        lr.SetPosition(0, ray.origin);
        RaycastHit hitInfo;
        bool isHit = Physics.Raycast(ray, out hitInfo);

        if (isHit)
        {
            lr.SetPosition(1, hitInfo.point);
            if (Input.GetButtonDown("Fire1"))
            {                    //총알자국 생성
                GameObject bulletImpact = Instantiate(bulletImpactFactory);
                //총알자국을 부딪힌 곳에 생성
                bulletImpact.transform.position = hitInfo.point;
                Destroy(bulletImpact, 2f);

                //만약 닿은 것이 Enemy라면
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    //Enemy에게 "파괴되어줘" 라고 하고싶다.
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