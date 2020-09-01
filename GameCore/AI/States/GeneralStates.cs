﻿using GameCore.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.AI
{
    public class ShipStateBase : SimpleStateBase
    {
        public Ship ParentShip;

        public ShipStateBase(string name, Ship parentShip) : base(name) { ParentShip = parentShip; }
    }

    public class ShipStateTimedBase : SimpleStateTimedBase
    {
        public Ship ParentShip;

        public ShipStateTimedBase(string name, Ship parentShip) : base(name) { ParentShip = parentShip; }
    }

    public class ShipIdleState : ShipStateBase
    {
        public ShipIdleState(Ship parentShip) : base("Idle", parentShip) { }

        public override void Begin()
        {
            ParentShip.StopMovement();
        }
    }

    public class ShipFollowingState : ShipStateBase
    {
        public Ship Target;
        public float FollowDistance;
        public bool Waiting = false;

        public ShipFollowingState(Ship parentShip) : base("Following", parentShip) { }

        public override void Begin()
        {
            ParentShip.SetDestination(Target.Position);
        }

        public override void Update(GameTime gameTime)
        {
            if (Vector2.Distance(Target.Position, ParentShip.Position) <= FollowDistance)
            {
                ParentShip.StopMovement();
                Waiting = true;
            }
            else
            {
                if (Waiting)
                {
                    if (Vector2.Distance(Target.Position, ParentShip.Position) >= FollowDistance * 2)
                    {
                        Waiting = false;
                    }
                }
                else
                {
                    ParentShip.SetDestination(Target.Position);
                }
            }
        }

        public override void End()
        {
        }
    }

    public class ShipPatrolFollowState : ShipStateBase
    {
        public Ship Target;

        public ShipPatrolFollowState(Ship parentShip) : base("PatrolFollow", parentShip) { }

        public override void Begin()
        {
            ParentShip.SetDestination(Target.Position);
        }

        public override void Update(GameTime gameTime)
        {
            ParentShip.SetDestination(Target.Position);
        }
    }

    public class SmallShipAttackingState : ShipStateBase
    {
        public Ship Target;

        public SmallShipAttackingState(Ship parentShip) : base("Attacking", parentShip) { }

        public override void Begin()
        {
            ParentShip.SetDestination(Target.Position);
        }

        public override void Update(GameTime gameTime)
        {
            ParentShip.SetDestination(Target.Position);

            foreach (var weapon in ParentShip.Weapons)
            {
                if (Vector2.Distance(Target.Position, ParentShip.Position) <= weapon.Range && weapon.CurrentCooldown <= 0)
                {
                    var angle = MathF.Abs(ParentShip.GetAngleToTarget(Target.Position) - ParentShip.Rotation);

                    if (angle <= weapon.MaxAngle)
                    {
                        //Target.TakeDamage(weapon.Damage);
                        GameplayState.ProjectileManager.FireProjectile(ProjectileType.Laser, ParentShip, Target, weapon.Damage);
                        weapon.CurrentCooldown = weapon.Cooldown;
                    }
                }
            }
        }
    }
}
