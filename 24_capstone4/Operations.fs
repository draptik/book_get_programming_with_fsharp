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

let loadAccount (owner, accountId, transactions) =
    let openingAccount =
        {   Balance = 0M
            AccountId = accountId
            Owner = owner }
    
    transactions
    |> List.sortBy (fun x -> x.Timestamp)
    |> List.fold (fun account txn ->
        if txn.Operation = "withdraw" then account |> withdraw txn.Amount
        else account |> deposit txn.Amount) openingAccount
