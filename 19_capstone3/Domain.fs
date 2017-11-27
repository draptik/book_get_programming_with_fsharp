namespace Capstone3.Domain

open System

type Customer = 
    { Name: string }

type Account =
    { Balance: decimal
      AccountId: Guid
      Owner: Customer }
