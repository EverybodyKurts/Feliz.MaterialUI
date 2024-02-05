module Main

open Fable.Core.JsInterop

importAll "../sass/main.sass"
importAll "../react-highlight-vs2019-custom.css"

open Elmish
open Elmish.React
open Elmish.Debug
open Elmish.HMR
open Feliz

open App

// App
// Program.mkSimple App.init App.update App.view
// #if DEBUG
// |> Program.withDebugger
// #endif
// |> Program.withReactSynchronous "feliz-app"
// |> Program.run

open Browser.Dom

let dom = document.getElementById "feliz-app"

ReactDOM.createRoot(dom).render(App())