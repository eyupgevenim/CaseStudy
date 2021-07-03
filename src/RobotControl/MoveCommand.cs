namespace RobotControl
{
    public class MoveCommand : ICommand
    {
        private readonly Robot robot;
        private readonly Movement movement;

        public MoveCommand(Robot robot, Movement movement)
        {
            this.robot = robot;
            this.movement = movement;
        }

        public void Execute()
        {
            robot.Execute(movement);
        }
    }
}
