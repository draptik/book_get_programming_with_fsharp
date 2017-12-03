// 23 Business Rules as Code

// 23.1 Specific types in F#

// type Customer =
//     {   CustomerId: string
//         Email: string
//         Telephone: string
//         Address: string }

// 23.1.1   Mixing values of the same type

// Sample code with errors:
// let createCustomer customerId email telephone address =
//     { CustomerId = telephone
//       Email = customerId
//       Telephone = address
//       Address = email }

// let customer =
//     createCustomer "C-123" "nicki@myemail.com" "029-293-23" "1 The Street"        


// 23.1.2   Single Case Discriminated Unions

// type Address = Address of string
// let myAddress = Address "1 The Street"
// let isTheSameAddress = (myAddress = "1 The Street") //won't compile
// let (Address addressData) = myAddress

type CustomerId = CustomerId of string
type Email = Email of string
type Telephone = Telephone of string
type Address = Address of string

type Customer =
    {   CustomerId: CustomerId
        Email: Email
        Telephone: Telephone
        Address: Address}

let createCustomer customerId email telephone address =
    { CustomerId = customerId
      Email = email
      Telephone = telephone
      Address = address }

createCustomer 
    (CustomerId "C-123")
    (Email "nicki@myemail.com")
    (Telephone "029-293-23")
    (Address "1 The Street")

    
     