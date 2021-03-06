﻿// Learn more about F# at http://fsharp.org

open Car
open System

let getDestination() =
    Console.Write("Enter destination: ")
    Console.ReadLine()

let mutable petrol = 100

[<EntryPoint>]
let main argv =
    while true do
        try
            let destination = getDestination()
            printfn "Trying to drive to %s" destination
            petrol <- driveTo(petrol, destination)
            printfn "Made it to %s! You have %d petrol left" destination petrol
        with ex -> printfn "ERROR: %s" ex.Message
    0
