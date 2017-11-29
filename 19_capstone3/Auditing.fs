module Capstone3.Auditing

open Capstone3.Domain
open System.IO

let fileSystemAudit (account: Account) (message: string) =
    // Linux path:
    let basePath = "/home/patrick/projects/book_get_programming_with_fsharp/19_capstone3/logging"
    Directory.CreateDirectory(sprintf "%s/%s" basePath account.Owner.Name) |> ignore
    let filePath = sprintf "%s/%s/%O.txt" basePath account.Owner.Name account.AccountId
    File.AppendAllLines(filePath, [ sprintf "Account %O: %s" account.AccountId message ])


let console (account: Account) (message: string) =
    printfn "Account %O: %s" account.AccountId message
