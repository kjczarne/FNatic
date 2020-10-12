module EthernetFrame

open Primitives
open System
open ByteHandler

type EtherTypeEnum =
    | Ipv4 = 0x0800
    | X75Internet = 0x0801
    | NbsInternet = 0x0802
    | EcmaInternet = 0x0803
    | Chaosnet = 0x0804
    | X25Level3 = 0x0805
    | Arp = 0x0806
    | IEEE8021QTpid = 0x8100
    | Ipv6 = 0x86dd

type TagControlInfo = {
    Priority: Bit
    DropEligible: bool
    VlanId: Bit
}

type WithTci = {
    TciByte: byte
    Tci: TagControlInfo
    EtherType2: uint16
}

type EtherPayload<'a> =
    | Body of 'a

type EthernetFrameBase<'a> = {
    DstMac: byte[]
    SrcMac: byte[]
    EtherType1: uint16
    Body: 'a
}

let makeEthernetFrame dstMac srcMac etherType1 body =
    { DstMac = dstMac
      SrcMac = srcMac
      EtherType1 = etherType1
      Body = body }

let ethernetFrameToBytes bodyConverter (ether : EthernetFrameBase<'a>) =
    seq { ether.DstMac
          ether.SrcMac
          BitConverter.GetBytes ether.EtherType1
          bodyConverter ether.Body }
        |> Array.concat

let ethernetFrameFromBytes bodyConstructor bytearray =
    { DstMac = read 0 6 bytearray
      SrcMac = read 6 6 bytearray
      EtherType1 = read 12 2 bytearray |> fun x -> BitConverter.ToUInt16(x, 0)
      Body = Array.truncate 14 bytearray |> bodyConstructor }

// we could represent the above as a seq of seq
// [ [dstMac]; [srcMac]; ... ]