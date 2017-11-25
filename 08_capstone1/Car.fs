
module Car

let getDistance (destination) =
    if destination = "Gas" then 10
    else 25

let calculateRemainingPetrol(currentPetrol:int, distance:int) : int =
    if currentPetrol >= distance then currentPetrol - distance
    else failwith "Ooops! You've run out of petrol!"

let driveTo(petrol: int, destination: string) : int =
    let distance = getDistance(destination)
    let remainingPetrol = calculateRemainingPetrol(petrol, distance)
    if destination = "Gas" then remainingPetrol + 50
    else remainingPetrol
