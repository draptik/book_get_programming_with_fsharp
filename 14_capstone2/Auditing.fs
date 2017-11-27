module Capstone2.Auditing

open Capstone2.Domain
open System.IO

let fileSystemAudit (account: Account) (message: string) =
    // target location: './logging/{customerName}/{accountId}.txt
    // content format: "Account <accountId>: <message>"
    // content example: "Account d89ac062-c777-4336-8192-6fba87920f3c: Performed operation 'withdraw' for 50EUR. Balance is now 75EUR"
    // Linux path:
    let basePath = "/home/patrick/projects/book_get_programming_with_fsharp/14_capstone2/logging"
    Directory.CreateDirectory(sprintf "%s/%s" basePath account.Owner.Name) |> ignore
    let filePath = sprintf "%s/%s/%O.txt" basePath account.Owner.Name account.AccountId
    File.AppendAllLines(filePath, [ sprintf "Account %O: %s" account.AccountId message ])


let console (account: Account) (message: string) =
    printfn "Account %O: %s" account.AccountId message
