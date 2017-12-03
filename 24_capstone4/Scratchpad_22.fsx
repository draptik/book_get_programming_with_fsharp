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
