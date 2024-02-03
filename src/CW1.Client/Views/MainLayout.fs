namespace CW1.Client.Views
open System
open System.Threading.Tasks
open Microsoft.AspNetCore.Components
open Microsoft.AspNetCore.Components.Web
open Elmish
open Bolero
open Bolero.Html
open CW1.Client
open MudBlazor

type MainLayout() =
    inherit ElmishComponent<Model,Message>()    

    override this.View model dispatch =        
        match model.page with 

        | Page.Home -> 
            concat {
                comp<PageTitle> { text "Contemporary" }
                comp<MudThemeProvider> {attr.empty()}
                comp<MudDialogProvider> {attr.empty()}
                comp<MudSnackbarProvider> {attr.empty()}                
                comp<MudLayout> {
                    AppBar.appBar model dispatch
                    comp<MudMainContent> {
                        comp<MudPaper> {
                            "Elevation" => 0
                            "Class" => "d-flex justify-space-around"
                            comp<MudPaper> {
                                "Elevation" => 0
                                "Class" => "d-flex flex-row gap-1"
                                "Style" => "padding-top: 3rem; max-width:40rem;"
                                comp<MudNumericField<int>> {
                                    "Class" => "ma-4"
                                    "Label" => "Count from server"
                                    "Variant" => Variant.Outlined
                                    "HideSpinButtons" => true
                                    "Value" => model.count
                                }
                                comp<MudButton> {
                                    "Color" => Color.Primary
                                    "Class" => "ma-4"
                                    "Variant" => Variant.Filled
                                    on.click (fun _ -> dispatch Reset)
                                    text "Reset Counter"
                                }
                           }
                       }
                    }         
                }
            }
