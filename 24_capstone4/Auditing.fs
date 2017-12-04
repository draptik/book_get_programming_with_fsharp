module Capstone4.Auditing

open Capstone4.Domain

open FileRepository

let printTransaction _ accountId transaction =
    printfn "Account %O: %s of %M" accountId transaction.Operation transaction.Amount


let composedLogger =
    let loggers = [ writeTransaction; printTransaction ]    
    fun accountId owner transaction ->
        loggers
        |> List.iter (fun logger -> logger accountId owner transaction)

// let fileSystemAudit account transaction =
//     writeTransaction account.AccountId account.Owner.Name transaction |> ignore

// let console account transaction =
//     printfn "Account %O: %s" account.AccountId (transaction |> serialized)
