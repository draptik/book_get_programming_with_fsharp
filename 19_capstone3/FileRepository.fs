module Capstone3.FileRepository

open Capstone3.Domain
open Transactions
open System.IO
open System

let private accountsPath =
    let path = @"accounts"
    Directory.CreateDirectory path |> ignore
    path

let patchedAccountsPath = Path.Combine(accountsPath)

let findAccountFolder owner =
    let folders = Directory.EnumerateDirectories(patchedAccountsPath, sprintf "%s_*" owner)    
    if Seq.isEmpty folders then ""
    else
        let folder = Seq.head folders
        DirectoryInfo(folder).Name

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

let findTransactionsOnDisk owner =
    let accountFolder = findAccountFolder owner
    if String.IsNullOrEmpty accountFolder then {Name = owner}, Guid.NewGuid(), List.empty
    else loadTransactions accountFolder

let writeTransaction accountId owner (transaction: Transaction) =
    let path = buildPath(owner, accountId)
    path |> Directory.CreateDirectory |> ignore
    let filePath = Path.Combine(path, sprintf "%d.txt" (DateTime.UtcNow.ToFileTimeUtc()))
    File.WriteAllText(filePath, transaction |> serialized)