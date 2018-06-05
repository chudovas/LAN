open System

/// <summary>
/// enum, отвечающий за тип ОС.
/// </summary>
type System = Windows | Linux | MacOS

/// <summary>
/// Класс операционной системы компьютера.
/// </summary>
type OS() =
    let mutable pWindows = 0.0 
    let mutable pLinux = 0.0 
    let mutable pMacOS = 0.0 
    
    /// <summary>
    /// Изменение вероятности заражения для ОС.
    /// </summary>
    /// <param name="pForWindows">Вероятность для Windows</param>
    /// <param name="pForLinux">Вероятность для Linux</param>
    /// <param name="pForMacOS">Вероятность для MacOS</param>
    member this.SetProbInfect pForWindows pForLinux pForMacOS =
        pWindows <- pForWindows
        pLinux <- pForLinux
        pMacOS <- pForMacOS

    /// <summary>
    /// Функция, для ОС позволяющая получить вероятность заражения.
    /// </summary>
    /// <param name="os">Текущая операционная система</param>
    member this.GetProbInfect(os : System) = 
        match os with
        | System.Windows -> pWindows
        | System.Linux -> pLinux
        | System.MacOS -> pMacOS
    
/// <summary>
/// Класс компьютера.
/// </summary>
type Comp(oSystem : System, isInfected : bool) =
    /// <summary>
    /// Тип ОС
    /// </summary>
    member val os = oSystem with get, set

    /// <summary>
    /// Флаг, показывающий, заражен ли компьютер
    /// </summary>
    member val infected = isInfected with get, set

    /// <summary>
    /// Флаг, показывающий, заразился ли компьютер сегодня.
    /// </summary>
    member val isInfectedToday = true with get, set

/// <summary>
/// Тип локальной сети
/// </summary>
type Lan(newComputers : array<Comp>, newAdMatrix : bool[,], newOS : OS) =
    let mutable computers = newComputers
    let mutable adMatrix = newAdMatrix
    let rand = new Random()
    let os = newOS
    do
        if (computers.Length <> Array2D.length1 adMatrix || computers.Length <> Array2D.length2 adMatrix)
        then 
            raise (ArgumentException("Length of computers array disagree with length of matrix!"))

    let isInfectedNow numOfComp =
        rand.Next(100) <= int (100.0 * os.GetProbInfect(computers.[numOfComp].os))

    let infectAllNeighbor numOfComp =
        for numOfNeigh in 0..computers.Length - 1 do        
            if (adMatrix.[numOfComp, numOfNeigh] = true && not computers.[numOfNeigh].infected && (isInfectedNow numOfNeigh))
            then
                computers.[numOfNeigh].infected <- true
                computers.[numOfNeigh].isInfectedToday <- true

    let nextStep() =
        for i in 0..computers.Length - 1 do
            if (computers.[i].infected = true && computers.[i].isInfectedToday = false)
            then
                infectAllNeighbor i 

    let nextStepBegin() =
        for comp in computers do
            comp.isInfectedToday <- false

    /// <summary>
    /// Получить следующий шаг состояния локальной сети.
    /// </summary>
    member this.NextStep() =
        nextStepBegin() 
        nextStep()
    
    /// <summary>
    /// Получить массив компьютеров.
    /// </summary>
    member this.Computers
        with get() =
            computers
