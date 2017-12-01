#load "Domain.fs"
#load "Operations.fs"
#load "FileRepository.fs"
open Capstone3.Domain
open Capstone3.Operations
open System
open System.IO

let openingAccount =
        { Owner = { Name = "Isaac" }; Balance = 0M; AccountId = Guid.Empty }

let isValidCommand command =
    let validCommands = ['d'; 'w'; 'x']
    validCommands |> Seq.contains command
// isValidCommand 'a'
// isValidCommand 'd'

let isStopCommand command =
    'x' = command
// isStopCommand 'x'

let getAmount (command:char) =
    if command = 'd' then 'd', 50M
    elif command = 'w' then 'w', 25M
    else command, 0M

let processCommand (account:Account) (command:char, amount:decimal) =
    match command with
    | 'd' -> deposit amount account 
    | 'w' -> withdraw amount account
    | _ -> account
// let acc1 = processCommand openingAccount ('d', 100M)
// let acc2 = processCommand acc1 ('d', 100M)
// let acc3 = processCommand acc2 ('w', 50M)

let account =
    let commands = [ 'd'; 'w'; 'z'; 'f'; 'd'; 'x'; 'w' ]

    commands
    |> Seq.filter isValidCommand
    |> Seq.takeWhile (not << isStopCommand)
    |> Seq.map getAmount
    |> Seq.fold processCommand openingAccount

let loadAccount (owner: Customer) (accountId: Guid) (transactions: Transaction list) : Account =
    let openingAccount =
        {   Balance = 0M
            AccountId = accountId
            Owner = owner }
    
    transactions
    |> List.sortBy (fun x -> x.Timestamp)
    |> List.fold (fun account txn ->
        if txn.Operation = "withdraw" then account |> withdraw txn.Amount
        else account |> deposit txn.Amount) openingAccount

let transactions =
    [
        {Amount = 100M; Operation = "deposit"; Accepted = true; Timestamp = DateTime.UtcNow}
        {Amount = 20M; Operation = "deposit"; Accepted = true; Timestamp = DateTime.UtcNow}
        {Amount = 30M; Operation = "withdraw"; Accepted = true; Timestamp = DateTime.UtcNow}
    ]
// loadAccount { Name = "Patrick" } Guid.Empty transactions

let accountsPath =
    let path = "accounts"
    Directory.CreateDirectory path |> ignore
    path

let findAccountFolder owner =
    let folders = Directory.EnumerateDirectories(accountsPath, sprintf "%s_*" owner)    
    if Seq.isEmpty folders then ""
    else
        let folder = Seq.head folders
        DirectoryInfo(folder).Name
// findAccountFolder "Patrick" = "Patrick_00000000-0000-0000-0000-000000000000"

let buildPath(owner, accountId:Guid) = 
    Path.Combine(accountsPath, sprintf @"%s_%O" owner accountId)
// buildPath("Patrick", Guid("00000000-0000-0000-0000-000000000000")) =
//     Path.Combine("accounts", "Patrick_00000000-0000-0000-0000-000000000000")

let findTransactionsOnDisk owner =
    let accountFolder = findAccountFolder owner
    // printfn "AccountFolder : %s" accountFolder
    // TODO 19.4 4
    let owner, accountId =
        let parts = accountFolder.Split '_'
        parts.[0], Guid.Parse parts.[1]

    owner, accountId, buidPath(owner, accountId)
    |> Directory.EnumerateFiles
    |> Seq.map (File.ReadAllText >> Transactions.deserialize)

// findTransactionsOnDisk "Patrick"

