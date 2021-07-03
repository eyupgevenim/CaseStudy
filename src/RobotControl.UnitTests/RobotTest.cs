using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RobotControl.UnitTests
{
    [TestClass]
    public class RobotTest
    {
        [TestMethod]
        public void Test_Robot()
        {
            //Input:
            //5 5
            //1 2 N LMLMLMLMM
            //3 3 E MMRMMRMRRM
            //Expected
            //Output: 
            //1 3 N
            //5 1 E

            int maxX = 5;
            int maxY = 5;
            var invoker = new Invoker();

            var robot1 = new Robot(maxX, maxY, 1, 2, Direction.N);
            invoker.AddCommands(robot1.RobotId, new List<ICommand>
            {
                new MoveCommand(robot1, Movement.L),
                new MoveCommand(robot1, Movement.M),
                new MoveCommand(robot1, Movement.L),
                new MoveCommand(robot1, Movement.M),
                new MoveCommand(robot1, Movement.L),
                new MoveCommand(robot1, Movement.M),
                new MoveCommand(robot1, Movement.L),
                new MoveCommand(robot1, Movement.M),
                new MoveCommand(robot1, Movement.M)
            });

            var robot2 = new Robot(maxX, maxY, 3, 3, Direction.E);
            invoker.AddCommands(robot2.RobotId, new List<ICommand>
            {
                new MoveCommand(robot2, Movement.M),
                new MoveCommand(robot2, Movement.M),
                new MoveCommand(robot2, Movement.R),
                new MoveCommand(robot2, Movement.M),
                new MoveCommand(robot2, Movement.M),
                new MoveCommand(robot2, Movement.R),
                new MoveCommand(robot2, Movement.M),
                new MoveCommand(robot2, Movement.R),
                new MoveCommand(robot2, Movement.R),
                new MoveCommand(robot2, Movement.M)
            });

            invoker.Invoke();

            Assert.IsTrue(robot1.X == 1);
            Assert.IsTrue(robot1.Y == 3);
            Assert.IsTrue(robot1.Direction == Direction.N);

            Assert.IsTrue(robot2.X == 5);
            Assert.IsTrue(robot2.Y == 1);
            Assert.IsTrue(robot2.Direction == Direction.E);

        }

        [TestMethod]
        public void Test_Robot_Input_Output()
        {
            //Input:
            //5 5
            //1 2 N LMLMLMLM
            //3 3 E MMRMMRMRRM
            //Expected
            //Output: 
            //1 3 N
            //5 1 E

            string inputLine1 = "5 5";
            string robot1InitCoordinates = "1 2 N";//Robot 1 initialize co-ordinates
            string robot1Commands = "LMLMLMLMM";//Robot 1 command array
            string robot2InitCoordinates = "3 3 E";//Robot 2 initialize co-ordinates
            string robot2Commands = "MMRMMRMRRM"; //Robot 2 command array

            int maxX = int.Parse(inputLine1.Split(new char[] { ' ' }).FirstOrDefault());
            int maxY = int.Parse(inputLine1.Split(new char[] { ' ' }).LastOrDefault());

            var invoker = new Invoker();

            var robot1 = GetRobot(maxX, maxY, robot1InitCoordinates);
            var robot1MoveCommands = GetMoveCommands(robot1, robot1Commands);
            invoker.AddCommands(robot1.RobotId, robot1MoveCommands);

            var robot2 = GetRobot(maxX, maxY, robot2InitCoordinates);
            var robot2MoveCommands = GetMoveCommands(robot2, robot2Commands);
            invoker.AddCommands(robot2.RobotId, robot2MoveCommands);

            invoker.Invoke();

            Assert.IsTrue($"{robot1.X} {robot1.Y} {robot1.Direction}" == "1 3 N");
            Assert.IsTrue($"{robot2.X} {robot2.Y} {robot2.Direction}" == "5 1 E");
        }

        private Robot GetRobot(int maxX, int maxY, string initializeCoordinates)
        {
            Match match = Regex.Match(initializeCoordinates, "(\\d) (\\d) ([N|E|S|W]{1})$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var firstX = match.Groups[1].Value;
                var firstY = match.Groups[2].Value;
                var firstDirection = match.Groups[3].Value;

                Enum.TryParse(firstDirection, out Direction direction);
                return new Robot(maxX, maxY, int.Parse(firstX), int.Parse(firstY), direction);
            }

            throw new ArgumentException($"invalid initialize co-ordinates inputs : {initializeCoordinates} - must be regex pattern:(\\d) (\\d) ([N|E|S|W]{{1}})$");
        }

        private List<ICommand> GetMoveCommands(Robot robot, string commands)
        {
            if (Regex.Match(commands, "[R|L|M]{1,}$", RegexOptions.IgnoreCase).Success)
            {
                var moveCommands = new List<ICommand>();
                foreach (var movementString in commands)
                {
                    Enum.TryParse($"{movementString}", out Movement movement);
                    moveCommands.Add(new MoveCommand(robot, movement));
                }

                return moveCommands;
            }

            throw new ArgumentException($"invalid commands inputs : {commands} - must be regex pattern:[R|L|M]{{1,}}$");
        }

    }
}