using System.Collections.Generic;

namespace RobotControl
{
    public class Invoker
    {
        private readonly Dictionary<string, Queue<ICommand>> _robotCommands = new Dictionary<string, Queue<ICommand>>();

        public void AddCommands(string robotId, List<ICommand> commands) 
        {
            if (_robotCommands.ContainsKey(robotId))
            {
                commands.ForEach(x => _robotCommands[robotId].Enqueue(x));
            }
            else
            {
                _robotCommands.Add(robotId, new Queue<ICommand>(commands));
            }
        }

        public void AddCommand(string robotId, ICommand command)
        {
            if (_robotCommands.ContainsKey(robotId))
            {
                _robotCommands[robotId].Enqueue(command);
            }
            else
            {
                var commands = new Queue<ICommand>();
                commands.Enqueue(command);

                _robotCommands.Add(robotId, commands);
            }
        }

        public void Invoke()
        {
            foreach (var robotCommand in _robotCommands)
            {
                foreach (var command in robotCommand.Value)
                {
                    command.Execute();
                    _robotCommands[robotCommand.Key].Peek();
                }
            }
        }

    }
}
