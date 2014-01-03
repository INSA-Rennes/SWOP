﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SmallWorld;

namespace SWOP {

    /// <summary>
    /// Logique d'interaction pour Tile.xaml
    /// </summary>
    public partial class TileView : UserControl {
        protected enum TileViewState
        {
            Idle,
            Over,
            Selected,
            SelectedOver
        }

        public ITile Tile { get; private set; }

        private TileViewState currentState;

        static Random random = new Random();
        

        public TileView(ITile t) {
            InitializeComponent();

            Tile = t;
        }


		/// <summary>
		/// Init position of the tile
		/// </summary>
        private void OnTileLoaded(object sender, RoutedEventArgs e)
        {
            // Set position (hexagon disposition)
            TranslateTransform trTns = new TranslateTransform(Tile.X * 60 + ((Tile.Y % 2 == 0) ? 0 : 30), Tile.Y * 50);
            TransformGroup trGrp = new TransformGroup();
            trGrp.Children.Add(trTns);

            grid.RenderTransform = trGrp;

            currentState = TileViewState.Idle;
            SetGround();
        }


		/// <summary>
		/// Change view of the tile
		/// </summary>
		/// <param name="newState"></param>
        protected void SetAppearance(TileViewState newState)
        {
            if (newState == currentState)
                return;

            switch(newState)
            {
                case TileViewState.Idle:
                    hexagon.Opacity = 1;
                    break;
                case TileViewState.Over:
                    hexagon.Opacity = 0.9;
                    break;
                case TileViewState.Selected:
                    hexagon.Opacity = 0.8;
                    break;
                case TileViewState.SelectedOver:
                    hexagon.Opacity = 0.7;
                    break;
                default:
                    throw new NotImplementedException();
            }

            currentState = newState;
        }


        public void SetGround() {
            string brushPath = "Brush"; // set to "Brush" or "BrushImg"

            switch (Tile.Type)
            {
                case TileType.Field:
                    hexagon.Fill = (Brush) Resources[brushPath + "Field"];
                    break;
                case TileType.Mountain:
                    hexagon.Fill = (Brush) Resources[brushPath + "Moutain"];
                    break;
                case TileType.Desert:
                    hexagon.Fill = (Brush) Resources[brushPath + "Desert"];
                    break;
                case TileType.Forest:
                    hexagon.Fill = (Brush) Resources[brushPath + "Forest"];
                    break;
                case TileType.Water:
                    hexagon.Fill = (Brush) Resources[brushPath + "Water"];
                    break;
            }
        }

        /// <summary>
        /// Randomize aspect of multiple Units on the same Tile
        /// </summary>
        public void DispatchArmy()
        {
            IEnumerable<UnitView> uViews = this.grid.Children.OfType<UnitView>();

            if (uViews.Count() < 2)
                return;

            foreach (UnitView uView in uViews)
            {
				int randMarginX = random.Next(-10, 15);
				int randMarginY = random.Next(-20, 15);

                uView.Margin = new Thickness(randMarginX, randMarginY, - randMarginX, - randMarginY);
            }
        }

        #region Events

		/// <summary>
		/// Player left-clik on the tile => select a unit on it
		/// </summary>
        private void Tile_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            MapView mapView = MainWindow.INSTANCE.MapView;
			if (mapView.SelectedTileView != this)
			{
				if (mapView.SelectedTileView != null)
					mapView.SelectedTileView.SetAppearance(TileViewState.Idle);

				mapView.SelectedTileView = this;
				SetAppearance(TileViewState.Selected);
			}

			// Unit selection
			if (this.Tile.IsOccupied())
			{
				IEnumerable<UnitView> uViews = this.grid.Children.OfType<UnitView>();
				if (uViews.Count() <= 1)
				{
					uViews.ElementAt(0).Select();
				}
				else
				{
					int idToSelect = 0;
					for (int i = 0; i < uViews.Count(); i++)
					{
						if (uViews.ElementAt(i).Unit.State == UnitState.Selected)
						{
							idToSelect = (i + 1) % uViews.Count();
							break;
						}
					}
					uViews.ElementAt(idToSelect).Select();
				}
			}
			else if (MainWindow.INSTANCE.ActiveUnitView != null)
			{
				MainWindow.INSTANCE.ActiveUnitView.Unselect();
			}
        }

		/// <summary>
		/// Player right-clik => move the selected unit, or attack
		/// </summary>
        private void Tile_MouseRightButtonDown(object sender, MouseEventArgs e)
        {
            if (MainWindow.INSTANCE.ActiveUnitView != null)
            {
                // Try to move
                MainWindow.INSTANCE.ActiveUnitView.Unit.Move(this.Tile);

                // Try to attack
                int maxDef = -1;
                IUnit bestDefUnit = null;
                foreach (IUnit unit in this.Tile.OccupyingUnits)
                {
                    if (unit.Def > maxDef)
                    {
                        bestDefUnit = unit;
                        maxDef = unit.Def;
                    }
                }
                if (bestDefUnit != null)
                {
                    Console.WriteLine("Defender : " + bestDefUnit.Name);
                    MainWindow.INSTANCE.ActiveUnitView.Unit.Attack(bestDefUnit);
                    MainWindow.INSTANCE.RefreshUI();
                }

                MapView mapView = MainWindow.INSTANCE.MapView;
                if (mapView.SelectedTileView != null)
                {
                    mapView.SelectedTileView.SetAppearance(TileViewState.Idle);
                    mapView.SelectedTileView = null;
                }
            }
        }


		/// <summary>
		/// Mouse over the tile
		/// </summary>
        private void Tile_MouseEnter(object sender, MouseEventArgs e)
        {
            if (MainWindow.INSTANCE.MapView.SelectedTileView == this)
                SetAppearance(TileViewState.SelectedOver);
            else
                SetAppearance(TileViewState.Over);
        }


		/// <summary>
		/// Mouse quit 'overred' tile
		/// </summary>
        private void Tile_MouseLeave(object sender, MouseEventArgs e)
        {
            if (MainWindow.INSTANCE.MapView.SelectedTileView == this)
                SetAppearance(TileViewState.Selected);
            else
                SetAppearance(TileViewState.Idle);

            /*foreach (ITile t in Tile.AdjacentsTiles)
            {
                MainWindow.INSTANCE.MapView.TilesView[t].Show();
            }*/
        }

        #endregion

    }
}
