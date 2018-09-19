namespace ToyRobot.Tests

open System
open Xunit
open FsUnit
open ToyRobot
open App

module InputTests = 

    [<Fact>]
    let ``Input = report should parse as a Report`` () =
        InputParser.parse "report"
        |> should equal (InputResult.Report)

    [<Fact>]
    let ``Report parsing should be case insensitive and ignore spaces`` () =
        InputParser.parse " rePorT "
        |> should equal (InputResult.Report)

    [<Fact>]
    let ``Input = quit should parse as a Exit`` () =
        InputParser.parse "quit"
        |> should equal (InputResult.Exit)

    [<Fact>]
    let ``Quit parsing should be case insensitive and ignore spaces`` () =
        InputParser.parse " qUIt "
        |> should equal (InputResult.Exit)

    [<Fact>]
    let ``Input = move should parse as a Command`` () =
        InputParser.parse "move"
        |> should equal (InputResult.Command(Move))

    [<Fact>]
    let ``Move parsing should be case insensitive and ignore spaces`` () =
        InputParser.parse " MovE "
        |> should equal (InputResult.Command(Move))

    [<Fact>]
    let ``Input = left should parse as a Command`` () =
        InputParser.parse "left"
        |> should equal (InputResult.Command(Left))

    [<Fact>]
    let ``Left parsing should be case insensitive and ignore spaces`` () =
        InputParser.parse " lEFt "
        |> should equal (InputResult.Command(Left))

    [<Fact>]
    let ``Input = right should parse as a Command`` () =
        InputParser.parse "right"
        |> should equal (InputResult.Command(Right))

    [<Fact>]
    let ``Right parsing should be case insensitive and ignore spaces`` () =
        InputParser.parse " riGht "
        |> should equal (InputResult.Command(Right))

    [<Fact>]
    let ``Input = place 2, 3, west should parse as a Command`` () =
        InputParser.parse "place 2,3,west"
        |> should equal (InputResult.Command(Place(2,3,West)))

    [<Fact>]
    let ``Place parsing should be case insensitive and ignore spaces`` () =
        InputParser.parse "    plAce 2,  3     ,   WEst    "
        |> should equal (InputResult.Command(Place(2,3,West)))

    [<Fact>]
    let ``Invalid commands should not parse`` () =
        InputParser.parse "  zzzzz xxxx   "
        |> should equal (InputResult.Failure("Invalid command"))

    [<Fact>]
    let ``Invalid place x coords should not parse`` () =
        InputParser.parse "place x,2,north"
        |> should equal (InputResult.Failure("Invalid coord (an integer is required)"))

    [<Fact>]
    let ``Invalid place y coords should not parse`` () =
        InputParser.parse "place 2,x,north"
        |> should equal (InputResult.Failure("Invalid coord (an integer is required)"))

    [<Fact>]
    let ``Invalid place direction should not parse`` () =
        InputParser.parse "place 2,2,southwest"
        |> should equal (InputResult.Failure("Invalid placement direction"))

    [<Fact>]
    let ``Invalid place commands should not parse`` () =
        InputParser.parse "place 2,2,"
        |> should equal (InputResult.Failure("Invalid placement direction"))

    [<Fact>]
    let ``Invalid place commands (missing comma) should not parse`` () =
        InputParser.parse "place 2,2 north"
        |> should equal (InputResult.Failure("Invalid command"))