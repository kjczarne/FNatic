// Learn more about F# at http://fsharp.org
module Program
open System
open Ipv4
open UdpDatagram
open EthernetFrame

[<EntryPoint>]
let main argv =
    // printfn "%b" (parser.MoveNext())
    // printfn "%s" (parser.Current.ToString())
    // printfn "Hello World from F#!"
    let udp = makeUdpDatagram 1234us 1234us 0us  // partially applied func without body
    let ipv4 = makeIpv4 0uy 0uy 1us 1us 2uy 17uy // TODO: protcol body enum
                        0us 
                        (Array.map byte [| 192; 168; 10; 1 |])
                        (Array.map byte [| 192; 168; 11; 2 |])
                        [] // empty options list
    let ether = makeEthernetFrame (Array.map byte [| 0xff; 0xff; 0xff; 0xff; 0xff; 0xf0 |])
                                  (Array.map byte [| 0xff; 0xff; 0xff; 0xff; 0xff; 0xfe |])
                                  80us

    // All above functions are partially applied and missing only the Body part.
    // Effectively any child type can be singled out through partial application.

    // Putting these together:

    let fullEther = udp (Array.map byte [| 0x01; 0x02; 0x03 |])
                    |> ipv4
                    |> ether

    let oneOption = makeIpv4Option 1uy 3uy [| 1uy; 2uy; 3uy |]

    let optionsList = ipv4OptionsFromBytes [| 1uy; 3uy; 1uy; 2uy; 3uy; 1uy; 3uy; 1uy; 2uy; 3uy; |]

    // printfn "%O" oneOption


    0 // return an integer exit code