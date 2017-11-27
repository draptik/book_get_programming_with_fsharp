open System
open System.IO

type Customer = 
    { Name: string }

type Account =
    { Balance: decimal
      AccountId: Guid
      Owner: Customer }
    
/// Deposits an amount into an account
let deposit (amount: decimal) (account: Account) : Account =
    { account with Balance = account.Balance + amount } 

let withdraw (amount: decimal) (account: Account) : Account =
    if amount > account.Balance then account
    else {  account with Balance = account.Balance - amount } 

// Test
let bob = { Name = "Bob" }    
let bobsId = Guid.NewGuid()

let bobsAccount =
    { Balance = 0M
      AccountId = bobsId
      Owner = bob }

bobsAccount 
|> deposit 10M
|> deposit 20M
|> withdraw 2M
|> withdraw 2000M
// should return 28


let fileSystemAudit (account: Account) (message: string) =
    // target location: './logging/{customerName}/{accountId}.txt
    // content format: "Account <accountId>: <message>"
    // content example: "Account d89ac062-c777-4336-8192-6fba87920f3c: Performed operation 'withdraw' for 50EUR. Balance is now 75EUR"
    // Linux path:
    let basePath = "/home/patrick/projects/book_get_programming_with_fsharp/14_capstone2/logging"
    Directory.CreateDirectory(sprintf "%s/%s" basePath account.Owner.Name) |> ignore
    let filePath = sprintf "%s/%s/%O.txt" basePath account.Owner.Name account.AccountId
    File.AppendAllLines(filePath, [ sprintf "Account %O: %s" account.AccountId message ])

// Test
fileSystemAudit bobsAccount "foo"

let console (account: Account) (message: string) =
    printfn "Account %O: %s" account.AccountId message

// Test
console bobsAccount "foo"    

let auditAs operationName audit operation amount account =
    audit account  (sprintf "%O: Perfoming a %s operation for %MEUR..." DateTime.UtcNow operationName amount)
    let updatedAccount = operation amount account
    let accountIsUnchanged = (updatedAccount = account)
    if accountIsUnchanged then audit account (sprintf "%O: Transaction rejected!" DateTime.UtcNow)
    else audit account (sprintf "%O: Transaction accepted! Balance is now %MEUR." DateTime.UtcNow updatedAccount.Balance)
    updatedAccount

let withdrawWithConsoleAudit = auditAs "withdraw" console withdraw
let depositWithConsoleAudit = auditAs "deposit" console deposit

bobsAccount
|> depositWithConsoleAudit 1000M
|> withdrawWithConsoleAudit 50M

