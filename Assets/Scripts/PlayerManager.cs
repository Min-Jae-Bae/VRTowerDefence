using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Oculus라는 심볼이 있다면 오큘러스 플레이어를 활성화하고싶다.
// 그렇지 않으면 PC플레이어를 활성화 하고싶다.
// 반대의 경우는 비활성화 하고싶다.
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