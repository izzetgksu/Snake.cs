using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WindowHeight = 16;
        Console.WindowWidth = 32;
        Console.BufferHeight = 16;
        Console.BufferWidth = 32;

        var snake = new List<Position>
        {
            new Position(10, 10),
            new Position(9, 10),
            new Position(8, 10)
        };

        var food = GenerateFood();

        var direction = Direction.Right;
        var score = 0;

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true).Key;
                direction = GetNewDirection(direction, key);
            }

            var head = snake.First();
            var newHead = GetNewHeadPosition(head, direction);

            if (newHead.Equals(food))
            {
                score++;
                food = GenerateFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }

            if (snake.Contains(newHead) || IsOutOfBounds(newHead))
            {
                Console.Clear();
                Console.WriteLine($"Game Over! Your score: {score}");
                break;
            }

            snake.Insert(0, newHead);

            Console.Clear();
            DrawSnake(snake);
            DrawFood(food);
            Thread.Sleep(100);
        }
    }

    static Position GenerateFood()
    {
        var random = new Random();
        return new Position(random.Next(0, Console.WindowWidth), random.Next(0, Console.WindowHeight));
    }

    static Direction GetNewDirection(Direction currentDirection, ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow:
                return currentDirection != Direction.Down ? Direction.Up : currentDirection;
            case ConsoleKey.DownArrow:
                return currentDirection != Direction.Up ? Direction.Down : currentDirection;
            case ConsoleKey.LeftArrow:
                return currentDirection != Direction.Right ? Direction.Left : currentDirection;
            case ConsoleKey.RightArrow:
                return currentDirection != Direction.Left ? Direction.Right : currentDirection;
            default:
                return currentDirection;
        }
    }

    static Position GetNewHeadPosition(Position head, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Position(head.X, head.Y - 1);
            case Direction.Down:
                return new Position(head.X, head.Y + 1);
            case Direction.Left:
                return new Position(head.X - 1, head.Y);
            case Direction.Right:
                return new Position(head.X + 1, head.Y);
            default:
                return head;
        }
    }

    static bool IsOutOfBounds(Position position)
    {
        return position.X < 0 || position.X >= Console.WindowWidth ||
               position.Y < 0 || position.Y >= Console.WindowHeight;
    }

    static void DrawSnake(List<Position> snake)
    {
        foreach (var segment in snake)
        {
            Console.SetCursorPosition(segment.X, segment.Y);
            Console.Write("■");
        }
    }

    static void DrawFood(Position food)
    {
        Console.SetCursorPosition(food.X, food.Y);
        Console.Write("★");
    }
}

enum Direction
{
    Up,
    Down,
    Left,
    Right
}

struct Position
{
    public int X { get; }
    public int Y { get; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object obj)
    {
        if (obj is Position other)
        {
            return X == other.X && Y == other.Y;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
