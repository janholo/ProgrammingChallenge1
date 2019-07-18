# ProgrammingChallenge1

## Purpose

Welcome to the 1. Programming Challenge.
The task in this challenge is to implement a `controller` which steers a paddle in the game `pong`.
A simple controller can be written in only a few lines of code.
The game is quite simple and has no UI other than the console output.
This is by design to keep everything simple and easy to understand.

## Game Mechanics

The full state of the game including width and height of the playing field
is passed as an argument to the `Update` method of your controller.
Based on the information given there you have to decide if you want to move upwards/downwards or stay where you are.
All of the fields of the `GameState` are documented and should be self explanatory.
(The GameState even provides functionality to simulate your own game inside the `Update` method)

Following are the rules of the game:

* The lower left corner of the playing field is coordinate (0,0)
* Positive x-axis goes to the right
* Positive y-axis goes to the top
* The Ball starts in the center of the field and moves towards the right paddle
* The ball bounces of the top and bottom wall in a 90° angle
* If the ball hits the left wall the right player wins
* If the ball hits the right wall the left player wins
* If the ball bounces of a paddle the ball accelerates
* The direction of the ball after it bounces of the paddle is dependent to the relative postion between paddle and ball
  * If the ball hits the center of the paddle the balls direction is horizontal
  * If the ball hits the paddle at the top the balls direction is upwards
  * If the ball hits the paddle at the bottom the balls direction is downwards

## How to contribute

1. In the folder `Controller` create your own folder.
1. In this folder create one (or more) classes which implement the `IController` interface.
1. A starting point for the implementation can be seen in the file `Jan.Reinhardt/NaiveController.cs`
1. You can do nearly anything you want in your implementation.
   Just dont try to hack the game to your advantage via reflection or some other stuff ;-)
1. To test your controller set your controller as the left or
   right controller (or both) in `Program.cs` and start the app via `dotnet run` or `F5`.
1. To submit your solution:
   1. Send your github id to me so i can add you as a contributor
   1. Push your changes to the master branch of the repo

> All participants work with the same repository, so please only commit changes in your own folder. Keep your changes to the `Program.cs` only locally. Otherwise chaos rises!

```csharp
var leftController = new Controller.<YourName>.<YourController>();
```

## End of the competition

The competition ends at 16.08.2019

After the competition ends all of the submitted controllers fight against each other in an elimination cup.
