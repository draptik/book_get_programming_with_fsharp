module Capstone3.Auditing

open Capstone3.Domain

open Transactions
open FileRepository

let fileSystemAudit (account: Account) (transaction: Transaction) =
    writeTransaction account.AccountId account.Owner.Name transaction |> ignore

let console (account: Account) (transaction: Transaction) =
    printfn "Account %O: %s" account.AccountId (transaction |> serialized)
