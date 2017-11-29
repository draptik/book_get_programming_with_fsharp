﻿// Learn more about F# at http://fsharp.org

open System
open Capstone3.Domain
open Capstone3.Operations

let isValidCommand command =
    let validCommands = ['d'; 'w'; 'x']
    validCommands |> Seq.contains command

let isStopCommand command =
    'x' = command

let getAmount (command:char) =
    if command = 'd' then 'd', 50M
    elif command = 'w' then 'w', 25M
    else command, 0M

let processCommand (account:Account) (command:char, amount:decimal) =
    match command with
    | 'd' -> deposit amount account 
    | 'w' -> withdraw amount account
    | _ -> account

let openingAccount =
        { Owner = { Name = "Isaac" }; Balance = 0M; AccountId = Guid.Empty }

[<EntryPoint>]
let main argv =

    let consoleCommands = seq {
        while true do
            Console.Write "\n(d)eposit, (w)ithdraw or e(x)it: "
            yield Console.ReadKey().KeyChar
    }

    let closingAccount =
        consoleCommands
        |> Seq.filter isValidCommand
        |> Seq.takeWhile (not << isStopCommand)
        |> Seq.map getAmount
        |> Seq.fold processCommand openingAccount

    printfn "\n\nClosing account: \n\n%A" closingAccount

    0 // return an integer exit code
