namespace CW1.Client.Views
open System
open Bolero.Html
open MudBlazor
open Microsoft.AspNetCore.Components.Web

module AppBar =

    let appBar model dispatch = 
        comp<MudAppBar> {
            "Fixed" => true
            "Dense" => true
            comp<MudGrid> {
                comp<MudItem> {
                    "xs" => 12
                    comp<MudText> {
                        "Align" => Align.Center
                        "Typo" => Typo.h6
                        "Contemporary Web App Toolkit Demo"
                    }
                }
            }
        }
