module Capstone4.FileRepository

open Capstone4.Domain
open Transactions
open System.IO
open System

let private accountsPath =
    let path = @"accounts"
    Directory.CreateDirectory path |> ignore
    path

// Only need when working from REPL
let patchedAccountsPath = Path.Combine(accountsPath)

let tryFindAccountFolder owner =
    let folders = Directory.EnumerateDirectories(patchedAccountsPath, sprintf "%s_*" owner)    
    if Seq.isEmpty folders then None
    else
        let folder = Seq.head folders
        Some (DirectoryInfo(folder).Name)

let private buildPath(owner, accountId:Guid) = 
    Path.Combine(patchedAccountsPath, sprintf @"%s_%O" owner accountId)

let loadTransactions (folder: string) =
    let owner, accountId =
        let parts = folder.Split '_'
        parts.[0], Guid.Parse parts.[1]

    {Name = owner}, accountId, buildPath(owner, accountId)
    |> Directory.EnumerateFiles
    |> Seq.map (File.ReadAllText >> deserialize)
    |> Seq.toList

let tryFindTransactionsOnDisk = tryFindAccountFolder >> Option.map loadTransactions

let writeTransaction accountId owner (transaction: Transaction) =
    let path = buildPath(owner, accountId)
    path |> Directory.CreateDirectory |> ignore
    let filePath = Path.Combine(path, sprintf "%d.txt" (DateTime.UtcNow.ToFileTimeUtc()))
    File.WriteAllText(filePath, transaction |> serialized)