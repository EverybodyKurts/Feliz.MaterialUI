module Samples.Usage.Hooks.UseMediaQuery

open Elmish
open Feliz
open Feliz.MaterialUI


type private Props = { key: string }


[<ReactComponent>]
let UseMediaQuery (_: string) : ReactElement =
  let isDarkMode = Hooks.useMediaQuery "@media (prefers-color-scheme: dark)"
  Mui.typography [
    typography.children ("System dark mode is currently " + if isDarkMode then "enabled" else "disabled")
  ]