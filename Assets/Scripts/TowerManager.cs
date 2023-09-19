using UnityEngine;
using UnityEngine.UI;

//타워의 구분자 정보를 지정하고싶다.
//체력을 표현하고싶다.
//타워들의 상황을 UI로 표현하고싶다.
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