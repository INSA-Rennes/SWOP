﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class Unit
    {
        protected ITile Position { get; set; } // A position code, or a tile ?
        protected int Atk { get; set; } // Attack Points
        protected int Def { get; set; } // Defense Points
        protected int Hp { get; set; } // Health Points
        protected int Mvt { get; set; } // Movement Points

        protected Unit()
        {
            this.Atk = 2;
            this.Def = 1;
            this.Hp = 2;
            this.Mvt = 1;
        }

        protected Unit(ITile pos)
            : this()
        {
            this.Position = pos;
        }

        // MOVE EXAMPLE : Consider this "pseudo code" for now
        // A Unit can only move one tile per round
        protected virtual void Move(ITile destination)
        {
            bool possibleMove = (this.Mvt != 0)
                && (!destination.IsOccupied());

            if (!possibleMove)
                return;
                
            this.Position.OccupyingUnit = null;
            this.Position = destination;
            destination.OccupyingUnit = this;
        }

        // Attacking is the same for every faction
        protected void Attack(Unit ennemy)
        {
            // TODO
        }
    }

    public class VikingsUnit : Unit
    {
        protected override void Move(ITile destination)
        {
            if (!destination.IsAdjacent(this.Position))
                return;
            base.Move(destination);
            this.Mvt--;
        }

    }

    public class DwarvesUnit : Unit
    {
        // A Dwarf can move from a Mountain to any another Mountain
        // Cannot cross Water
        protected override void Move(ITile destination)
        {
            bool mountainTravel = !destination.IsAdjacent(this.Position)
                && destination.Type != TileType.Mountain
				&& this.Position.Type != TileType.Mountain;

			if (!mountainTravel || destination.Type == TileType.Water)
                return;
            base.Move(destination);
            this.Mvt--;
        }
    }

    public class GaulsUnit : Unit
    {
        public GaulsUnit() 
            : base()
        {
            this.Mvt = 2;
        }

        public GaulsUnit(ITile pos)
            : base(pos)
        {
            this.Mvt = 2;
        }

        protected void Move(ITile destination)
        {
			if (!destination.IsAdjacent(this.Position) || destination.Type == TileType.Water)
                return;
            base.Move(destination);
			if (destination.Type == TileType.Field)
                this.Mvt -= 1;
            else
                this.Mvt -= 2;
        }
    }
}
