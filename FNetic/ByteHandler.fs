module ByteHandler
open System

// let int16ToBytes (integer : int16) =
//     BitConverter.GetBytes integer

// let intToBytes (integer : int) =
//     BitConverter.GetBytes integer

// let int64ToBytes (integer : int64) =
//     BitConverter.GetBytes integer

// let uint16ToBytes (integer : uint16) =
//     BitConverter.GetBytes integer

// let uintToBytes (integer : uint) =
//     BitConverter.GetBytes integer

// let uint64ToBytes (integer : uint64) =
//     BitConverter.GetBytes integer

// let floatToBytes  // beware, float is double in F# type system
// let float32ToBytes

let read start bytes ( bytearray : byte array ) =
    Array.skip start bytearray 
    |> Array.truncate bytes

let rec readRepeat blockSize blockParseFn ( bytearray : byte [] ) acc =
    let chunks = Array.toList <| Array.chunkBySize blockSize bytearray
    match chunks with
    | c :: rest -> readRepeat blockSize blockParseFn ( Array.concat rest ) ( acc @ ( [ blockParseFn c ] ) )
    | [] -> acc

let rec readRepeat2 offsetToLenField lenFieldSize otherFieldsSize blockParseFn ( bytearray : byte [] ) acc =
    let firstLen = read offsetToLenField lenFieldSize bytearray
    let padded = if firstLen.Length < 4 
                 then Array.replicate (4 - firstLen.Length) 0uy 
                      |> Array.append firstLen 
                 else firstLen
    let blockSize = otherFieldsSize 
                    + lenFieldSize
                    + ( padded |> fun x -> BitConverter.ToInt32(x, 0) )
    readRepeat blockSize blockParseFn bytearray acc

let getByte (bytearray : byte []) =
    bytearray.[0]


// TODO: let toBytesBigEndian