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

    [<Test>]
    member this.TestRead() =
        let stream = [| 1uy; 2uy; 3uy; 14uy; 15uy; 16uy; 17uy; |]
        let firstThree = read 0 3 stream
        let lastThree = read 4 3 stream
        Assert.AreEqual([| 1uy; 2uy; 3uy; |], firstThree)
        Assert.AreEqual([| 15uy; 16uy; 17uy |], lastThree)

    [<Test>]
    member this.TestReadRepeat() =
        let optionsList = readRepeat 5 ipv4OptionFromBytes 
                            [| 1uy; 3uy; 1uy; 2uy; 3uy; 1uy; 3uy; 1uy; 2uy; 3uy; |] 
                            []
        printfn "%d" optionsList.Length
        printfn "%O" optionsList
        Assert.True(optionsList.Length = 2)

    [<Test>]
    member this.TestReadRepeat2() =
        let optionsList = readRepeat2 1 1 1 ipv4OptionFromBytes 
                            [| 1uy; 3uy; 1uy; 2uy; 3uy; 1uy; 3uy; 1uy; 2uy; 3uy; |] 
                            []
        // printfn "%d" optionsList.Length
        Assert.True(optionsList.Length = 2)
