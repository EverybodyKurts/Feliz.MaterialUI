module Samples.Components.Autocomplete.Autocomplete

open System

open Elmish
open Feliz
open Feliz.UseElmish
open Feliz.MaterialUI


type Country =
  { Id: Guid
    Name: string
    Description: string }


type Model =
  { Countries: Country list
    MyText: string
    LastSelected: Guid option }


type Msg =
  | SetText of string
  | Select of Guid option


let init () : Model * Cmd<Msg> =
  { Countries =
      ["Afghanistan"; "Aland Islands"; "Albania"; "Algeria"; "American Samoa"; "Andorra"; "Angola"; "Anguilla"; "Antarctica"; "Antigua and Barbuda"; "Argentina"; "Armenia"; "Aruba"; "Australia"; "Austria"; "Azerbaijan"]
      |> List.map (fun s ->
        { Id = Guid.NewGuid(); Name = s; Description = sprintf "Description of %s" s }
      )
    MyText = ""
    LastSelected = None }, Cmd.none


let update (msg: Msg) (m: Model) : Model * Cmd<Msg> =
  match msg with
  | SetText s -> { m with MyText = s }, Cmd.none
  | Select id -> { m with LastSelected = id }, Cmd.none


[<ReactComponent>]
let Autocomplete (_: string) : ReactElement =
  let model, dispatch = React.useElmish(init, update, [| |])
  Html.div [
    Mui.autocomplete [
      autocomplete.options (model.Countries |> List.toArray)
      autocomplete.getOptionLabel (fun c -> c.Name)
      autocomplete.freeSolo true
      autocomplete.renderInput (fun props ->
        Mui.textField [
          yield! props.felizProps
          textField.value model.MyText
          textField.onChange (SetText >> dispatch)
          textField.fullWidth true
          prop.style [
            style.width 300
          ]
        ]
      )
      autocomplete.value (
        model.LastSelected
        |> Option.bind (fun cid -> model.Countries |> List.tryFind (fun c -> c.Id = cid))
      )
      autocomplete.onChange (fun c -> c.Id |> Some |> Select |> dispatch)
    ]
    Mui.typography [
      Html.text "Last selected: "
      Html.text (
        model.LastSelected
        |> Option.bind (fun cid -> 
            model.Countries
            |> List.tryPick (fun c -> if c.Id = cid then Some c.Name else None)
        )
        |> Option.defaultValue "None"
      )
    ]
  ]