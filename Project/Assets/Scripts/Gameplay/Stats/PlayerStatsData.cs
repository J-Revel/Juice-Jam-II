using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerStatValue
{
    public PlayerStatCategory category;
    public int level;
}

public class PlayerStatsData : MonoBehaviour
{
    public static PlayerStatsData instance;
    private List<PlayerStatValue> playerStats = new List<PlayerStatValue>();
    public List<Upgrade> upgrades;
    public System.Action statChangedDelegate;
    
    [System.NonSerialized]
    public float lastUpdateTime = 1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateStats();
    }

    public PlayerStatValue[] stats {
        get {
            return playerStats.ToArray();
        }
    }

    public void ApplyPowerUp(UpgradeConfig upgrade)
    {
        AddStat(upgrade.stat, upgrade.level);
        statChangedDelegate?.Invoke();
    }

    public float GetStatValue(PlayerStatCategory category)
    {
        foreach(PlayerStatValue stat in playerStats)
        {
            if(stat.category == category)
            {
                return stat.category.Evaluate(stat.level);
            }
        }
        return category.Evaluate(0);
    }

    public bool HasStat(PlayerStatCategory category)
    {
        foreach(PlayerStatValue stat in playerStats)
        {
            if(stat.category == category)
            {
                return true;
            }
        }
        return false;
    }

    public void AddStat(PlayerStatCategory category, int amount)
    {
        foreach(PlayerStatValue stat in playerStats)
        {
            if(stat.category == category)
            {
                stat.level += amount;
                return;
            }
        }
        PlayerStatValue newValue = new PlayerStatValue();
        newValue.level = amount;
        newValue.category = category;
        playerStats.Add(newValue);
        statChangedDelegate?.Invoke();
    }

    public void UpdateStats()
    {
        playerStats.Clear();
        Dictionary<PlayerStatCategory, int> statIndexes = new Dictionary<PlayerStatCategory, int>();
        foreach(Upgrade upgrade in upgrades)
        {
            if(upgrade == null)
                continue;
            foreach(UpgradeConfig config in upgrade.upgradeList)
            {
                if(config.stat == null)
                    continue;
                if(!statIndexes.ContainsKey(config.stat))
                {
                    statIndexes[config.stat] = playerStats.Count;
                    PlayerStatValue value = new PlayerStatValue();
                    value.category = config.stat;
                    value.level = 0;
                    playerStats.Add(value);
                }
                playerStats[statIndexes[config.stat]].level += config.level;
            }
        }
        statChangedDelegate?.Invoke();
        lastUpdateTime = Time.time;
    }

    public void ApplyUpgrade(Upgrade upgrade)
    {
        upgrades.Add(upgrade);
        UpdateStats();
    }

    // public void AddStatMultiplier(PlayerStatCategory category, int amount)
    // {
    //     if(!playerStatLocations.ContainsKey(category))
    //     {
    //         PlayerStatValue newValue = new PlayerStatValue();
    //         newValue.level = 0;
    //         newValue.category = category;
    //         playerStats.Add(newValue);
    //         playerStatLocations[category] = playerStats.Count - 1;
    //     }
    //     playerStats[playerStatLocations[category]].multiplier += amount;
    // }
    
}

[System.Serializable]
public struct UpgradeConfig
{
    public PlayerStatCategory stat;
    public int level;
}


