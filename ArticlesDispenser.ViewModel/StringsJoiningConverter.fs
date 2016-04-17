namespace ArticlesDispenser.ViewModel

open System
open System.Windows.Data
open System.Globalization

type StringsJoiningConverter() = 
    let join (items : seq<string>) = String.Join("; ", items)
    interface IValueConverter with
        member x.Convert(value : obj, targetType : Type, parameter : obj, culture : CultureInfo) : obj = 
            value :?> seq<string> |> join :> obj
        member x.ConvertBack(value : obj, targetType : Type, parameter : obj, culture : CultureInfo) : obj = 
            failwith "Not implemented yet"
