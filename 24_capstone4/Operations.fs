module Capstone4.Operations

open System
open Capstone4.Domain

let deposit amount account =
    { account with Balance = account.Balance + amount } 

let withdraw amount account =
    if amount > account.Balance then account
    else {  account with Balance = account.Balance - amount } 

let auditAs operationName audit operation amount account =
    let transaction = 
        {   Amount = amount
            Operation = operationName
            Timestamp = DateTime.UtcNow
            Accepted = true }
    let updatedAccount = operation amount account
    let accountIsUnchanged = (updatedAccount = account)
    if accountIsUnchanged then audit account { transaction with Accepted = false }
    else audit account transaction
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
        | Some Withdraw, account -> account |> withdraw txn.Amount
        | None, _ -> account) openingAccount
