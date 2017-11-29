module Capstone3.Operations

open System
open Capstone3.Domain

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
            Accepted = true
        }
    audit account transaction
    let updatedAccount = operation amount account
    let accountIsUnchanged = (updatedAccount = account)
    if accountIsUnchanged then audit account { transaction with Accepted = false }
    else audit account transaction
    updatedAccount
