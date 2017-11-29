module Capstone3.FileRepository

open Capstone3.Domain
open Transactions
open System.IO
open System

let private accountsPath =
    let path = @"accounts"
    Directory.CreateDirectory path |> ignore
    path

let findAccountFolder owner =
    let folders = Directory.EnumerateDirectories(accountsPath, sprintf "%s_*" owner)    
    if Seq.isEmpty folders then ""
    else
        let folder = Seq.head folders
        DirectoryInfo(folder).Name

let private buildPath(owner, accountId:Guid) = 
    Path.Combine(accountsPath, sprintf @"%s_%O" owner accountId)

let writeTransaction accountId owner (transaction: Transaction) =
    let path = buildPath(owner, accountId)
    path |> Directory.CreateDirectory |> ignore
    let filePath = Path.Combine(path, sprintf "%d.txt" (DateTime.UtcNow.ToFileTimeUtc()))
    File.WriteAllText(filePath, transaction |> serialized)