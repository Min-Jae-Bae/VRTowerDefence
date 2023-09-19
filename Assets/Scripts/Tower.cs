using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ü���� ǥ���ϰ�ʹ�.
//�ʿ���: ü��, UI
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