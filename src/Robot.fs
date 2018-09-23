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

type Robot = {
    //Robot may end up having more properties...
    Position : RobotPosition
}

type RobotCommand = 
    | Place of RobotPosition
    | Move 
    | Left
    | Right

type FailedCommand = {
    Robot : Robot
    Error : string
}

type CommandResult = 
    | Success of Robot
    | Failure of FailedCommand

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
        { Position = { X = 0; Y = 0; Direction = Direction.North } }

    let report position = 
        sprintf "(%i,%i) facing %s" position.X position.Y (position.Direction.ToString()) 

    let private coordsAreValid x y = 
        x >= 0 && x <= grid.MaxX && y >=0 && y <= grid.MaxY

    let private movePosition pos =
        match pos.Direction with 
        | Direction.North when pos.Y < grid.MaxY -> Ok { pos with Y = pos.Y + 1 }
        | Direction.South when pos.Y > 0         -> Ok { pos with Y = pos.Y - 1 }
        | Direction.East  when pos.X < grid.MaxX -> Ok { pos with X = pos.X + 1 }
        | Direction.West  when pos.X > 0         -> Ok { pos with X = pos.X - 1 }
        | _ -> Error "Unable to move - currently at edge of grid" // Ignore move commands that move robot outside of valid coords

    let private moveRobot (robot : Robot) = 
        match movePosition robot.Position with 
        | Ok pos    -> Success { robot with Position = pos }
        | Error msg -> Failure { Robot = robot; Error = msg }

    let private turn getNewDirection (robot : Robot) = 
        let newDirection = getNewDirection robot.Position.Direction
        let newPosition = { robot.Position with Direction = newDirection }
        Success { robot with Position = newPosition }

    let private placeRobot (position : RobotPosition) robot = 
        if coordsAreValid position.X position.Y then
            Success { robot with Position = position }
        else
            let msg = sprintf "Ignored place command as coords were invalid. Robot still at: %s" (report robot.Position)
            Failure { Robot = robot; Error = msg }

    let handleCommand (cmd : RobotCommand) (robot : Robot) : CommandResult = 
        match cmd with
        | Move      -> moveRobot robot 
        | Left      -> turn Compass.turnLeft robot
        | Right     -> turn Compass.turnRight robot
        | Place pos -> placeRobot pos robot
