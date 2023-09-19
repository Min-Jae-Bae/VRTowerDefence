using UnityEngine;
using UnityEngine.UI;

//Ÿ���� ������ ������ �����ϰ�ʹ�.
//ü���� ǥ���ϰ�ʹ�.
//Ÿ������ ��Ȳ�� UI�� ǥ���ϰ�ʹ�.
public class TowerManager : MonoBehaviour
{
    public Tower[] towers;
    public Slider[] sliderTower;

    private void Awake()
    {
        for (int i = 0; i < towers.Length; i++)
        {
            towers[i].onUpdateTowerInfo = UpdateTowerInfo;
        }
    }

    private void UpdateTowerInfo(Tower tower)
    {
        Slider slider = sliderTower[tower.index];
        slider.value = tower.HP / (float)tower.MaxHP;
    }
}