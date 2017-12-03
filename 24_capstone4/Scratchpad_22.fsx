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

