#load "Domain.fs"
#load "Operations.fs"
open Capstone3.Domain
open Capstone3.Operations
open System

let openingAccount =
        { Owner = { Name = "Isaac" }; Balance = 0M; AccountId = Guid.Empty }

let isValidCommand command =
    let validCommands = ['d'; 'w'; 'x']
    validCommands |> Seq.contains command
// isValidCommand 'a'
// isValidCommand 'd'

let isStopCommand command =
    'x' = command
// isStopCommand 'x'

let getAmount (command:char) =
    if command = 'd' then 'd', 50M
    elif command = 'w' then 'w', 25M
    else command, 0M

let processCommand (account:Account) (command:char, amount:decimal) =
    match command with
    | 'd' -> deposit amount account 
    | 'w' -> withdraw amount account
    | _ -> account
// let acc1 = processCommand openingAccount ('d', 100M)
// let acc2 = processCommand acc1 ('d', 100M)
// let acc3 = processCommand acc2 ('w', 50M)

let account =
    let commands = [ 'd'; 'w'; 'z'; 'f'; 'd'; 'x'; 'w' ]

    commands
    |> Seq.filter isValidCommand
    |> Seq.takeWhile (not << isStopCommand)
    |> Seq.map getAmount
    |> Seq.fold processCommand openingAccount
