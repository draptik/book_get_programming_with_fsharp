
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


type BankOperation = Deposit | Withdraw

type Command = BankCommand of BankOperation | Exit

let tryGetBankOperation cmd =
    match cmd with
    | BankCommand op -> Some op
    | Exit -> None

let tryParseCommand cmd =
    match cmd with
    | 'd' -> Some (BankCommand Deposit)
    | 'w' -> Some (BankCommand Withdraw)
    | 'x' -> Some Exit
    | _ -> None

[ 'a'; 'd'; 'w'; 'x'; 'q' ]
|> Seq.choose tryParseCommand
|> Seq.takeWhile ((<>) Exit)
|> Seq.choose tryGetBankOperation
