#r "nuget: Plotly.NET"
open Plotly.NET
open System

// Higher Order Functions and Partial Application (currying)

let printNumber format number = format number

let printNumberAsFloat = printNumber (fun x -> printfn $"%0.3f{x}") 

let printNumberAsCurrency = printNumber (fun (x:float) -> printfn "%s" (x.ToString("C")))

printNumberAsFloat 314159264.0
printNumberAsCurrency 314159264.0

// Partial Application & pipelining
314159264.0 |> printNumberAsFloat 
314159264.0 |> printNumber (fun x -> printfn $"The number is: %0.3f{x}")
//             ------------------partial application---------------------

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

let rec factorial n = 
    match n with
    | 0 -> 1
    | _ -> n * factorial (n - 1)

//Discriminated Union
type Cases = 
    | Case1 of string 
    | Case2 of int
    | Case3 of DateTime

let cases = [Case1 "a"; Case2 1; Case3 DateTime.Now]

let rec printCases cases = 
    match cases with
    | [] -> ()
    | Case1 s::rest -> printfn $"Case1: {s}"; printCases rest
    | Case2 i::rest -> printfn $"Case2: {i}"; printCases rest
    | _::rest -> printCases rest

// Recursion
let ls3 = [1; 2; 3; 4; 5]

let rec fSum acc ls = 
    match ls with
    | [] -> acc
    | h::t -> fSum (acc + h) t

fSum 0 ls3

//Map, Filter, Fold, Scan
let ls4 = List.map (fun x -> x * 2) ls3
let ls5 = List.filter (fun x -> x % 2 = 0) ls3
let sum = List.fold (fun acc x -> acc + x) 0 ls3
let sums = List.scan (fun acc x -> acc + x) 0 ls3

// List Comprehensions
let cList1 = [for i in 1 .. 10 -> i * 2]
let cArray1 = [| for i in 1..10 -> i * 2 |]

// Computation Expressions (Async, Seq, Task)
let c1 = seq { for i in 1..10 -> i * 2 }

let s2 = async { return 1 + 1 }
Async.RunSynchronously s2
Async.Start
let t1 = task { return 1 + 1 }
t1 |> Async.AwaitTask |> Async.RunSynchronously

// Type Providers
(*
*)
type Choice = {choices: string list}
let cs1 = {choices = ["a"; "b"; "c"]}
cs1.choices.[0]


#r "nuget: FSharp.Data"
open FSharp.Data
type TJson = JsonProvider< @"C:\s\genai\leasing\1_jpeg.json">
let data = TJson.GetSample()
data.Choices.[0]

