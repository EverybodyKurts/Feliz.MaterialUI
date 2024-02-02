# Feliz.MaterialUI

Hopefully, the new home for [Feliz](https://github.com/Zaid-Ajaj/Feliz)-style Fable bindings for [Material-UI](https://material-ui.com/).


## Goals

### 1. Create a Github codespace

Make it as easy as possible for developers to collaborate and contribute to the project. The dependencies and the most commonly used tools are already installed. The project can be readily build and packaged. Of course. This entails dockerizing the project, most likely using a devcontainer as the base image.

### 2. Publish documentation to github pages via github actions

Automating publication hopefully means that the project can be readily built and up to date. In fact, automating as much as possible to lighten maintenance for regular contributors is an overall goal.

### 3. Build project via `npm` scripts

Currently, `fake` is holding the project back from being upgraded (it requires `net6` and they no plans to update that dependency). It's time to take it out and substitute it with a more lightweight process.

## Contributing

This project uses `fake`, `paket`, and `femto` as .NET Core 3 local tools. Therefore, run `dotnet tool restore` to restore the necessary CLI tools before doing anything else.

To run targets using Fake: `dotnet fake build -t TargetName`

### Regular maintenance

1. Run the `RegularMaintenance` target, which will update all packages as well as the Femto metadata in `Feliz.MaterialUI.fsproj`, and regenerate the bindings based on the live MUI docs
2. Check all changes to Feliz.MaterialUI and adjust the generator’s API parser as needed. Remember to check all doc comment updates too, since changes there may indicate that something must be changed elsewhere. Run the `RegenerateFromCache` target (or run the generator project in VS) to re-generate based on the recently downloaded HTML pages.
3. Update the version number in `Feliz.MaterialUI.fsproj`
4. Update `RELEASE_NOTES.md`
5. Update relevant docs/samples (typically in `docs-app/public/pages`, potentially also by adding menu items in `App.fs`)
6. Run the `CiBuild` target to check that everything compiles
7. Run the `Docs:Run` target to verify that the docs are still working
8. Commit and tag the commit (this is what triggers deployment from  AppVeyor). For consistency, the tag should be identical to the version (e.g. `1.2.3`).
9. Push the changes and the tag to the repo. If AppVeyor build succeeds, the package is automatically published to NuGet.
10. Publish the docs by running the `Docs:Publish` target

