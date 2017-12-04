module Capstone4.Program

open Capstone4.Domain

open System
open Operations
open Auditing
open FileRepository

type Command = BankCommand of BankOperation | Exit

let tryGetAmount command =
    Console.Write "\nEnter Amount: "
    let amount = Console.ReadLine() |> Decimal.TryParse
    match amount with
    | true, amount -> Some(command, amount)
    | false, _ -> None

let depositWithAudit = deposit |> auditAs "deposit" fileSystemAudit
let withdrawWithAudit = withdraw |> auditAs "withdraw" fileSystemAudit
let getAccount = findTransactionsOnDisk >> loadAccount

let processCommand account (command, amount) =
    match command with
    | Deposit -> account |> depositWithAudit amount 
    | Withdraw -> account |> withdrawWithAudit amount 

let tryParseCommand cmd =
    match cmd with
    | 'd' -> Some (BankCommand Deposit)
    | 'w' -> Some (BankCommand Withdraw)
    | 'x' -> Some Exit
    | _ -> None

let tryGetBankOperation cmd =
    match cmd with
    | BankCommand op -> Some op
    | Exit -> None


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
        |> Seq.choose tryGetBankOperation
        |> Seq.choose tryGetAmount
        |> Seq.fold processCommand openingAccount

    printfn "\n\nClosing account: \n\n%A" closingAccount

    0 // return an integer exit code
