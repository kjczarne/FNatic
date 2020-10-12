module FNeticTests

open NUnit.Framework
open ByteHandler
open Ipv4

[<SetUp>]
let Setup () =
    ()


[<Test>]
let Test1 () =
    Assert.Pass()


[<Author("Krzysztof Czarnecki", "kjczarne@gmail.com")>]
[<TestFixture>]
type TestByteHandler () =

    // [<Test>]
    // member this.TestRead() =
        
    // [<Test>]
    // member this.TestReadRepeat() =

    [<Test>]
    member this.TestReadRepeat2() =
        let optionsList = readRepeat2 1 1 1 ipv4OptionFromBytes 
                            [| 1uy; 3uy; 1uy; 2uy; 3uy; 1uy; 3uy; 1uy; 2uy; 3uy; |] 
                            []
        // printfn "%d" optionsList.Length
        Assert.True(optionsList.Length = 2)
