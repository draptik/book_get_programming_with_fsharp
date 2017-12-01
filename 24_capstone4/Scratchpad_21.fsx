open System
open System.IO


// 21 Modelling Relationships in F#

// 21.1.1 Composition in F#

type Disk1 = { SizeGb : int }
type Computer =
    { Manufacturer : string
      Disks: Disk1 list }
 
let myPc =
    { Manufacturer = "Computers Inc."
      Disks =
        [ { SizeGb = 100 }
          { SizeGb = 250 }
          { SizeGb = 500 } ] }


// 21.2 Discriminated Unions in F#          

// (used instead of inheritance in OO)

type Disk =
| HardDisk of RPM:int * Platters:int
| SolidState
| MMC of NumberOfPins:int

// 21.2.1 Creating instances of DUs

let myHardDisk = HardDisk(RPM = 250, Platters = 7)
let myHardDiskShort = HardDisk(250, 7)
let args = 250, 7
let myHardDiskTupled = HardDisk args
let myMMC = MMC 5
let mySsd = SolidState

// 21.2.2 Accessing an instance of a DU

let seek disk =
    match disk with
    | HardDisk _ -> "Seeking loudly at a reasonable speed!"
    | MMC _ -> "Seeking quietly but slowly"
    | SolidState -> "Already found it!"
seek mySsd

let describe disk =
    match disk with
    | SolidState -> "I'm a new-fangled SSD."
    | MMC(1) -> "I've only got 1 pin."
    | MMC(pins) when pins < 5 -> "I'm an MMC with a few pins"
    | MMC(pins) -> sprintf "I'm an MMC with %i pins." pins
    | HardDisk(5400, _) -> "I'm a slow hard disk"
    | HardDisk(_, 7) -> "I have 7 spindles!"
    | HardDisk _ -> "I'm a hard disk"

SolidState |> describe
MMC(1) |> describe
MMC(2) |> describe
MMC(10) |> describe
HardDisk(5400, 10) |> describe
HardDisk(5400, 7) |> describe
HardDisk(1000, 7) |> describe
HardDisk(1000, 1) |> describe

// 21.3 Tips for working with Discriminated Unions

// 21.3.1 Nested DUs

type MMCDisk =
    | RsMmc
    | MmcPlus
    | SecureMMC

type DiskNested =
| HardDisk of RPM:int * Platters:int
| SolidState
| MMC of MMCDisk * NumberOfPins:int
//       ^^^^^^^   ^^^^^^^^^^^^
//        field1     field2

// 21.3.2 Shared fields

// OO: shared fields in base class
// FP: record with common field and DU "field" for varying implementation

type DiskInfo =
    {   Manufacturer : string
        SizeGb : int
        DiskData : DiskNested }

type Computer2 = { Manufacturer : string;  Disks : DiskInfo list }

let myPc =
    { Manufacturer = "Computers Inc."
      Disks =
        [ { Manufacturer = "HardDisks Inc."
            SizeGb = 100
            DiskData = HardDisk(5400, 7) }
          { Manufacturer = "SuperDisks Corp."
            SizeGb = 250
            DiskData = SolidState } ] }        

// 21.4.2 Creating Enums

type Printer =
    | Inkjet = 0
    | Laserjet = 1
    | DotMatrix = 2

// (the explicit ordinal is required in F#)    

