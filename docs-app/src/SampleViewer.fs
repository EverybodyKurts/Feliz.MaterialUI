﻿module SampleViewer

open Elmish
open Fable.Core
open Fable.Core.JsInterop
open Fable.MaterialUI.MaterialDesignIcons
open Fable.MaterialUI.Icons
open Fable.SimpleHttp
open Feliz
open Feliz.UseElmish
open Feliz.Markdown
open Feliz.MaterialUI


let samples =
  Map.ofList [
    "Samples.Usage.Localization.Localization", Samples.Usage.Localization.Localization.Localization
    "Samples.Usage.Hooks.UseMediaQuery", Samples.Usage.Hooks.UseMediaQuery.UseMediaQuery
    "Samples.Components.Autocomplete.Autocomplete", Samples.Components.Autocomplete.Autocomplete.Autocomplete
    "Samples.Samples.SignIn.SignIn", Samples.Samples.SignIn.SignIn.SignIn
  ]


type Sample = ReactElement
type CodeBlock = string
type Path = string list
type Url = string
type ErrorMessage = string
type StatusCode = int


type State =
  | Initial
  | Loading
  | Loaded of Result<(unit -> Sample) * CodeBlock * Path, ErrorMessage>


type Msg =
  | StartLoad of path: Path
  | LoadCompleted of Result<(unit -> Sample) * CodeBlock * Path, ErrorMessage>


let init =
  Initial, Cmd.ofMsg (StartLoad [])


let update (msg: Msg) (state: State) =
  match msg with
  | StartLoad path ->
      let url = path |> String.concat "/"
      let loadSource () =
        async {
          let! statusCode, responseText = Http.get url

          if statusCode = 200 then
            let codeBlock = sprintf "```fsharp\n%s\n```" responseText
            let sampleName = responseText.Split('\n').[0].Trim().Substring(7)
            let sample =
              samples.TryFind sampleName
              |> Option.defaultWith (fun () _ ->
                  Mui.typography [
                    typography.color.error
                    typography.paragraph true
                    typography.children (sprintf "Sample not found: '%s'" sampleName)
                  ]
              )
            return Ok (sample, codeBlock, path)
          else
            return Error (sprintf "Failed with status %i when loading %s" statusCode url)
        }
      Loading, Cmd.OfAsync.perform loadSource () (fun res -> LoadCompleted res)

  | LoadCompleted res ->
      Loaded res, Cmd.none


type DemoProps = {
  GetSample: unit -> Sample
  MarkdownCodeBlock: string
  Path: string list
}

let useDemoStyles = Styles.makeStyles(fun styles theme ->
  let bgColor =
    if theme.palette.``type`` = PaletteType.Light
    then theme.palette.grey.``100``
    else theme.palette.grey.A400
  {|
    demoPaper = styles.create [
      style.padding (theme.spacing 3)
      style.backgroundColor bgColor
    ]
    codePanel = styles.create [
      style.backgroundColor bgColor
    ]
    resetSampleButton = styles.create [
      style.floatStyle.right
      style.marginTop (-theme.spacing 2)
      style.marginRight (-theme.spacing 2)
    ]
  |}
)

let Demo = React.functionComponent(fun (props: DemoProps) ->
  let c = useDemoStyles ()
  let isExpanded, setIsExpanded = React.useState false
  let sampleKey, setSampleKey = React.useState 0
  let sample =
    React.useMemo(
      (fun () -> props.GetSample (string sampleKey)),
      [|string sampleKey|]
    )
  Html.div [

    // Sample
    Mui.paper [
      paper.classes.root c.demoPaper
      prop.children [
        Mui.tooltip [
          tooltip.title ("Reset sample")
          tooltip.children(
            Mui.iconButton [
              iconButton.classes.root c.resetSampleButton
              // button.children (undoIcon [])
              prop.onClick (fun _ -> setSampleKey (sampleKey + 1))
            ]
          )
        ]
        sample
      ]
      paper.elevation 0
    ]

    Mui.grid [
      grid.container true
      grid.direction.row
      grid.justify.flexEnd
      grid.children [
        // GitHub button
        Mui.tooltip [
          tooltip.title (if isExpanded then "Hide code" else "Show code")
          tooltip.children(
            Mui.iconButton [
              prop.onClick (fun _ -> setIsExpanded (not isExpanded))
              iconButton.color.inherit'
              // iconButton.children (codeIcon [])
            ]
          )
        ]

        // GitHub button
        Mui.tooltip [
          tooltip.title "View the source on GitHub"
          tooltip.children(
            Mui.iconButton [
              prop.href (sprintf "https://github.com/Shmew/Feliz.MaterialUI/tree/master/docs-app/public/%s" (String.concat "/" props.Path))
              iconButton.component' "a"
              iconButton.color.inherit'
              // iconButton.children (gitHubIcon [])
            ]
          )
        ]
      ]
    ]

    // Code
    Mui.collapse [
      collapse.in' isExpanded
      prop.style [
        style.display.block
      ]
      collapse.children [
        Markdown.markdown [
          prop.className c.codePanel
          markdown.children props.MarkdownCodeBlock
          markdown.escapeHtml false
          markdown.components [
            markdown.components.code (fun props ->
              CommonViews.code (props.language, props.value)
            )
          ]
        ]
      ]
    ]


    //Mui.expansionPanel [
    //  prop.className c.codePanel
    //  expansionPanel.expanded isExpanded
    //  expansionPanel.onChange setIsExpanded
    //  expansionPanel.children [
    //    Mui.expansionPanelSummary [
    //      expansionPanelSummary.expandIcon (expandMoreIcon [])
    //      expansionPanelSummary.children [
    //        Mui.typography (if isExpanded then "Hide code" else "Show code")


    //      ]
    //    ]
    //    Mui.expansionPanelDetails [


    //    ]
    //  ]
    //]

  ]
)


[<ReactComponent>]
let SampleLoader () =
  let state , dispatch = React.useElmish(init, update, [| |])
  match state with
  | Initial ->
      Html.none
  | Loading ->
      React.fragment [
        Mui.skeleton [
          skeleton.variant.rect
          skeleton.height 400
        ]
      ]
  | Loaded (Ok (getSample, codeBlock, path)) ->
      Demo({GetSample = getSample; MarkdownCodeBlock = codeBlock; Path = path})
  | Loaded (Error errorMsg) ->
      Mui.typography [
        typography.color.error
        typography.paragraph true
        typography.children errorMsg
      ]


// let sampleViewer path =
//   React.elmishComponent("SampleLoader", init path, update, render)
