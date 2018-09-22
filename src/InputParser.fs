namespace ToyRobot

type InputResult = 
    | Command of RobotCommand
    | Report 
    | Failure of string
    | Exit

module InputParser = 
    open System

    let private parseInt (intString : string) : Result<int, string> = 
        match Int32.TryParse intString with
        | true, i  -> Ok i
        | false, _ -> Error "Invalid coord (an integer is required)" 

    let private parseDirection (directionString : string) : Result<Direction, string> = 
        match directionString.Trim().ToUpper() with
        | "NORTH" -> Ok Direction.North
        | "SOUTH" -> Ok Direction.South
        | "WEST"  -> Ok Direction.West
        | "EAST"  -> Ok Direction.East
        | _       -> Error "Invalid placement direction"

    let private parsePlace (input : string) = 
        let placeArgs = input.Replace("PLACE", "").Split(",")
        if placeArgs.Length = 3 then
            let xResult = parseInt placeArgs.[0] 
            let yResult = parseInt placeArgs.[1] 
            let fResult = parseDirection placeArgs.[2]

            match (xResult, yResult, fResult) with
            | (Ok x, Ok y, Ok f) 
                -> Command (Place { X = x; Y = y; Direction = f })
            | (Error err, _, _) | (_, Error err, _) | (_, _, Error err) 
                -> Failure err
        else
            Failure "Invalid command"

    let parse (input : string) : InputResult = 
        let inputText = input.Trim().ToUpper()

        match inputText.StartsWith("PLACE") with
        | true ->
            parsePlace inputText
        | false ->
            match inputText with
            | "MOVE"   -> Command Move
            | "LEFT"   -> Command Left
            | "RIGHT"  -> Command Right
            | "REPORT" -> Report
            | "QUIT"   -> Exit
            | _        -> Failure "Invalid command"
            