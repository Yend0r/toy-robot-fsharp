namespace ToyRobot

type Grid = {
    MaxX : int
    MaxY : int
}

type Direction = 
    | North = 0 
    | East = 1
    | South = 2
    | West = 3

type RobotPosition = {
    X : int
    Y : int
    Direction : Direction
}

type RobotCommand = 
    | Place of RobotPosition
    | Move 
    | Left
    | Right

type Robot = {
    X : int
    Y : int
    Direction : Direction
    LastAction : string
}

module Compass = 
    let private directions = [| Direction.North; Direction.East; Direction.South; Direction.West |] 

    let turnLeft (currentDirection : Direction) = 
        let pos = int currentDirection
        directions.[(pos - 1 + directions.Length) % directions.Length]

    let turnRight (currentDirection : Direction) = 
        let pos = int currentDirection
        directions.[(pos + 1) % directions.Length]


[<RequireQualifiedAccess>]
module RobotApi = 

    // This is hard-coded as per instructions but could easily be specified by user
    let grid : Grid = 
        { MaxX = 4; MaxY = 4 }

    let init() : Robot = 
        { X = 0; Y = 0; Direction = Direction.North; LastAction = "Robot initialized" }

    let report robot = 
        sprintf "(%i,%i) facing %s" robot.X robot.Y (robot.Direction.ToString()) 

    let private coordsAreValid x y = 
        x >= 0 && x <= grid.MaxX && y >=0 && y <= grid.MaxY

    let moveDescription direction = sprintf "Moved %s" (direction.ToString())

    let turnDescription direction = sprintf "Turned %s" (direction.ToString())

    let private moveRobot robot = 
        match robot.Direction with 
        | Direction.North when robot.Y < grid.MaxY -> { robot with Y = robot.Y + 1; LastAction = moveDescription robot.Direction }
        | Direction.South when robot.Y > 0         -> { robot with Y = robot.Y - 1; LastAction = moveDescription robot.Direction }
        | Direction.East  when robot.X < grid.MaxX -> { robot with X = robot.X + 1; LastAction = moveDescription robot.Direction }
        | Direction.West  when robot.X > 0         -> { robot with X = robot.X - 1; LastAction = moveDescription robot.Direction }
        | _ -> { robot with LastAction = "Unable to move - currently at edge of grid" } // Ignore move commands that move robot outside of valid coords

    let private turnLeft robot = 
        let newDirection = Compass.turnLeft robot.Direction
        { robot with Direction = newDirection; LastAction = turnDescription newDirection }

    let private turnRight robot = 
        let newDirection = Compass.turnRight robot.Direction
        { robot with Direction = newDirection; LastAction = turnDescription newDirection }

    let private placeRobot (position : RobotPosition) robot = 
        if coordsAreValid position.X position.Y then
            let placedRobot = { X = position.X; Y = position.Y; Direction = position.Direction; LastAction = "" }
            { placedRobot with LastAction = sprintf "Placed at %s" (report placedRobot) }
        else
            { robot with LastAction = sprintf "Ignored place command as coords were invalid. Robot still at: %s" (report robot) }

    let handleCommand (cmd : RobotCommand) (robot : Robot) : Robot = 
        match cmd with
        | Move -> moveRobot robot 
        | Left -> turnLeft robot
        | Right -> turnRight robot
        | Place position -> placeRobot position robot
