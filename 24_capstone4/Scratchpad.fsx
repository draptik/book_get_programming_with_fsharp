#load "Domain.fs"
#load "Operations.fs"
#load "FileRepository.fs"
open Capstone4.Domain
open Capstone4.Operations
open System
open System.IO

// 20.1.1 For loops
for number in 1 .. 10 do
    printfn "%d Hello!" number

for number in 10 .. -1 .. 1 do
    printfn "%d Hello!" number

let customerIds = [ 45 .. 99 ]    
for customerId in customerIds do
    printfn "%d bought something!" customerId

for even in 2 .. 2 .. 10 do
    printfn "%d is an number!" even

// 20.1.2 While loops
open System.IO
let reader = new StreamReader(File.OpenRead @".gitignore")
while (not reader.EndOfStream) do
    printfn "%s" (reader.ReadLine())

// 20.1.3 Comprehensions
open System
let arrayOfChars = [| for c in 'a' .. 'z' -> Char.ToUpper c |]    
let listOfSquares = [ for i in 1 .. 10 -> i * i ]
let seqOfStrings = seq { for i in 2 .. 4 .. 20 -> sprintf "Number %d" i }

// 20.2 Branching logic in F# 

// 20.2.1 Priming Exercise - Customer Credit Limits

let limitIfElse score years =
    if score = "medium" && years = 1 then 500
    elif score = "good" && (years = 0 || years = 1) then 750
    elif score = "good" && years = 2 then 1000
    elif score = "good" then 2000
    else 250
limitIfElse "good" 2 = 1000

let limit customer =
    match customer with
    | "medium", 1 -> 500
    | "good", 0 | "good", 1 -> 750
    | "good", 2 -> 1000
    | "good", _ -> 2000
    | _ -> 250    
limit ("medium", 1) = 500

// 20.2.3 Exhaustive checking

let getCreditLimit customer =
    match customer with
    | "medium", 1 -> 500
    | "good", years when years < 2 -> 750 // 20.2.4 Exhaustive checking
    | "good", 2 -> 1000
    | "good", _ -> 2000
    | _ -> 250    

getCreditLimit ("medium", 1) = 500
getCreditLimit ("bad", 1) = 250
getCreditLimit ("good", 1) = 750 

// 20.2.5 Nested matches
let getCreditLimitNested customer =
    match customer with
    | "medium", 1 -> 500
    | "good", years ->
        match years with
        | 0 | 1 -> 750
        | 2 -> 1000
        | _ -> 2000
    | _ -> 250

getCreditLimitNested ("good", 0) = 750

// 20.3 Flexible pattern matching

// 20.3.1 Collections

type Customer = { Name: string; Balance: int }
let handleCustomer customers =
    match customers with
    | [] -> failwith "No customers supplied!"
    | [ customer ] -> printfn "Single customer, name is %s" customer.Name
    | [ first; second ] -> printfn "Two customers, balance = %d" (first.Balance + second.Balance)
    | customers -> printfn "Customers supplied: %d" customers.Length
 
handleCustomer [] // throws exception
handleCustomer [ { Balance = 10; Name = "Joe" } ] // prints name