using System;

namespace RobotControl
{
    public class Robot
    {
        private readonly int MaxX;
        private readonly int MaxY;
        public readonly string RobotId;
        public int X { get; private set; }
        public int Y { get; private set; }
        public Direction Direction { get; private set; }
        public Robot(int maxX, int maxY, int x, int y, Direction direction)
        {
            RobotId = Guid.NewGuid().ToString();
            MaxX = maxX;
            MaxY = maxY;
            X = x;
            Y = y;
            Direction = direction;
        }

        public void Execute(Movement movement)
        {
            if (!IsValid(movement))
            {
                //TODO:..
                return;
            }

            //0-1-2-3
            //N-E-S-W
            //rigth -> +1
            //left  -> -1
            switch (movement)
            {
                case Movement.R:
                    {
                        Direction = (Direction)(((int)this.Direction + 1) % 4);
                    }
                    break;
                case Movement.L:
                    {
                        Direction = (Direction)(((int)this.Direction - 1 + 4) % 4);
                    }
                    break;
                case Movement.M:
                    {
                        Move();
                    }
                    break;
                default:
                    throw new InvalidOperationException($"undefined {nameof(movement)}");
            }
        }

        private void Move()
        {
            switch (Direction)
            {
                case Direction.N:
                    {
                        Y++;
                    }
                    break;
                case Direction.S:
                    {
                        Y--;
                    }
                    break;
                case Direction.W:
                    {
                        X--;
                    }
                    break;
                case Direction.E:
                    {
                        X++;
                    }
                    break;
                default:
                    throw new InvalidOperationException($"undefined {nameof(Direction)}");
            }
        }

        private bool IsValid(Movement movement)
        {
            if (movement != Movement.M)
            {
                return true;
            }

            switch (Direction)
            {
                case Direction.N: return MaxY > Y;
                case Direction.S: return Y > 0;
                case Direction.E: return MaxX > X;
                case Direction.W: return X > 0;
                default: return false;
            }
        }
    }
}
