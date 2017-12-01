namespace Capstone4.Domain

open System

type Customer = 
    { Name: string }

type Account =
    { Balance: decimal
      AccountId: Guid
      Owner: Customer }

type Transaction =
    {   Amount: decimal
        Operation: string
        Timestamp: DateTime
        Accepted: bool }

module Transactions =
    
    let serialized transaction =
        sprintf "%O***%s***%M***%b"
            transaction.Timestamp
            transaction.Operation
            transaction.Amount
            transaction.Accepted

    let deserialize (fileContent:string) =
        let parts = fileContent.Split([|"***"|], StringSplitOptions.None)
        {   Timestamp = DateTime.Parse parts.[0]
            Operation = parts.[1]
            Amount = Decimal.Parse parts.[2]
            Accepted = Boolean.Parse parts.[3] }
            