using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMenu : MonoBehaviour
{
    private PlayerControls m_UpgradeControls;
    private void Awake()
    {
        GeneralGameBehavior.SwitchState(GeneralGameBehavior.GameState.UPDGRADE_STATE);
        m_UpgradeControls = GeneralGameBehavior.Controls;
    }
}
