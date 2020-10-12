module Ipv4

open Primitives
open ByteHandler
open UdpDatagram
open System

type Ipv4Option = {
    B1: byte
    Len: byte
    Body: byte[]
    Copy: bool
    OptClasses: byte
    Number: byte
}

let makeIpv4Option b1 len body =
    { B1 = b1
      Len = len
      Body = body
      Copy = if (b1 &&& 0b10000000uy <> 0uy) then true else false
      OptClasses = b1 &&& 0b01100000uy
      Number = b1 &&& 0b00011111uy }

let ipv4OptionFromBytes bytearray =
    let b1 = read 0 1 bytearray |> getByte
    let len = read 1 1 bytearray |> getByte
    let body = read 2 (len |> int) bytearray
    makeIpv4Option b1 len body

let rec ipv4OptionsFromBytes bytearray = 
    readRepeat2 1 1 1 ipv4OptionFromBytes bytearray []

type SrcIpAddr = byte[]
type DstIpAddr = byte[]

type Ipv4Packet<'a> = {
    B1: byte
    B2: byte
    TotalLength: uint16
    Identification: uint16
    B67: uint16
    Ttl: byte
    Protocol: byte
    HeaderChecksum: uint16
    SrcIpAddr: SrcIpAddr
    DstIpAddr: DstIpAddr
    Options: list<Ipv4Option>
    Version: uint32
    Ihl: uint32
    IhlBytes: uint32
    Body: 'a
}

let getIhl b1 = uint32 (b1 &&& 0xfuy)
let getIhlBytes b1 = (getIhl b1) * 4u

let makeIpv4 
    b1 b2 identification b67
    ttl protocol headerChecksum srcIpAddr dstIpAddr
    options body =
        let ihl = getIhl b1
        let ihlbytes = getIhlBytes b1
        {
            B1 = b1
            B2 = b2
            TotalLength = uint16 (20us + body.Length + uint16 ihlbytes)
            Identification = identification
            B67 = b67
            Ttl = ttl
            Protocol = protocol
            HeaderChecksum = headerChecksum
            SrcIpAddr = srcIpAddr
            DstIpAddr = dstIpAddr
            Options = options
            Version = uint32 ((b1 &&& 0xf0uy) >>> 4)
            Ihl = ihl
            IhlBytes = ihlbytes
            Body = body
        }
    // A bit of code duplication but the Struct types
    // in .NET with an implicit constructor cannot
    // contain instances of themselves as members
    // and this is a problem for types like Ipv4
    // which can recursively contain another Ipv4 packet

let ipv4PacketToBytes bodyConverter (ipv4 : Ipv4Packet<'a>) =
    seq { 
          bodyConverter ipv4.Body }
        |> Array.concat

let ipv4PacketFromBytes bodyConstructor bytearray =
    let b1 = read 0 1 bytearray |> getByte
    let totalLength = (read 2 2 bytearray |> fun x -> BitConverter.ToUInt16(x, 0))
    let optionsLength = (int (getIhlBytes b1 - 20u))
    let ret = makeIpv4 b1 
                      (read 1 1 bytearray |> getByte)
                      (read 4 2 bytearray |> fun x -> BitConverter.ToUInt16(x, 0))
                      (read 6 2 bytearray |> fun x -> BitConverter.ToUInt16(x, 0))
                      (read 8 1 bytearray |> getByte)
                      (read 9 1 bytearray |> getByte)
                      (read 10 2 bytearray |> fun x -> BitConverter.ToUInt16(x, 0))
                      (read 12 4 bytearray)
                      (read 16 4 bytearray)
                      (read 18 optionsLength bytearray |> ipv4OptionsFromBytes)
                      (Array.truncate (int (getIhlBytes b1 - 20u + 18u)) bytearray |> bodyConstructor)
    // update length with the one parsed and return the record:
    {| ret with TotalLength = totalLength |}