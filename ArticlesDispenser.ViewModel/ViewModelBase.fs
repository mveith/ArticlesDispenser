namespace ArticlesDispenser.ViewModel

open System
open System.ComponentModel
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns

type ViewModelBase() = 
    let propertyChanged = new Event<_, _>()
    
    let toPropName (query : Expr) = 
        match query with
        | PropertyGet(a, b, list) -> b.Name
        | _ -> ""
    
    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member x.PropertyChanged = propertyChanged.Publish
    
    abstract OnPropertyChanged : string -> unit
    override x.OnPropertyChanged(propertyName : string) = 
        propertyChanged.Trigger(x, new PropertyChangedEventArgs(propertyName))
    member x.OnPropertyChanged(expr : Expr) = 
        let propName = toPropName (expr)
        x.OnPropertyChanged(propName)
