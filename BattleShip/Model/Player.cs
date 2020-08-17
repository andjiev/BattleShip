﻿using BattleShip.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip.Controller
{
    [Serializable]
    class Player
    {
        protected List<Ship> ships;
        protected List<int> amounts;
        public List<Point> positions;
        public List<Point> missedPositions;
        public Ship selected;
        public static int boardSize = 10;

        protected bool isPlayer;
        
        public Player()
        {
            this.amounts = new List<int>();
            this.positions = new List<Point>();
            this.missedPositions = new List<Point>();
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    positions.Add(new Point { X = i, Y = j });
                }
            }
            this.amounts.Add(3);
            this.amounts.Add(2);
            this.amounts.Add(2);
            this.amounts.Add(1);
            this.amounts.Add(1);
        }

        public void SetGridView(DataGridView grid)
        {
            grid.Rows.Clear();
            grid.RowCount = boardSize;
            grid.ColumnCount = boardSize; ;
            for (int i = 0; i < boardSize; i++)
            {
                grid.Rows[i].Height = 36;
                grid.Columns[i].Width = 36;
            }

            if (!isPlayer)
            {
                grid.ClearSelection();
            }
        }

        public void RemoveDeadPoints(Point position)
        {
            positions.Remove(new Point { X = position.X - 1, Y = position.Y - 1 });
            positions.Remove(new Point { X = position.X - 1, Y = position.Y + 1 });
            positions.Remove(new Point { X = position.X + 1, Y = position.Y - 1 });
            positions.Remove(new Point { X = position.X + 1, Y = position.Y + 1 });
        }

        public void RemoveDeadShip()
        {
            foreach (Point point in selected.viewPoints)
            {
                positions.Remove(point);
            }
        }

        public void Random()
        {
            ships = new List<Ship>();
            bool picked = false;

            for (int i = 4; i >= 0; i--)
            {
                for (int j = 0; j < amounts[i]; j++)
                {
                    while (!picked)
                    {
                        int index = new Random().Next(positions.Count);
                        Ship.View type = (Ship.View)new Random().Next(2);
                        Point position = positions[index];

                        if(!isPlayer && position == new Point(0,0)){
                            continue;
                        }

                        Ship primary = new Ship(i + 1, Color.Blue, position, type);
                        if (isPlayer && ships.Exists(ship => ship.ExistShip(primary)))
                        {
                            primary.ChangePosition(position);
                        }
                        if (!ships.Exists(ship => ship.ExistShip(primary)))
                        {
                            ships.Add(primary);
                            picked = true;
                            RemovePositions(primary);
                        }
                    }

                    picked = false;
                }
            }
            positions = new List<Point>();
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    positions.Add(new Point { X = i, Y = j });
                }
            }
        }


        private void RemovePositions(Ship primary)
        {
            foreach (Point point in primary.viewPoints)
            {
                positions.Remove(point);
            }
        }

        public void UpdateMissed(DataGridView grid)
        {
            foreach (Point position in missedPositions)
            {
                DataGridViewImageCell imgCell = new DataGridViewImageCell();
                imgCell.Value = Properties.Resources.dotImage;
                grid.Rows[position.X].Cells[position.Y] = imgCell;
            }
        }

        public void ShowShips(DataGridView grid)
        {
            if (isPlayer)
            {
                ships.ForEach(ship => ship.ShowShip(grid));
            }
            else
            {
                ships.ForEach(ship => ship.enemyShipsDraw(grid));
            }
        }

        public bool Won()
        {
            return ships.All(ship => ship.Destroyed());
        }
    }
}
