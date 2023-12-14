open System
let getFirstLetter (text: string) =
    text.ToCharArray() |> Array.head |> Char.ToUpper |> string 

let toInitials (name: string) =
    name.Split([|' '|])
    |> Array.map (fun s -> getFirstLetter s + ".")
    |> String.concat " "



let isOdd (str: string) = (str.Length % 2 = 0)
let getMiddle (str : string) =
  let md = str.Length / 2
  let ml = md - 1
  match (isOdd str) with
  | true -> sprintf  "%c%c" str.[ml] str.[md]
  | false -> sprintf  "%c" str.[md]
    
