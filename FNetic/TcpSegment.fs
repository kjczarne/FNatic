module TcpSegment

type TcpSegment = {
    SrcPort: uint16
    DstPort: uint16
    SeqNum: uint32
    AckNum: byte
    B12: byte
    B13: byte
    Length: uint16  // TODO: no length on normal TCP segment?
    WindowSize: uint16
    Checksum: uint16
    UrgentPointer: uint16
    Body: byte []
}

let makeTcpSegment 
    srcPort dstPort seqNum ackNum 
    b12 b13 windowSize checksum urgentPointer (body : byte []) =
        { SrcPort = srcPort
          DstPort = dstPort
          SeqNum = seqNum
          AckNum = ackNum
          B12 = b12
          B13 = b13
          Length = uint16 body.Length
          WindowSize = windowSize
          Checksum = checksum
          UrgentPointer = urgentPointer
          Body = body }