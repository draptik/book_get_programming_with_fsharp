module Capstone4.Operations

open System
open Capstone4.Domain

let rateAccount account =
    if account.Balance < 0M then Overdrawn account
    else Credit(CreditAccount account)    

let deposit amount account =
    let account =
        match account with
        | Credit (CreditAccount account) -> account
        | Overdrawn account -> account
    { account with Balance = account.Balance + amount }
    |> rateAccount

let withdraw amount (CreditAccount account) =
    { account with Balance = account.Balance - amount }
    |> rateAccount

let withdrawSafe amount ratedAccount =
    match ratedAccount with
    | Credit account -> account |> withdraw amount
    | Overdrawn _ -> 
        printfn "Your account is overdrawn - withdrawal rejected!"
        ratedAccount

let auditAs operationName audit operation amount account accountId owner =
    let transaction = { Amount = amount; Operation = operationName; Timestamp = DateTime.UtcNow }
    let updatedAccount = operation amount account
    audit accountId owner.Name transaction
    updatedAccount

let tryParseSerializedOperation operation =
    match operation with
    | "withdraw" -> Some Withdraw
    | "deposit" -> Some Deposit
    | _ -> None

let loadAccount (owner, accountId, transactions) =
    let openingAccount =
        {   Balance = 0M
            AccountId = accountId
            Owner = owner }
    
    transactions
    |> List.sortBy (fun x -> x.Timestamp)
    |> List.fold (fun account txn ->
        let operation = tryParseSerializedOperation txn.Operation
        match operation, account with
        | Some Deposit, _ -> account |> deposit txn.Amount
        | Some Withdraw, Credit account -> account |> withdraw txn.Amount
        | Some Withdraw, Overdrawn _ -> account
        | None, _ -> account) openingAccount
