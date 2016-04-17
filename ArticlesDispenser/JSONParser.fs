module JSONParser

open FSharp.Data
open FSharp.Data.JsonExtensions
open System
open Article

let convertToDate secondsCount = 
    let baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    let utcDate = secondsCount |> baseDate.AddSeconds
    utcDate.ToLocalTime()

let getValues (parentValue : JsonValue) = parentValue.Properties |> Seq.map (fun (prop, value) -> value)

let parseTags (tags : option<JsonValue>) = 
    match tags with
    | option.None -> [| "_untagged_" |]
    | option.Some value -> 
        value
        |> getValues
        |> Seq.map (fun v -> (v?tag).AsString())
        |> Seq.toArray

let isResolvedArticle (value : JsonValue) = 
    let isResolved = (value?resolved_id).AsInteger() > 0
    isResolved

let createArticle (value : JsonValue) = 
    let id = (value?item_id).AsInteger()
    let isResolvedArticle = isResolvedArticle value
    
    let title = 
        if isResolvedArticle then (value?resolved_title).AsString()
        else (value?given_url).AsString()
    
    let summary = 
        if isResolvedArticle then (value?excerpt).AsString()
        else String.Empty
    
    let length = 
        if isResolvedArticle then (value?word_count).AsInteger()
        else 0
    
    let tags = parseTags (value.TryGetProperty("tags"))
    let date = convertToDate ((value?time_added).AsFloat())
    new Article(id, title, summary, length, tags, date)

let parseArticles (json : string) = 
    let root = JsonValue.Parse(json)
    root?list
    |> getValues
    |> Seq.map createArticle
