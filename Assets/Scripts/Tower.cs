using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//체력을 표현하고싶다.
//필요요소: 체력, UI
public class Tower : MonoBehaviour
{
    public int index, HP, MaxHP = 50;

    public Action<Tower> onUpdateTowerInfo;

    private void Start()
    {
        HP = MaxHP;
        onUpdateTowerInfo(this);
    }

    internal void OnDamageProcess()
    {
        HP -= 1;
        onUpdateTowerInfo(this);
    }
}