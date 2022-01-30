using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
namespace Snake2._0
{
    public partial class MainWindow : Window
    {
        readonly DispatcherTimer timer = new DispatcherTimer();
        readonly Random random = new Random();
        public enum Direction { Left, Right, Up, Down }
        Direction directionPlayer = Direction.Right, directionOld = Direction.Right;

        int snakeLength = 2, mode = 2, scoreValue = 0, highscore = 0;

        string? remainderText, qoutientText;

        float timerOfTime, timeSpeed = 0.125f;

        bool alreadyPlaying = false;

        Vector newPosition, positionPlayer, positionFruit;

        Queue<Rectangle> rectRec = new Queue<Rectangle>();
        Queue<Vector> rectVecs = new Queue<Vector>();

        public MainWindow()
        {
            timer.Tick += Timer_Tick;
            InitializeComponent();
        }

        void Timer_Tick(object sender, EventArgs e)                         //loop repeats every timer.Interval Seconds
        {
            positionPlayer = new Vector(Grid.GetRow(Player), Grid.GetColumn(Player));
            positionFruit = new Vector(Grid.GetRow(Fruit), Grid.GetColumn(Fruit));

            timerOfTime += timeSpeed;
            int qoutient = Math.DivRem((int)timerOfTime, 60, out int remainder);             // changing Timer by determining Minutes and Seconds
            if(remainder < 10)
            {
                remainderText = "0" + remainder.ToString();
            }
            else
            {
                remainderText = remainder.ToString();
            }

            if (qoutient < 10)
            {
                qoutientText = "0" + qoutient.ToString();
            }
            else
            {
                qoutientText = qoutient.ToString();
            }
            Timer.Content = qoutientText + ":" + remainderText;

            Movement();
            SpawnTail();
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)             //KeyDown event for controls called when a key is pressed
        {
            if ((e.Key == Key.W || e.Key == Key.Up) && directionPlayer != Direction.Down && directionOld != Direction.Down)                 // Direction contols
            {
                directionPlayer = Direction.Up;
            }
            else if ((e.Key == Key.S || e.Key == Key.Down) && directionPlayer != Direction.Up && directionOld != Direction.Up)
            {
                directionPlayer = Direction.Down;
            }
            else if ((e.Key == Key.A || e.Key == Key.Left) && directionPlayer != Direction.Right && directionOld != Direction.Right)
            {
                directionPlayer = Direction.Left;
            }
            else if ((e.Key == Key.D || e.Key == Key.Right) && directionPlayer != Direction.Left && directionOld != Direction.Left)
            {
                directionPlayer = Direction.Right;
            }
            else if(e.Key == Key.Space)                                          // Start Control
            {
                if(!alreadyPlaying)
                {
                    StartGame();
                }
            }
            else if(e.Key == Key.D1 || e.Key == Key.NumPad1)                       // Mode control
            {
                if(!alreadyPlaying)
                {
                    timeSpeed = 0.25f;
                    DifficultyLevel.Content = "1: slow";
                    mode = 1;
                }
            }
            else if(e.Key == Key.D2 || e.Key == Key.NumPad2)
            {
                if (!alreadyPlaying)
                {
                    timeSpeed = 0.125f;
                    DifficultyLevel.Content = "2: medium";
                    mode = 2;
                }
            }
            else if (e.Key == Key.D3 || e.Key == Key.NumPad3)
            {
                if (!alreadyPlaying)
                {
                    timeSpeed = 0.065f;
                    DifficultyLevel.Content = "3: fast";
                    mode = 3;
                }
            }
        }

        private void Movement()                                      //used for ´moving the head of the snake and checking for the collision with other gameobjects called every Frame
        {
            if (CheckSpawn(positionPlayer))                           // checking if head collided with body
            {
                EndGame();
                return;
            }
            else if (positionPlayer == positionFruit)                     // checking if Fruit was collected
            {
                do
                {
                    newPosition = new Vector(random.Next(grid.RowDefinitions.Count), random.Next(grid.ColumnDefinitions.Count));
                } while (newPosition == positionPlayer || CheckSpawn(newPosition) == true);

                Grid.SetRow(Fruit, (int)newPosition.X);
                Grid.SetColumn(Fruit, (int)newPosition.Y);

                snakeLength++;
                scoreValue += 1 * mode;
                Score.Content = "Score: " + scoreValue;
            }

            switch (directionPlayer)                                       // moving the player in the direction that was determined in keydownEvent and checking if head collided with the Walls
            {
                case Direction.Up:
                    if (positionPlayer.X - 1 >= 0)
                    {
                        Grid.SetRow(Player, (int)positionPlayer.X - 1);
                    }
                    else { EndGame(); }
                    break;

                case Direction.Down:
                    if (positionPlayer.X + 1 < grid.RowDefinitions.Count)
                    {
                        Grid.SetRow(Player, (int)positionPlayer.X + 1);
                    }
                    else { EndGame(); }
                    break;

                case Direction.Left:
                    if (positionPlayer.Y - 1 >= 0)
                    {
                        Grid.SetColumn(Player, (int)positionPlayer.Y - 1);
                    }
                    else { EndGame(); }
                    break;

                case Direction.Right:
                    if(positionPlayer.Y + 1 < grid.ColumnDefinitions.Count)
                    {
                        Grid.SetColumn(Player, (int)positionPlayer.Y + 1);
                    }
                    else { EndGame(); }
                    break;
            }
            directionOld = directionPlayer;
        }

        private void SpawnTail()                                                    //used to render the tail of the snake
        {
            Rectangle rect = new Rectangle               // creating a prefab for the snake body
            {
                Width = 30,
                Height = 30,
                Fill = Brushes.LightGreen
            };

            grid.Children.Add(rect);                          // creating a new body part
            rectRec.Enqueue(rect);
            Grid.SetRow(rect, (int)positionPlayer.X);
            Grid.SetColumn(rect, (int)positionPlayer.Y);
            rectVecs.Enqueue(positionPlayer);

            if (rectRec.Count > snakeLength)                  // deleting a body part if the body is too long
            {
                grid.Children.Remove(rectRec.Dequeue());
                rectVecs.Dequeue();
            }
        }

        private bool CheckSpawn(Vector positionToCheck)                              //checking if one of the body parts is on the positionToCheck Vector2
        {
            bool check = false;
            foreach(Vector vector in rectVecs)
            {
                if(vector == positionToCheck)
                {
                    check = true;
                }
            }
            return check;
        }

        private void StartGame()                                                     //resets all objects so the game can start again
        {
            snakeLength = 2;
            timerOfTime = 0;
            scoreValue = 0;

            Start.Visibility = Visibility.Collapsed;
            Difficulty.Visibility = Visibility.Collapsed;
            DifficultyLevel.Visibility = Visibility.Collapsed;

            alreadyPlaying = true;
            Score.Content = "Score: 0";

            while (rectRec.Count >= 1)
            {
                grid.Children.Remove(rectRec.Dequeue());
            }

            rectVecs.Clear();
            rectRec.Clear();

            Grid.SetRow(Player, 10);
            Grid.SetColumn(Player, 5);
            directionPlayer = Direction.Right;

            Grid.SetRow(Fruit, 10);
            Grid.SetColumn(Fruit, 14);

            timer.Interval = TimeSpan.FromSeconds(timeSpeed);
            timer.Start();
        }

        private void EndGame()                                                       //checks if a new highscore must be saved and shows death message
        {
            if(scoreValue > highscore)
            {
                highscore = scoreValue;
                Highscore.Content = "Highscore: " + highscore;
            }

            alreadyPlaying = false;

            Start.Visibility = Visibility.Visible;
            Difficulty.Visibility = Visibility.Visible;
            DifficultyLevel.Visibility = Visibility.Visible;

            timer.Stop();
        }
    }
}
