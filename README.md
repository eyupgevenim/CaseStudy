Robot Control
=============

Unit test
---------
```csharp

[TestMethod]
public void Test_Robot()
{
	//Input:
	//5 5
	//1 2 N 
	//LMLMLMLMM
	//3 3 E 
	//MMRMMRMRRM
	//Expected Output: 
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

```

Testing with console application
---------

Can be test the inputs from the *.txt extension file




