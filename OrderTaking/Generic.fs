namespace OrderTaking.Generic

open System


type Undefined = exn

type NonEmptyList<'a> = { Head: 'a; Tail: 'a list }

module NonEmptyList =
    let create head tail = { Head = head; Tail = tail } |> Ok

    let ofList list =
        match list with
        | [] -> Error "List must not be empty"
        | head :: tail -> create head tail

    let toList nel = nel.Head :: nel.Tail

type Command<'data> =
    { Data : 'data
      TimeStamp : DateTime
      UserId : string }
