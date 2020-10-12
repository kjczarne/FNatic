module UdpDatagram
open System

type UdpDatagram = {
    SrcPort: uint16
    DstPort: uint16
    Length: uint16
    Checksum: uint16
    Body: byte [] }

let myDatagram = {
    SrcPort = uint16 3
    DstPort = uint16 2
    Length = uint16 1
    Checksum = uint16 1
    Body = Array.zeroCreate 10 }

let makeUdpDatagram
    srcPort dstPort checksum (body : byte []) =
        { SrcPort = srcPort
          DstPort = dstPort
          Length = uint16 body.Length
          Checksum = checksum
          Body = body }
