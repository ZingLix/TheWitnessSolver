# TheWitnessSolver

> [Chinese Version](https://zinglix.xyz/2017/07/21/The-Witness-Puzzle-Solver/)

This is a program designed to solve the puzzle in the game The Witness.

The Witness is a puzzle game which main form is  one touch drawing just like the pictures below.

!([https://zinglix.xyz/img/in-post/WitnessSolver/1.jpg](https://zinglix.xyz/img/in-post/WitnessSolver/1.jpg))

There will be lots of different shapes in the  

Since this is my first C#/WPF program, the code is sure to be terrible. I will still work on improving it.


## Rule

| Shape     | Position  |  Rule |
| ----------------- |:-------:|:-------------:|
| Hexagon      | Dot,Side | Path has to cover each hexagon. |
| Octangle      |   Grid    | Octangle must be made in pair. |
| Square | Grid    |  Path has to separate the square of different color. |
| Triangle |     Grid    | The times the path passes must be same as the number of triangle.|
| Tetris|      Grid     | The region separated by the path should be same as the shape of the combination of Tetris.|
| Three-tipped Icon |     Side, Grid    | Eliminate a condition which doesn't match.|

## To-Do List
* UI Re-design
* Refactor the main code

## Copyright
LGPL Licenses.