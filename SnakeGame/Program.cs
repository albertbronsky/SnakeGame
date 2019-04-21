﻿﻿﻿using System;
using System.IO;
using System.Media;

namespace SnakeGame
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            int[] xPosition = new int[50];
                xPosition[0] = 30;
            int[] yPosition = new int[50];
                yPosition[0] = 15;
            int appleXDim = 10;
            int appleYDim = 10;
            int applesEaten = 0;

            decimal gameSpeed = 150m;
            
            bool isGameOn = true;
            bool isWallHit = false;
            bool isAppleEaten = false;
            
            var basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            SoundPlayer player = new SoundPlayer();
            
            Random random = new Random();

            Console.CursorVisible = false;
            
            paintSnake(applesEaten, xPosition, yPosition, out xPosition, out yPosition);
            
            setApplePositionOnScreen(random, out appleXDim, out appleYDim);
            paintApple(appleXDim, appleYDim);
            
            BuildWall();

            ConsoleKey command = Console.ReadKey().Key;
            
            do
            {
                switch (command)
                {
                    case ConsoleKey.LeftArrow:
                        Console.SetCursorPosition(xPosition[0], yPosition[0]);
                        Console.Write(" ");
                        xPosition[0]--;
                        break;
                    case ConsoleKey.UpArrow:
                        Console.SetCursorPosition(xPosition[0], yPosition[0]);
                        Console.Write(" ");
                        yPosition[0]--;
                        break;
                    case ConsoleKey.RightArrow:
                        Console.SetCursorPosition(xPosition[0], yPosition[0]);
                        Console.Write(" ");
                        xPosition[0]++;
                        break;
                    case ConsoleKey.DownArrow:
                        Console.SetCursorPosition(xPosition[0], yPosition[0]);
                        Console.Write(" ");
                        yPosition[0]++;
                        break;
                }

                paintSnake(applesEaten, xPosition, yPosition, out xPosition, out yPosition);
                
                isWallHit = DidSnakeHitWall(xPosition[0], yPosition[0]);

                if (isWallHit)
                {
                    player.SoundLocation = Path.Combine(basePath, @"./../../sounds/hit.wav");
                    player.Play();
                    
                    isGameOn = false;
                    Console.SetCursorPosition(15, 10);
                    Console.WriteLine("The snake hit the wall and died.");
                }

                isAppleEaten = determineIfAppleWasEaten(xPosition[0], yPosition[0], appleXDim, appleYDim);

                if (isAppleEaten)
                {
                    setApplePositionOnScreen(random, out appleXDim, out appleYDim);
                    paintApple(appleXDim, appleYDim);
                    applesEaten++;
                    
                    player.SoundLocation = Path.Combine(basePath, @"./../../sounds/apple.wav");
                    player.Play();
                    
                    gameSpeed *= .925m;
                }
                
                if (Console.KeyAvailable) command = Console.ReadKey().Key;
                System.Threading.Thread.Sleep(Convert.ToInt32(gameSpeed));

            } while (isGameOn);
        }

        private static void paintSnake(int applesEaten, int[] xPositionIn, int[] yPositionIn, out int[] xPositionOut, out int[] yPositionOut)
        {
            // Paint the head
            Console.SetCursorPosition(xPositionIn[0], yPositionIn[0]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("O");
            
            // Paint the body
            for (int i = 1; i < applesEaten + 1; i++)
            {
                Console.SetCursorPosition(xPositionIn[i],yPositionIn[i]);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("o");
            }
          
            // Erase last part of snake
            Console.SetCursorPosition(xPositionIn[applesEaten + 1], yPositionIn[applesEaten + 1]);
            Console.WriteLine(" ");
            
            // Record location of each body part
            for (int i = applesEaten + 1; i > 0; i--)
            {
                xPositionIn[i] = xPositionIn[i - 1];
                yPositionIn[i] = yPositionIn[i - 1];                
            }

            xPositionOut = xPositionIn;
            yPositionOut = yPositionIn;
        }


        private static bool DidSnakeHitWall(int xPosition, int yPosition)
        {
            if (xPosition == 1 || xPosition == 60 || yPosition == 1 || yPosition == 30)
            {
                return true;
            }
            return false;
        }

        private static void BuildWall()
        {
            for (int i = 1; i < 31; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(1, i);
                Console.Write("#");
                Console.SetCursorPosition(60, i);
                Console.Write("#");
            }
            
            for (int i = 1; i < 61; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(i, 1);
                Console.Write("#");
                Console.SetCursorPosition(i, 30);
                Console.Write("#");
            }
        }
        
        private static bool determineIfAppleWasEaten(int xPosition, int yPosition, int appleXDim, int appleYDim)
        {
            if (xPosition == appleXDim && yPosition == appleYDim)
            {
                return true;
            }
            return false;
        }

        private static void paintApple(int appleXDim, int appleYDim)
        {
            Console.SetCursorPosition(appleXDim, appleYDim);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("o");
        }

        private static void setApplePositionOnScreen(Random random, out int appleXDim, out int appleYDim)
        {
            appleXDim = random.Next(0 + 2, 60 - 2);
            appleYDim = random.Next(0 + 2, 30 - 2);
        }
    }
}