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

// Exercise

// type CustomerId = CustomerId of string
// type Email = Email of string
// type Telephone = Telephone of string
// type Address = Address of string

// type Customer =
//     {   CustomerId: CustomerId
//         Email: Email
//         Telephone: Telephone
//         Address: Address}

// let createCustomer customerId email telephone address =
//     { CustomerId = customerId
//       Email = email
//       Telephone = telephone
//       Address = address }

// createCustomer 
//     (CustomerId "C-123")
//     (Email "nicki@myemail.com")
//     (Telephone "029-293-23")
//     (Address "1 The Street")

// Exercise

// type CustomerId = CustomerId of string

// type ContactDetails =
// | Email of string
// | Telephone of string
// | Address of string

// type Customer =
//     {   CustomerId: CustomerId
//         ContactDetails: ContactDetails }

// let createCustomer customerId contactDetails =
//     { CustomerId = customerId
//       ContactDetails = contactDetails }

// createCustomer 
//     (CustomerId "C-123")
//     (Email "nicki@myemail.com")

// We can now guarantee that one and only one type of contact is supplied e.g. Telephone.    



// 23.1.3   Using Optional values within a domain

// Exercise


// type CustomerId = CustomerId of string

// type ContactDetails =
// | Email of string
// | Telephone of string
// | Address of string

// type Customer =
//     {   CustomerId: CustomerId
//         PrimaryContactDetails: ContactDetails 
//         SecondaryContactDetails: ContactDetails option }

// let createCustomer customerId primaryContactDetails secondaryContactDetails =
//     { CustomerId = customerId
//       PrimaryContactDetails = primaryContactDetails
//       SecondaryContactDetails = secondaryContactDetails }

// let customer =
//     createCustomer 
//         (CustomerId "C-123")
//         (Email "nicki@myemail.com")
//         (Some (Address "1 Street"))

// printfn "%A" customer



// 23.2   Encoding business rules with marker types     

// Exercise

type CustomerId = CustomerId of string

type ContactDetails =
| Email of string
| Telephone of string
| Address of string

type Customer =
    {   CustomerId: CustomerId
        PrimaryContactDetails: ContactDetails 
        SecondaryContactDetails: ContactDetails option }

type GenuineCustomer = GenuineCustomer of Customer

let createCustomer customerId primaryContactDetails secondaryContactDetails =
    { CustomerId = customerId
      PrimaryContactDetails = primaryContactDetails
      SecondaryContactDetails = secondaryContactDetails }

let validateCustomer customer =
    match customer.PrimaryContactDetails with
    | Email e when e.EndsWith "SuperCorp.com" -> Some(GenuineCustomer customer)
    | Address _ | Telephone _ -> Some(GenuineCustomer customer)
    | Email _ -> None
 
let sendWelcomeEmail (GenuineCustomer customer) =
    printfn "Hello, %A, and welcome to our site!" customer.CustomerId


let customer =
    createCustomer 
        (CustomerId "C-123")
        (Address "nicki@SuperCorp.com")
        (Some (Address "1 Street"))

validateCustomer customer    




// 23.3   Results vs Exceptions

// type Result<'a> =
// | Success of 'a
// | Failure of string
 
// insertCustomer : contactDetails:ContactDetails -> Result<CustomerId>
 
// match insertContact (Email "nicki@myemail.com") with
// | Success customerId -> printfn "Saved with %A" customerId
// | Failure error -> printfn "Unable to save: %s" error

