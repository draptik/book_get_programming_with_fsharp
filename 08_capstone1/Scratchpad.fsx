// Gets the distance to a given destination
let getDistance (destination) =
    if destination = "Gas" then 10
    elif destination = "Home" then 25
    elif destination = "Stadium" then 25
    else failwith "Unkown destination!"

getDistance("Home") = 25
getDistance("Stadium") = 25

let calculateRemainingPetrol(currentPetrol:int, distance:int) : int =
    if currentPetrol >= distance then currentPetrol - distance
    else failwith "Ooops! You've run out of petrol!"

calculateRemainingPetrol(1,1) = 0
calculateRemainingPetrol(10,5) = 5
//calculateRemainingPetrol(1,2) = 0

