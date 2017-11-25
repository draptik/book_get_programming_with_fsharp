open System

type Customer = 
    { Name: string }

type Account =
    { Balance: decimal
      AccountId: Guid
      Owner: Customer }
    
/// Deposits an amount into an account
let deposit (amount:decimal) (account:Account) : Account =
    { AccountId = Guid.Empty; Owner = { Name = "Sam" }; Balance = 10M } 