module Capstone3.FileRepository

open Capstone3.Domain
open Capstone3.Domain.Transactions
open System.IO
open System

let private accountsPath =
    let path = @"logging"
    Directory.CreateDirectory path |> ignore
    path

let findAccountFolder owner =
    let folders = Directory.EnumerateDirectories(accountsPath, sprintf "%s_" owner)    
    printfn "%A" folders
    if Seq.isEmpty folders then ""
    else
        let folder = Seq.head folders
        DirectoryInfo(folder).Name

let private buildPath(owner, accountId:Guid) = 
    sprintf @"%s%s_%O" accountsPath owner accountId

let writeTransaction accountId owner (transaction: Transaction) =
    let path = buildPath(owner, accountId)
    path |> Directory.CreateDirectory |> ignore
    let filePath = sprintf "%s/%d.txt" path (DateTime.UtcNow.ToFileTimeUtc())
    File.WriteAllText(filePath, transaction |> serialized)