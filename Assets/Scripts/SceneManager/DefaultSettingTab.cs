using QuantumTek.SimpleMenu;
using UnityEngine;

public class DefaultSettingTab : MonoBehaviour
{
    public SM_TabGroup tabGroup;
    public SM_TabWindow defaultTab;

    void OnEnable()
    {
        tabGroup.ChangeTab(defaultTab);
    }
}
