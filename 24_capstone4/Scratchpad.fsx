
// 24.2.2   Adding a command handler with discriminated unions

type Command =
| Withdraw
| Deposit
| Exit

let tryParseCommand cmd =
    match cmd with
    | 'd' -> Some Deposit
    | 'w' -> Some Withdraw
    | 'x' -> Some Exit
    | _ -> None

[ 'a'; 'd'; 'w'; 'x'; 'q' ]
|> Seq.choose tryParseCommand
|> Seq.takeWhile ((<>) Exit)


[ Some(Withdraw); Some(Deposit); Some(Exit); Some(Deposit) ]
|> Seq.takeWhile (Withdraw)
