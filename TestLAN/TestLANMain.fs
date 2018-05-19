open NUnit.Framework
open FsUnit
open System
open LAN

[<Test>]
let ``test LAN 1`` () =
    let os = new OS()
    os.setProbInfect 1.0 0.0 0.0
    let lan = new Lan([|new Comp(System.Windows, false)|], Array2D.create 1 1 true, os)
    lan.NextStep()
    (lan.Computers.Length = 1 && lan.Computers.[0].infect = false) |> should be True

let ``test LAN 2`` () =
    let os = new OS()
    os.setProbInfect 1.0 0.0 1.0
    let lan = new Lan([|new Comp(System.MacOS, true); new Comp(System.Linux, false)|], Array2D.create 2 2 false, os)
    for i in 0..100 do
        lan.NextStep()
    let comp = lan.Computers
    (comp.Length = 2 && comp.[0].infect = true && comp.[1].infect = false) |> should be True

let ``test LAN 3`` () =
    let os = new OS()
    os.setProbInfect 1.0 0.0 1.0
    fun() -> new Lan([|new Comp(System.MacOS, true); new Comp(System.Linux, false)|], Array2D.create 2 3 false, os) |> ignore |> should throw typeof<ArgumentException>  

let ``test LAN 4`` () =
    let os = new OS()
    os.setProbInfect 1.0 1.0 0.5
    
    let mutable matrix = Array2D.create 3 3 true
    matrix.[0, 1] <- false
    matrix.[0, 2] <- false
    matrix.[1, 0] <- false
    matrix.[2, 0] <- false

    let lan = new Lan([|new Comp(System.Windows, false); new Comp(System.Linux, false); new Comp(System.MacOS, true)|], matrix, os)
    lan.NextStep()
    let comp = lan.Computers
    (lan.Computers.[0].infect = false && comp.[1].infect = true) |> should be True

[<EntryPoint>]
let main argv =
    ``test LAN 1`` ()
    ``test LAN 2`` ()
    ``test LAN 3`` ()
    ``test LAN 4`` ()
    0
