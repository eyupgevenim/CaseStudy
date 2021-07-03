using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RobotControl.ConsoleApp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            InitializeTest();

            Console.WriteLine("\n Press any key to turn it off");
            Console.ReadKey();
        }

        static void InitializeTest()
        {
            Console.Write("Select *.txt file from your computer (y/N):");
            var yesOrNo = Console.ReadKey();
            Console.WriteLine($"");

            if (string.Equals($"{yesOrNo.KeyChar}", "y", StringComparison.OrdinalIgnoreCase))
            {
                InputTest();
            }
        }

        static void InputTest()
        {
            try
            {
                var lines = GetInputLines().Select((Value, Index) => new { Value, Index });
                if (lines.Any())
                {
                    Console.WriteLine($"");
                    Console.WriteLine($"INPUTS:");
                    Console.WriteLine($"==========================");
                    Console.WriteLine($"{string.Join("\n", lines.Select(x => x.Value))}");
                    Console.WriteLine($"==========================");


                    string inputLine1 = lines.FirstOrDefault().Value.Trim();
                    if (!Regex.Match(inputLine1, @"(\d) (\d)$").Success)
                    {
                        throw new ArgumentException($"invalid input line 1 : {inputLine1} - must be regex pattern:(\\d) (\\d)$");
                    }

                    int maxX = int.Parse(inputLine1.Split(new char[] { ' ' }).FirstOrDefault());
                    int maxY = int.Parse(inputLine1.Split(new char[] { ' ' }).LastOrDefault());

                    var invoker = new Invoker();
                    var robots = new List<Robot>();
                    foreach (var line in lines.Where(x=> x.Index % 2 == 1))
                    {
                        var robot = GetRobot(maxX, maxY, line.Value.Trim());
                        var moveCommandsString = lines.FirstOrDefault(x => x.Index == (line.Index + 1)).Value;
                        var moveCommands = GetMoveCommands(robot, moveCommandsString);
                        invoker.AddCommands(robot.RobotId, moveCommands);
                        robots.Add(robot);
                    }

                    invoker.Invoke();

                    Console.WriteLine($"");
                    Console.WriteLine($"OUTPUTS:");
                    Console.WriteLine($"==========================");
                    foreach (var robot in robots)
                    {
                        Console.WriteLine($"{robot.X} {robot.Y} {robot.Direction}");
                    }
                    Console.WriteLine($"==========================");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:{ex.Message}");
            }

            Console.WriteLine($"");
            InitializeTest();
        }

        static string[] GetInputLines()
        {
            var applicationPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", "");
            using (OpenFileDialog fileSelectPopUp = new OpenFileDialog())
            {
                fileSelectPopUp.Title = "";
                fileSelectPopUp.InitialDirectory = applicationPath;
                fileSelectPopUp.Filter = "Text Document|*.txt";
                fileSelectPopUp.FilterIndex = 2;
                fileSelectPopUp.RestoreDirectory = true;
                if (fileSelectPopUp.ShowDialog() == DialogResult.OK)
                {
                    return System.IO.File.ReadAllLines(fileSelectPopUp.FileName);
                }
                else
                {
                    Console.WriteLine("File not selected");
                } 
            }

            return new string[0];
        }

        static Robot GetRobot(int maxX, int maxY, string initializeCoordinates)
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

        static List<ICommand> GetMoveCommands(Robot robot, string commands)
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
