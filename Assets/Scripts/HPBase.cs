using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//체력을 표현하고싶다.
//체력의 값이 바뀌면 표현하고싶다.
//필요요소: UI, HP, MaxHp
public class HPBase : MonoBehaviour
{
    private int hp;
    public int maxHP = 2;
    public Slider sliderHP;

    public int HP
    {
        set
        {
            //현재 value 값을 놓는다.
            hp = value;
            //현재 value 값을 sliderHP.value에 반영한다.
            sliderHP.value = hp;
        }
        get
        {
            //현재 hp 값을 반환한다.
            return hp;
        }
    }

    private void Start()
    {
        sliderHP.maxValue = maxHP;
        HP = maxHP;
    }
}