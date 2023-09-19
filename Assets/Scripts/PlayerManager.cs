using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Oculus��� �ɺ��� �ִٸ� ��ŧ���� �÷��̾ Ȱ��ȭ�ϰ�ʹ�.
// �׷��� ������ PC�÷��̾ Ȱ��ȭ �ϰ�ʹ�.
// �ݴ��� ���� ��Ȱ��ȭ �ϰ�ʹ�.
public class PlayerManager : MonoBehaviour
{
    public GameObject playerForPC;
    public GameObject playerForOculus;

    private void Awake()
    {
#if Oculus
        playerForPC.SetActive(false);
        playerForOculus.SetActive(true);
#else
        playerForPC.SetActive(true);
        playerForOculus.SetActive(false);
#endif
    }
}