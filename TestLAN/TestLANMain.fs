open NUnit.Framework
open FsUnit
open System
open LAN

[<Test>]
let ``test of LAN class 1`` () =
    let os = new OS()
    os.SetProbInfect 1.0 0.0 0.0
    let lan = new Lan([|new Comp(System.Windows, false)|], Array2D.create 1 1 true, os)
    lan.NextStep()
    (lan.Computers.Length = 1 && lan.Computers.[0].infected = false) |> should be True

let ``test of LAN class 2`` () =
    let os = new OS()
    os.SetProbInfect 1.0 0.0 1.0
    let lan = new Lan([|new Comp(System.MacOS, true); new Comp(System.Linux, false)|], Array2D.create 2 2 false, os)
    for i in 0..100 do
        lan.NextStep()
    let comp = lan.Computers
    (comp.Length = 2 && comp.[0].infected = true && comp.[1].infected = false) |> should be True

let ``test of LAN class 3`` () =
    let os = new OS()
    os.SetProbInfect 1.0 0.0 1.0
    (fun() -> new Lan([|new Comp(System.MacOS, true); new Comp(System.Linux, false)|], Array2D.create 2 3 false, os)) |> ignore |> should throw typeof<ArgumentException>  

let ``test of LAN class 4`` () =
    let os = new OS()
    os.SetProbInfect 1.0 1.0 0.5
    
    let mutable matrix = Array2D.create 3 3 true
    matrix.[0, 1] <- false
    matrix.[0, 2] <- false
    matrix.[1, 0] <- false
    matrix.[2, 0] <- false

    let lan = new Lan([|new Comp(System.Windows, false); new Comp(System.Linux, false); new Comp(System.MacOS, true)|], matrix, os)
    lan.NextStep()
    let comp = lan.Computers
    (lan.Computers.[0].infected = false && comp.[1].infected = true) |> should be True

[<EntryPoint>]
let main argv =
    ``test of LAN class 1`` ()
    ``test of LAN class 2`` ()
    ``test of LAN class 3`` ()
    ``test of LAN class 4`` ()
    0
