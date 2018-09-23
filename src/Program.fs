namespace ToyRobot

module App = 
    open System

    let private showHelp() = 
        printfn " "
        printfn "--------------------------"
        printfn "Valid commands (case insensitive):"
        printfn "   place x,y,f (where x,y are integers and f is one of north, south, east or west.)"
        printfn "   move (moves the robot one space forward)"
        printfn "   left (turns the robot to the left)"
        printfn "   right (turns the robot to the right)"
        printfn "   report (shows the robots current coords and direction)"
        printfn "   quit"
        printfn "--------------------------"
        printfn " "

    let private handleInputFailure err = 
        printfn "Error: %s" err
        showHelp()
        printfn "Enter a command:"

    let report robot = RobotApi.report robot.Position

    let private handleReport robot = 
        printfn "%s" (report robot)

    let private handleCommand cmd robot = 
        let commandResult = RobotApi.handleCommand cmd robot
        match commandResult with 
        | CommandResult.Success newRobot -> 
            printfn "Success: %s" (report newRobot)
            newRobot //Return updated robot
        | CommandResult.Failure cmdFailure -> 
            printfn "Failed: %s" cmdFailure.Error
            robot //Return original, unchanged robot

    let private handleExit() = 
        printfn "Exiting..."    

    let rec captureInput robot =
        Console.ReadLine()
        |> handleInput captureInput robot 

    and handleInput (getNextInput : Robot -> unit) robot input =
        match InputParser.parse input with
        | Exit ->
            handleExit()        
        | Failure err ->
            handleInputFailure err
            getNextInput robot
        | Report ->
            handleReport robot
            getNextInput robot
        | Command cmd ->
            handleCommand cmd robot 
            |> getNextInput 

    [<EntryPoint>]
    let main argv =
        printfn "Welcome to the toy robot."
        printfn "The robot is on a 5 x 5 square grid (zero-based coords)."
        printfn "Coords: x is east-west, y is north-south."

        showHelp()

        let robot = RobotApi.init()

        printfn "Robot starting at: %s" (report robot)
        printfn "Enter a command:"

        captureInput robot

        0 // return an integer exit code
