using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Upgrade Build", menuName = "ScriptableObjects/Upgrade Build")]
public class UpgradeBuild : ScriptableObject
{
    public Upgrade[] upgrades;
    
}