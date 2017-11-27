module Capstone2.Operations

open Capstone2.Domain

let deposit (amount: decimal) (account: Account) : Account =
    { account with Balance = account.Balance + amount } 

let withdraw (amount: decimal) (account: Account) : Account =
    if amount > account.Balance then account
    else {  account with Balance = account.Balance - amount } 
