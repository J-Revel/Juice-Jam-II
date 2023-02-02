using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeData", menuName = "ScriptableObjects/Upgrade", order = 1)]
public class Upgrade : ScriptableObject
{
    public enum UpgradeCategory { Dash, Range, Melee, Global}
    // public float upgradeLevel = 1;

    // public UpgradeType PowerUpType;
    public UpgradeCategory type;
    public UpgradeConfig[] upgradeList;
    public Sprite spriteIcon;
    public string title;
    [TextArea]
    public string description;
    public bool isReusable;
}