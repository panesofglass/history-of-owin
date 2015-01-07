// --------------------------------------------------------------------------------------
// FAKE build script 
// --------------------------------------------------------------------------------------

#I "packages/FAKE/tools"
#r "FakeLib.dll"
open System
open System.IO
open Fake 
open Fake.Git

// Git configuration (used for publishing documentation in gh-pages branch)
// The profile where the project is posted 
let gitHome = "https://github.com/panesofglass"
// The name of the project on GitHub
let gitName = "history-of-owin"

// --------------------------------------------------------------------------------------
// Clean build results & restore NuGet packages

Target "Clean" (fun _ ->
    CleanDirs ["temp";"output"]
)

// --------------------------------------------------------------------------------------
// Generate the documentation

Target "Generate" (fun _ ->
    if not <| executeFSIWithArgs "" "generate.fsx" [] [] then
        failwith "generating slides failed"
)

// --------------------------------------------------------------------------------------
// Release Scripts

Target "Publish" (fun _ ->
    let tempDocsDir = "temp/gh-pages"
    CleanDir tempDocsDir
    Repository.cloneSingleBranch "" (gitHome + "/" + gitName + ".git") "gh-pages" tempDocsDir

    CopyRecursive "output" tempDocsDir true |> tracefn "%A"
    StageAll tempDocsDir
    Commit tempDocsDir "Update slides"
    Branches.push tempDocsDir
)

// --------------------------------------------------------------------------------------
// Run all targets by default. Invoke 'build <Target>' to override

Target "All" DoNothing

"Clean"
==> "Generate"
==> "Publish"
==> "All"

RunTargetOrDefault "All"
