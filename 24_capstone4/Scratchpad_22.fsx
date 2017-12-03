open System.Web.Configuration
let calculateAnnualPremiumUsd score =
    match score with
    | Some 0 -> 250
    | Some score when score < 0 -> 400
    | Some score when score > 0 -> 150
    | None ->
        printfn "No score supplied. Using temporary premium."
        300
calculateAnnualPremiumUsd (Some 0) = 250
calculateAnnualPremiumUsd (Some 10) = 150
calculateAnnualPremiumUsd (Some -10) = 400
calculateAnnualPremiumUsd None = 300

// Exercise

type Customer =
    {   Name: string
        YearPassed: int
        SafetyScore: Option<int>}

let drivers =
    [   {   Name = "Fred Smith"; SafetyScore = Some (550); YearPassed = 1980 };
        {   Name = "Jan Dunn"; SafetyScore = None; YearPassed = 1980 }]

let calculateAnnualPremiumUsdCustomer (customer: Customer) =
    match customer.SafetyScore with
    | Some 0 -> 250
    | Some score when score < 0 -> 400
    | Some score when score > 0 -> 150
    | None ->
        printfn "No score supplied. Using temporary premium."
        300
calculateAnnualPremiumUsdCustomer drivers.[0] = 150
calculateAnnualPremiumUsdCustomer drivers.[1] = 300


// 22.3 The Option Module

// 22.3.1 Mapping

// let description =
//     match customer.SafetyScore with
//     | Some score -> Some(describe score)
//     | None -> None
 
// let descriptionTwo =
//     customer.SafetyScore
//     |> Option.map(fun score -> describe score)
 
// let shorthand = customer.SafetyScore |> Option.map describe
// let optionalDescribe = Option.map describe

// 22.3.2 Binding

// Same as Option.map, except it returns Options.

// let tryFindCustomer cId = if cId = 10 then Some drivers.[0] else None
// let getSafetyScore customer = customer.SafetyScore
// let score = tryFindCustomer 10 |> Option.bind getSafetyScore

// 22.3.3 Filtering

// let test1 = Some 5 |> Option.filter(fun x -> x > 5)
// let test2 = Some 5 |> Option.filter(fun x -> x = 5)

// 22.3.4 Other Option functions

// Option.count
// Option.exists

// 22.4.1 Option.toList

// Takes an optional value in, and if it's Some value, returns a llist with that single value in it.

// 22.4.2 List.choose

// Very useful: combination of map and filter; It allows you to apply a function that might return a value, and then automatically strips out any of the items that returned None.

let tryLoadCustomer customerId =
    match customerId with
    | id when id >= 2 && id <= 7 -> Some (sprintf "Customer %i" id)
    | _ -> None

[ 1 .. 10 ] 
|> List.choose tryLoadCustomer 
|> printfn "%A"
