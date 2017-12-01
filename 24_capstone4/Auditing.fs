module Capstone4.Auditing

open Capstone4.Domain

open Transactions
open FileRepository

let fileSystemAudit account transaction =
    writeTransaction account.AccountId account.Owner.Name transaction |> ignore

let console account transaction =
    printfn "Account %O: %s" account.AccountId (transaction |> serialized)
