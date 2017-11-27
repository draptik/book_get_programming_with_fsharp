open System

type Customer = 
    { Name: string }

type Account =
    { Balance: decimal
      AccountId: Guid
      Owner: Customer }
    
/// Deposits an amount into an account
let deposit (amount:decimal) (account:Account) : Account =
    { account with Balance = account.Balance + amount } 

let withdraw (amount:decimal) (account:Account) : Account =
    if amount > account.Balance then account
    else {  account with Balance = account.Balance - amount } 

let bob = { Name = "Bob" }    
let bobsId = Guid.NewGuid()

let bobsAccount =
    { Balance = 0M
      AccountId = bobsId
      Owner = bob }

bobsAccount 
|> deposit 10M
|> deposit 20M
|> withdraw 2M
|> withdraw 2000M
// should return 28
