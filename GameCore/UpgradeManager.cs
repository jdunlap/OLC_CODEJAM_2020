﻿using GameCore.Entities;
using PandaMonogame.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public enum UpgradeType
    {
        MinerCap1, // +5 Max Miners
        MinerCap2, // +10 Max Miners
        MiningRate1, // +5 Mining Rate
        MiningRate2, // +5 Mining Rate
        RepairRate, // +10 Repair Rate
        ShieldRegen1, // +5 Shield Regen
        ShieldRegen2, // +10 Shield Regen
        Warmachine1, // + Warmachine beams
        Warmachine2, // + Warmachine missiles
        Hyperdrive, // Fix Hyperdrive (Victory)
    }

    public class UpgradeManager
    {
        public Dictionary<UpgradeType, int> UpgradeCosts = new Dictionary<UpgradeType, int>()
        {
            { UpgradeType.MinerCap1, 100 },
            { UpgradeType.MinerCap2, 250 },
            { UpgradeType.MiningRate1, 150 },
            { UpgradeType.MiningRate2, 300 },
            { UpgradeType.RepairRate, 200 },
            { UpgradeType.ShieldRegen1, 200 },
            { UpgradeType.ShieldRegen2, 250 },
            { UpgradeType.Warmachine1, 250 },
            { UpgradeType.Warmachine2, 250 },
            { UpgradeType.Hyperdrive, 500 },
        };

        public Dictionary<UpgradeType, bool> UpgradesUnlocked = new Dictionary<UpgradeType, bool>()
        {
            { UpgradeType.MinerCap1, false },
            { UpgradeType.MinerCap2, false },
            { UpgradeType.MiningRate1, false },
            { UpgradeType.MiningRate2, false },
            { UpgradeType.RepairRate, false },
            { UpgradeType.ShieldRegen1, false },
            { UpgradeType.ShieldRegen2, false },
            { UpgradeType.Warmachine1, false },
            { UpgradeType.Warmachine2, false },
            { UpgradeType.Hyperdrive, false },
        };

        public Dictionary<UpgradeType, PUIWBasicButton> UpgradeButtons;

        #region menu button methods
        public void UpgradeMinerCap1(params object[] args)
        {
            UnlockUpgrade(UpgradeType.MinerCap1);
        }

        public void UpgradeMinerCap2(params object[] args)
        {
            UnlockUpgrade(UpgradeType.MinerCap2);
        }

        public void UpgradeMiningRate1(params object[] args)
        {
            UnlockUpgrade(UpgradeType.MiningRate1);
        }

        public void UpgradeMiningRate2(params object[] args)
        {
            UnlockUpgrade(UpgradeType.MiningRate2);
        }

        public void UpgradeRepairRate(params object[] args)
        {
            UnlockUpgrade(UpgradeType.RepairRate);
        }

        public void UpgradeShieldRegen1(params object[] args)
        {
            UnlockUpgrade(UpgradeType.ShieldRegen1);
        }

        public void UpgradeShieldRegen2(params object[] args)
        {
            UnlockUpgrade(UpgradeType.ShieldRegen2);
        }

        public void UpgradeWarmachine1(params object[] args)
        {
            UnlockUpgrade(UpgradeType.Warmachine1);
        }

        public void UpgradeWarmachine2(params object[] args)
        {
            UnlockUpgrade(UpgradeType.Warmachine2);
        }

        public void UpgradeHyperdrive(params object[] args)
        {
            UnlockUpgrade(UpgradeType.Hyperdrive);
        }
        #endregion

        public int UpgradePoints;

        public int BonusMaxMiners;
        public int BonusMiningRate;
        public float BonusRepairRate;
        public float BonusShieldRegen;

        public UpgradeManager()
        {
        }

        public void Load()
        {
            UpgradeButtons = new Dictionary<UpgradeType, PUIWBasicButton>();

            foreach (var kvp in UpgradeCosts)
            {
                var button = GameplayState.Menu.GetWidget<PUIWBasicButton>("btnUpgrade" + kvp.Key.ToString());
                button.SetTooltip(kvp.Value.ToString() + " points", kvp.Value.ToString() + " points" + (kvp.Key == UpgradeType.Hyperdrive ? "\nMust unlock all other upgrades." : ""));
                UpgradeButtons.Add(kvp.Key, button);
            }
        }

        public void UnlockUpgrade(UpgradeType type)
        {
            if (UpgradesUnlocked[type])
                return;
            if (UpgradePoints < UpgradeCosts[type])
                return;

            switch (type)
            {
                case UpgradeType.MinerCap1:
                    {
                        BonusMaxMiners += 5;
                    }
                    break;

                case UpgradeType.MinerCap2:
                    {
                        BonusMaxMiners += 10;
                    }
                    break;

                case UpgradeType.MiningRate1:
                    {
                        BonusMiningRate += 5;

                        foreach (Miner miner in GameplayState.WorldManager.Miners)
                        {
                            miner.GatherRate += 5;
                        }
                    }
                    break;

                case UpgradeType.MiningRate2:
                    {
                        BonusMiningRate += 10;

                        foreach (Miner miner in GameplayState.WorldManager.Miners)
                        {
                            miner.GatherRate += 10;
                        }
                    }
                    break;

                case UpgradeType.RepairRate:
                    {
                        BonusRepairRate += 25;

                        foreach (var ship in GameplayState.WorldManager.PlayerShips)
                        {
                            if (ship is RepairShip repairShip)
                            {
                                repairShip.RepairRate += 25;
                            }
                        }
                    }
                    break;

                case UpgradeType.ShieldRegen1:
                    {
                        BonusShieldRegen += 10;
                    }
                    break;

                case UpgradeType.ShieldRegen2:
                    {
                        BonusShieldRegen += 20;
                    }
                    break;

                case UpgradeType.Warmachine1:
                    {
                        var warmachine = GameplayState.WorldManager.PlayerEntity;
                        var newWeapons = new List<Weapon>();

                        foreach (var weapon in warmachine.Weapons)
                        {
                            if (weapon.ProjectileType.Contains("Laser"))
                            {
                                for (var i = 0; i < 4; i++)
                                {
                                    newWeapons.Add(new Weapon()
                                    {
                                        ProjectileType = weapon.ProjectileType,
                                        MountType = weapon.MountType,
                                        TargetType = weapon.TargetType,
                                        Range = weapon.Range,
                                        Cooldown = weapon.Cooldown,
                                        Damage = weapon.Damage,
                                        MaxAngle = weapon.MaxAngle,
                                    });
                                }
                            }
                        }

                        foreach (var weapon in newWeapons)
                            warmachine.Weapons.Add(weapon);
                    }
                    break;

                case UpgradeType.Warmachine2:
                    {
                        var warmachine = GameplayState.WorldManager.PlayerEntity;
                        var newWeapons = new List<Weapon>();

                        foreach (var weapon in warmachine.Weapons)
                        {
                            if (weapon.ProjectileType.Contains("Torpedo"))
                            {
                                for (var i = 0; i < 4; i++)
                                {
                                    newWeapons.Add(new Weapon()
                                    {
                                        ProjectileType = weapon.ProjectileType,
                                        MountType = weapon.MountType,
                                        TargetType = weapon.TargetType,
                                        Range = weapon.Range,
                                        Cooldown = weapon.Cooldown,
                                        Damage = weapon.Damage,
                                        MaxAngle = weapon.MaxAngle,
                                    });
                                }
                            }
                        }

                        foreach (var weapon in newWeapons)
                            warmachine.Weapons.Add(weapon);
                    }
                    break;

                case UpgradeType.Hyperdrive:
                    {
                        var otherUpgrades = true;

                        foreach (var kvp in UpgradesUnlocked)
                        {
                            if (kvp.Key != UpgradeType.Hyperdrive && kvp.Value == false)
                                otherUpgrades = false;
                        }

                        if (!otherUpgrades)
                            return;

                        GameplayState.GameWon = true;
                    }
                    break;
            }

            UpgradePoints -= UpgradeCosts[type];

            UpgradesUnlocked[type] = true;
            UpgradeButtons[type].Visible = false;
            UpgradeButtons[type].Active = false;
        }
    }
}
