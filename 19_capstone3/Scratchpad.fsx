#load "Domain.fs"
open Capstone3.Domain
open System

let openingAccount =
        { Owner = { Name = "Isaac" }; Balance = 0M; AccountId = Guid.Empty }

let isValidCommand command =
    let validCommands = ['d'; 'w'; 'x']
    validCommands |> Seq.contains command


let account =
    let commands = [ 'd'; 'w'; 'z'; 'f'; 'd'; 'x'; 'w' ]

    commands
    |> Seq.filter isValidCommand
    |> Seq.takeWhile (not << isStopCommand)
    |> Seq.map getAmount
    |> Seq.fold processCommand openingAccount
