#r "nuget: Plotly.NET"
open Plotly.NET

// Higher Order Functions and Partial Application (currying)

let printNumber format number = format number

let printNumberAsFloat = printNumber (fun x -> printfn $"%0.3f{x}") 

let printNumberAsCurrency = printNumber (fun (x:float) -> printfn "%s" (x.ToString("C")))

printNumberAsFloat 314159264.0
printNumberAsCurrency 314159264.0

// Partial Application & pipelining
[9; 11; 3; 4; 5] |> List.indexed |> Chart.Line |> Chart.show

//Type Inference
let strs = ["1"; "2"; "3"]

// Immutability 
let mutable x = 1 
x <- 2
x = 1

let ls1 = [1; 2; 3]
let ls2 = 4::ls1


// Pattern Matching 

// Recursion

// Computation Expressions (Async, Seq, Task)

// Type Providers
(*
#r "nuget: FSharp.Data"
open FSharp.Data
type TJson = JsonProvider< @"C:\s\genai\leasing\1_jpeg.json">
let data = TJson.GetSample()

*)

