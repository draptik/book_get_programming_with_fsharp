open System.Security.Cryptography.X509Certificates

// 24.2.2   Adding a command handler with discriminated unions

// type Command =
// | Withdraw
// | Deposit
// | Exit

// let tryParseCommand cmd =
//     match cmd with
//     | 'd' -> Some Deposit
//     | 'w' -> Some Withdraw
//     | 'x' -> Some Exit
//     | _ -> None

// [ 'a'; 'd'; 'w'; 'x'; 'q' ]
// |> Seq.choose tryParseCommand
// |> Seq.takeWhile ((<>) Exit)



// 24.2.3 Tightening the model further


// type BankOperation = Deposit | Withdraw

// type Command = BankCommand of BankOperation | Exit

// let tryGetBankOperation cmd =
//     match cmd with
//     | BankCommand op -> Some op
//     | Exit -> None

// let tryParseCommand cmd =
//     match cmd with
//     | 'd' -> Some (BankCommand Deposit)
//     | 'w' -> Some (BankCommand Withdraw)
//     | 'x' -> Some Exit
//     | _ -> None

// [ 'a'; 'd'; 'w'; 'x'; 'q' ]
// |> Seq.choose tryParseCommand
// |> Seq.takeWhile ((<>) Exit)
// |> Seq.choose tryGetBankOperation


// open System

// let result = "10" |> Int32.TryParse
// match result with
// | true, result -> printfn "x is a string that parses to %i" result
// | false, _ -> printfn "default"


// 24.4 Implementing business rules with types

open System

type Customer = { Name: string }

type Account = { Balance: decimal; AccountId: Guid; Owner: Customer }

// This is an account with non-negative balance (marker DU)
type CreditAccount = CreditAccount of Account

type RatedAccount =
    | Credit of CreditAccount
    | Overdrawn of Account

let rateAccount account =
    if account.Balance < 0M then Overdrawn account
    else Credit(CreditAccount account)    

// This function demonstrates the power of modelling with the type system!
// The *compiler* will not allow passing in an account with negative balance!
let withdraw amount (CreditAccount account) =
    { account with Balance = account.Balance - amount }
    |> rateAccount

// This helper function accepts any RatedAccount.
// So this function also accepts an account which is overdrawn,
// BUT if the account is overdrawn (and therefore has type Overdrawn) it just returns the current state.
let withdrawSafe amount ratedAccount =
    match ratedAccount with
    | Credit account -> account |> withdraw amount
    | Overdrawn _ -> 
        printfn "Your account is overdrawn - withdrawal rejected!"
        ratedAccount

// This function unboxes a RatedAccount, then increases the Balance, and returns the newly account
let deposit amount account =
    let account =
        match account with
        | Credit (CreditAccount account) -> account
        | Overdrawn account -> account
    { account with Balance = account.Balance + amount }
    |> rateAccount

// REPL Testing:
let emptyAccount = { Balance = 0M; AccountId = Guid.Empty; Owner = { Name = "Homer" }} |> rateAccount

// COOL! The following line won't compile: emptyAccount is of type RatedAccount, but it must be of type CreditAccount:
// emptyAccount |> withdraw 10M 

emptyAccount |> deposit 10M // this compiles just fine

// Final Balance is -100M (not -200M), because the first withdraw passes, but the 2nd withdraw doesn't
emptyAccount
|> withdrawSafe 100M
|> withdrawSafe 100M
