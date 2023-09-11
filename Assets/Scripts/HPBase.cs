using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ü���� ǥ���ϰ�ʹ�.
//ü���� ���� �ٲ�� ǥ���ϰ�ʹ�.
//�ʿ���: UI, HP, MaxHp
public class HPBase : MonoBehaviour
{
    private int hp;
    public int maxHP = 2;
    public Slider sliderHP;

    public int HP
    {
        set
        {
            //���� value ���� ���´�.
            hp = value;
            //���� value ���� sliderHP.value�� �ݿ��Ѵ�.
            sliderHP.value = hp;
        }
        get
        {
            //���� hp ���� ��ȯ�Ѵ�.
            return hp;
        }
    }

    private void Start()
    {
        sliderHP.maxValue = maxHP;
        HP = maxHP;
    }
}