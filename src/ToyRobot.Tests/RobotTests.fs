namespace ToyRobot.Tests

open System
open Xunit
open FsUnit
open ToyRobot

module RobotTests = 
    let createRobot() = RobotApi.init()
    let report robot = RobotApi.report robot
    let move robot = RobotApi.handleCommand Move robot
    let right robot = RobotApi.handleCommand Right robot
    let left robot = RobotApi.handleCommand Left robot
    let place x y direction robot = RobotApi.handleCommand (Place(x, y, direction)) robot

    [<Fact>]
    let ``Initial robot should be 0,0 facing North`` () =
        createRobot()
        |> report 
        |> should equal "(0,0) facing North"

    [<Fact>]
    let ``Placing robot within grid should work`` () =
        createRobot()
        |> place 2 3 Direction.West
        |> report 
        |> should equal "(2,3) facing West"

    [<Fact>]
    let ``Placing robot outside of grid should be ignored`` () =
        createRobot()
        |> place 2 3 Direction.West
        |> place 8 0 Direction.East
        |> place 8 0 Direction.South
        |> report 
        |> should equal "(2,3) facing West"

    [<Fact>]
    let ``Moving robot 4 times should give 0,4 facing North`` () =
        createRobot()
        |> move
        |> move
        |> move
        |> move
        |> report 
        |> should equal "(0,4) facing North"

    [<Fact>]
    let ``Moving robot 6 times should give 0,4 facing North (invalid moves should be ignored)`` () =
        createRobot()
        |> move
        |> move
        |> move
        |> move
        |> move
        |> move
        |> report 
        |> should equal "(0,4) facing North"

    [<Fact>]
    let ``Moving, then turning right and moving robot 3 times should give 3,1 facing East`` () =
        createRobot()
        |> move
        |> right 
        |> move
        |> move
        |> move
        |> report 
        |> should equal "(3,1) facing East"

    [<Fact>]
    let ``Moving, then turning right and moving robot 6 times should give 4,1 facing East (invalid moves should be ignored)`` () =
        createRobot()
        |> move
        |> right
        |> move
        |> move
        |> move
        |> move
        |> move
        |> move
        |> report 
        |> should equal "(4,1) facing East"

    [<Fact>]
    let ``Turning left 1 time should give 0,0 facing West`` () =
        createRobot()
        |> left 
        |> report 
        |> should equal "(0,0) facing West"

    [<Fact>]
    let ``Turning left 2 times should give 0,0 facing South`` () =
        createRobot()
        |> left
        |> left 
        |> report 
        |> should equal "(0,0) facing South"

    [<Fact>]
    let ``Turning left 3 times should give 0,0 facing East`` () =
        createRobot()
        |> left
        |> left
        |> left 
        |> report 
        |> should equal "(0,0) facing East"

    [<Fact>]
    let ``Turning left 4 times should give 0,0 facing North`` () =
        createRobot()
        |> left
        |> left 
        |> left
        |> left 
        |> report 
        |> should equal "(0,0) facing North"

    [<Fact>]
    let ``Turning right 1 time should give 0,0 facing East`` () =
        createRobot()
        |> right 
        |> report 
        |> should equal "(0,0) facing East"

    [<Fact>]
    let ``Turning right 2 times should give 0,0 facing South`` () =
        createRobot()
        |> right 
        |> right 
        |> report 
        |> should equal "(0,0) facing South"

    [<Fact>]
    let ``Turning right 3 times should give 0,0 facing West`` () =
        createRobot()
        |> right 
        |> right 
        |> right 
        |> report 
        |> should equal "(0,0) facing West"

    [<Fact>]
    let ``Turning right 4 times should give 0,0 facing North`` () =
        createRobot()
        |> right 
        |> right 
        |> right 
        |> right 
        |> report 
        |> should equal "(0,0) facing North"

    [<Fact>]
    let ``Turning right then left should give 0,0 facing North`` () =
        createRobot()
        |> right 
        |> left 
        |> report 
        |> should equal "(0,0) facing North"