open System

type System = Windows = 0 | Linux = 1 | MacOS = 2

type OS() =
    let mutable pW = 0.0 
    let mutable  pL = 0.0 
    let mutable  pMOS = 0.0 
    
    member this.setProbInfect pForWindows pForLinux pForMacOS =
        pW <- pForWindows
        pL <- pForLinux
        pMOS <- pForMacOS

    member this.getProbOfInfect(os : System) = 
        match os with
        | System.Windows -> pW
        | System.Linux -> pL
        | System.MacOS -> pMOS
    
type Comp(oSystem : System, isInfected : bool) =
    member val os = oSystem with get, set
    member val infect = isInfected with get, set
    member val isInfectedToday = true with get, set

type Lan(newComputers : array<Comp>, newAdMatrix : bool[,], newOS : OS) =
    let mutable computers = newComputers
    let mutable adMatrix = newAdMatrix
    let rand = new Random()
    let os = newOS
    do
        if (computers.Length <> Array2D.length1 adMatrix || computers.Length <> Array2D.length2 adMatrix)
        then 
            raise (ArgumentException("Lenght of computers array disagree with length of matrix!"))

    let isInfectedNow numOfComp =
        rand.Next(100) <= int (100.0 * os.getProbOfInfect(computers.[numOfComp].os))

    let infectAllNeighbor numOfComp =
        for numOfNeigh in 0..computers.Length - 1 do        
            if (adMatrix.[numOfComp, numOfNeigh] = true && not computers.[numOfNeigh].infect && (isInfectedNow numOfNeigh))
            then
                computers.[numOfNeigh].infect <- true
                computers.[numOfNeigh].isInfectedToday <- true

    let nextStep() =
        for i in 0..computers.Length - 1 do
            if (computers.[i].infect = true && computers.[i].isInfectedToday = false)
            then
                infectAllNeighbor i 

    let nextStepBegin() =
        for comp in computers do
            comp.isInfectedToday <- false

    member this.NextStep() =
        nextStepBegin() 
        nextStep()
    
    member this.Computers
        with get() =
            computers
