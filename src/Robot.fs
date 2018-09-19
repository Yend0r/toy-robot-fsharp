namespace ToyRobot

type Grid = {
    X : int
    Y : int
}

type Direction = 
    | North
    | East
    | South
    | West

type RobotCommand = 
    | Place of int * int * Direction
    | Move 
    | Left
    | Right

type Robot = {
    X : int
    Y : int
    Direction: Direction
    Message : string
}

[<RequireQualifiedAccess>]
module RobotApi = 

    // This is hard-coded as per instructions but could easily be specified by user
    let grid : Grid = 
        { X = 4; Y = 4 }

    let init() : Robot = 
        { X = 0; Y = 0; Direction = North; Message = "Robot initialized" }

    let report robot = 
        sprintf "(%i,%i) facing %s" robot.X robot.Y (robot.Direction.ToString()) 

    let private coordsAreValid x y = 
        x >= 0 && x <= grid.X && y >=0 && y <= grid.Y

    let private moveRobot robot = 
        match robot.Direction with 
        | North when robot.Y < grid.Y -> { robot with Y = robot.Y + 1; Message = "Moved north" }
        | South when robot.Y > 0      -> { robot with Y = robot.Y - 1; Message = "Moved south" }
        | East  when robot.X < grid.X -> { robot with X = robot.X + 1; Message = "Moved east" }
        | West  when robot.X > 0      -> { robot with X = robot.X - 1; Message = "Moved west" }
        | _ -> { robot with Message = "Unable to move - currently at edge of grid" } // Ignore move commands that move robot outside of valid coords

    let private turnLeft robot = 
        match robot.Direction with
        | North -> { robot with Direction = West; Message = "Turned west" }
        | East  -> { robot with Direction = North; Message = "Turned north" }
        | West  -> { robot with Direction = South; Message = "Turned south" }
        | South -> { robot with Direction = East; Message = "Turned east" }

    let private turnRight robot = 
        match robot.Direction with
        | North -> { robot with Direction = East; Message = "Turned east" }
        | East  -> { robot with Direction = South; Message = "Turned south" }
        | West  -> { robot with Direction = North; Message = "Turned north" }
        | South -> { robot with Direction = West; Message = "Turned west" }

    let private placeRobot x y direction robot = 
        if coordsAreValid x y then
            let placedRobot = { X = x; Y = y; Direction = direction; Message = "" }
            { placedRobot with Message = sprintf "Placed at %s" (report placedRobot) }
        else
            { robot with Message = sprintf "Ignored place command as coords were invalid. Robot still at: %s" (report robot) }

    let handleCommand (cmd : RobotCommand) (robot : Robot) : Robot = 
        match cmd with
        | Move -> moveRobot robot 
        | Left -> turnLeft robot
        | Right -> turnRight robot
        | Place(x, y, direction) -> placeRobot x y direction robot
