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

// 20.3.2 Records

let getStatus customer =
    match customer with
    | { Balance = 0 } -> "Customer has empty balance!"
    | { Name = "Isaac" } -> "This is a great customer!"
    | { Name = name; Balance = 50 } -> sprintf "%s has a large balance!" name
    | { Name = name } -> sprintf "%s is a normal customer" name
 
{ Balance = 50; Name = "Joe" } |> getStatus
{ Balance = 0; Name = "Joe" } |> getStatus
{ Balance = 10; Name = "Isaac" } |> getStatus
{ Balance = 10; Name = "Other" } |> getStatus


// You can even mix and match patterns – how about checking that a list of customers has three elements, the first customer is called “Tanya” and the second customer has a balance of 25? No problem!
type Customer2 = { Name2: string; Balance2: int }
let checkStrangeCondition customers =
    match customers with
    | [ { Name2 = "Tanya" }; { Balance2 = 25 }; _ ] -> "It's a match!"
    | _ -> "No match!"
checkStrangeCondition [ 
    {Name2 = "Tanya"; Balance2 = 1000 } 
    {Name2 = "Foooo"; Balance2 = 25 } 
    {Name2 = "Baaar"; Balance2 = 500 }]
checkStrangeCondition [ 
    {Name2 = "Tanya"; Balance2 = 1000 } 
    {Name2 = "Foooo"; Balance2 = 25 } 
    {Name2 = "Foooo"; Balance2 = 25 } 
    {Name2 = "Baaar"; Balance2 = 500 }]    

// 20.5 Exercises

// creating a random list of numbers of variable length and writing pattern matches to test if the list:
//
// - Is a specific length
// - Is empty
// - Has the first item equal to 5 (hint: use head / tail syntax here with ::)

// random list of numbers
let genRandomNumbers count =
    let rnd = System.Random()
    List.init count (fun _ -> rnd.Next())

let someCheck someList =
    match someList with
    | [] -> printfn "List is empty!"
    | l when List.length l = 10 -> printfn "List has length 10"
    | head :: _ when head = 5  -> printfn "First entry is 5"
    | _ -> ()

genRandomNumbers 0 |> someCheck
genRandomNumbers 10 |> someCheck
5 :: genRandomNumbers 3 |> someCheck

// 21 Modelling Relationships in F#

// 21.1.1 Composition in F#

type Disk1 = { SizeGb : int }
type Computer =
    { Manufacturer : string
      Disks: Disk1 list }
 
let myPc =
    { Manufacturer = "Computers Inc."
      Disks =
        [ { SizeGb = 100 }
          { SizeGb = 250 }
          { SizeGb = 500 } ] }


// 21.2 Discriminated Unions in F#          

// (used instead of inheritance in OO)

type Disk =
| HardDisk of RPM:int * Platters:int
| SolidState
| MMC of NumberOfPins:int

// 21.2.1 Creating instances of DUs

let myHardDisk = HardDisk(RPM = 250, Platters = 7)
let myHardDiskShort = HardDisk(250, 7)
let args = 250, 7
let myHardDiskTupled = HardDisk args
let myMMC = MMC 5
let mySsd = SolidState

// 21.2.2 Accessing an instance of a DU

let seek disk =
    match disk with
    | HardDisk _ -> "Seeking loudly at a reasonable speed!"
    | MMC _ -> "Seeking quietly but slowly"
    | SolidState -> "Already found it!"
seek mySsd

let describe disk =
    match disk with
    | SolidState -> "I'm a new-fangled SSD."
    | MMC(1) -> "I've only got 1 pin."
    | MMC(pins) when pins < 5 -> "I'm an MMC with a few pins"
    | MMC(pins) -> sprintf "I'm an MMC with %i pins." pins
    | HardDisk(5400, _) -> "I'm a slow hard disk"
    | HardDisk(_, 7) -> "I have 7 spindles!"
    | HardDisk _ -> "I'm a hard disk"

SolidState |> describe
MMC(1) |> describe
MMC(2) |> describe
MMC(10) |> describe
HardDisk(5400, 10) |> describe
HardDisk(5400, 7) |> describe
HardDisk(1000, 7) |> describe
HardDisk(1000, 1) |> describe

// 21.3 Tips for working with Discriminated Unions

// 21.3.1 Nested DUs

type MMCDisk =
    | RsMmc
    | MmcPlus
    | SecureMMC

type DiskNested =
| HardDisk of RPM:int * Platters:int
| SolidState
| MMC of MMCDisk * NumberOfPins:int
//       ^^^^^^^   ^^^^^^^^^^^^
//        field1     field2

// 21.3.2 Shared fields

// OO: shared fields in base class
// FP: record with common field and DU "field" for varying implementation

type DiskInfo =
    {   Manufacturer : string
        SizeGb : int
        DiskData : DiskNested }

type Computer2 = { Manufacturer : string;  Disks : DiskInfo list }

let myPc =
    { Manufacturer = "Computers Inc."
      Disks =
        [ { Manufacturer = "HardDisks Inc."
            SizeGb = 100
            DiskData = HardDisk(5400, 7) }
          { Manufacturer = "SuperDisks Corp."
            SizeGb = 250
            DiskData = SolidState } ] }        

// 21.4.2 Creating Enums

type Printer =
    | Inkjet = 0
    | Laserjet = 1
    | DotMatrix = 2

// (the explicit ordinal is required in F#)    

