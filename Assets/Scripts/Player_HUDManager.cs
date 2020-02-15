using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Player : MonoBehaviour
{
    [Header("HUD UI")]
    [SerializeField] Image ui_HPBar;
    [SerializeField] Image ui_SPBar;
    [SerializeField] Text ui_HPText;
    [SerializeField] Text ui_SPText;

    public void Set_FillAmount_HPBar()
    {
        ui_HPBar.fillAmount = (float)currHp / (float)maxHp;
        ui_HPText.text = currHp + " / " + maxHp;
    }

    public void Set_FillAmount_SPBar()
    {
        ui_SPBar.fillAmount = (float)currSp / (float)maxSp;
        ui_SPText.text = currSp + " / " + maxSp;
    }
}
