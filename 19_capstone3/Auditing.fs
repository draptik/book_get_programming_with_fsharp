module Capstone3.Auditing

open Capstone3.Domain
open System.IO
open Capstone3.Domain.Transactions

let fileSystemAudit (account: Account) (transaction: Transaction) =
    // Linux path:
    let basePath = "/home/patrick/projects/book_get_programming_with_fsharp/19_capstone3/logging"
    Directory.CreateDirectory(sprintf "%s/%s" basePath account.Owner.Name) |> ignore
    let filePath = sprintf "%s/%s/%O.txt" basePath account.Owner.Name account.AccountId
    File.AppendAllLines(filePath, [ sprintf "Account %O: %s" account.AccountId (transaction |> serialized) ])


let console (account: Account) (transaction: Transaction) =
    printfn "Account %O: %s" account.AccountId (transaction |> serialized)