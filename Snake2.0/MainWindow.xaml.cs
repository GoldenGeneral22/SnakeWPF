using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake2._0
{
    public partial class MainWindow : Window
    {
        public enum Direction { Left, Right, Up, Down }

        Direction directionPlayer = Direction.Right;

        Rectangle rec = new Rectangle()
        {
            Width = 30,
            Height = 30,
            Fill = Brushes.LightGreen,
        };

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.125);
            timer.Tick += Timer_Tick;
            timer.Start();
            grid.Children.Add(rec);
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            Movement();
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W && directionPlayer != Direction.Down)
            {
                directionPlayer = Direction.Up;
            }
            else if (e.Key == Key.S && directionPlayer != Direction.Up)
            {
                directionPlayer = Direction.Down;
            }
            else if (e.Key == Key.A && directionPlayer != Direction.Right)
            {
                directionPlayer = Direction.Left;
            }
            else if (e.Key == Key.D && directionPlayer != Direction.Left)
            {
                directionPlayer = Direction.Right;
            }
        }

        private void Movement()
        {
            int playerRow = Grid.GetRow(Player);
            int playerCol = Grid.GetColumn(Player);

            if (directionPlayer == Direction.Up)
            {
                if (Grid.GetRow(Player) - 1 >= 0)
                {
                    Grid.SetRow(Player, Grid.GetRow(Player) - 1);
                }
            }
            else if (directionPlayer == Direction.Down)
            {
                Grid.SetRow(Player, Grid.GetRow(Player) + 1);
            }
            else if (directionPlayer == Direction.Left)
            {
                if (Grid.GetColumn(Player) - 1 >= 0)
                {
                    Grid.SetColumn(Player, Grid.GetColumn(Player) - 1);
                }  
            }
            else
            {
                Grid.SetColumn(Player, Grid.GetColumn(Player) + 1);
            }
            Grid.SetColumn(rec, playerCol);
            Grid.SetRow(rec, playerRow);
        }
    }
}
