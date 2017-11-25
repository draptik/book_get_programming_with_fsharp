// Gets the distance to a given destination
let getDistance (destination) =
    if destination = "Gas" then 10
    elif destination = "Home" then 25
    elif destination = "Stadium" then 25
    else failwith "Unkown destination!"

getDistance("Home") = 25
getDistance("Stadium") = 25
