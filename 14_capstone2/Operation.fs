module Capstone2.Operations

open System
open Capstone2.Domain

let deposit amount account =
    { account with Balance = account.Balance + amount } 

let withdraw amount account =
    if amount > account.Balance then account
    else {  account with Balance = account.Balance - amount } 

let auditAs operationName audit operation amount account =
    audit account  (sprintf "%O: Perfoming a %s operation for %MEUR..." DateTime.UtcNow operationName amount)
    let updatedAccount = operation amount account
    let accountIsUnchanged = (updatedAccount = account)
    if accountIsUnchanged then audit account (sprintf "%O: Transaction rejected!" DateTime.UtcNow)
    else audit account (sprintf "%O: Transaction accepted! Balance is now %MEUR." DateTime.UtcNow updatedAccount.Balance)
    updatedAccount
