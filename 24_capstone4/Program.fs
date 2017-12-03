module Capstone4.Program

open Capstone4.Domain

open System
open Operations
open Auditing
open FileRepository

let getAmountConsole command =
    Console.Write "\nEnter Amount: "
    command, Console.ReadLine() |> Decimal.Parse

let depositWithAudit = deposit |> auditAs "deposit" fileSystemAudit
let withdrawWithAudit = withdraw |> auditAs "withdraw" fileSystemAudit
let getAccount = findTransactionsOnDisk >> loadAccount

let processCommand account (command, amount) =
    match command with
    | Deposit -> account |> depositWithAudit amount 
    | Withdraw -> account |> withdrawWithAudit amount 
    | _ -> account

let tryParseCommand cmd =
    match cmd with
    | 'd' -> Some Deposit
    | 'w' -> Some Withdraw
    | 'x' -> Some Exit
    | _ -> None



[<EntryPoint>]
let main argv =

    let openingAccount =
        Console.Write "Please enter your name: "
        Console.ReadLine() |> getAccount
    
    let consoleCommands = seq {
        while true do
            Console.Write "\n(d)eposit, (w)ithdraw or e(x)it: "
            yield Console.ReadKey().KeyChar
    }

    let closingAccount =
        consoleCommands
        |> Seq.choose tryParseCommand
        |> Seq.takeWhile ((<>) Exit)
        |> Seq.map getAmountConsole
        |> Seq.fold processCommand openingAccount

    printfn "\n\nClosing account: \n\n%A" closingAccount

    0 // return an integer exit code
