module CommonViews

open Feliz
open Feliz.MaterialUI
open Feliz.ReactHighlight

[<ReactComponent>]
let code (language: string, code: ReactElement array) : ReactElement =
  Mui.paper [
    paper.elevation 0
    paper.children [
      Highlight.highlight [
        prop.className (if language = "f#" then "fsharp" else language)
        // prop.text code
        prop.children code
      ]
    ]
  ]
