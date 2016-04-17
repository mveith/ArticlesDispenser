namespace ArticlesDispenser.ViewModel

open ArticleFilters
open System
open System.Collections.Generic

type Filtering() = 
    let defaultFilter = fun a -> true
    let filters = [| defaultFilter; defaultFilter; defaultFilter |]
    let mutable selectedTags = ""
    let mutable forbiddenTags = ""
    let mutable maxLength : option<int> = option.None
    let filters = [| defaultFilter; defaultFilter; defaultFilter |]
    let parseString (input : string) = input.Split([| ';' |], System.StringSplitOptions.RemoveEmptyEntries)
    
    let createTagsFilter tagsValue filter = 
        let tags = parseString tagsValue |> Seq.map (fun t -> t.Trim())
        filter tags
    
    let convertStringToInt input = 
        let couldParse, result = System.Int32.TryParse input
        match couldParse with
        | true -> option.Some result
        | false -> option.None
    
    let convertToString input = 
        match input with
        | option.Some value -> value.ToString()
        | option.None -> ""
    
    member x.FilterArticles allArticles = 
        let filteredArticles = 
            allArticles
            |> filter filters
            |> Seq.toArray
        filteredArticles
    
    member x.Filter() = 
        filters.[0] <- createTagsFilter selectedTags isArticleWithAllTags
        filters.[1] <- createTagsFilter forbiddenTags isArticleWithoutTags
        filters.[2] <- if Option.isSome maxLength then isArticleWithMaxLength (Option.get maxLength)
                       else defaultFilter
        ()
    
    member this.SelectedTags 
        with get () = selectedTags
        and set (v) = selectedTags <- v
    
    member this.ForbiddenTags 
        with get () = forbiddenTags
        and set (v) = forbiddenTags <- v
    
    member this.MaxLength 
        with get () = convertToString maxLength
        and set (v) = maxLength <- convertStringToInt v
