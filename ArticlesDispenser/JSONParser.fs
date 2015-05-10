module JSONParser

open System
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open Article

let getItemProperty (propertyName : string) (itemToken : JToken) = 
    itemToken.SelectToken(propertyName).ToString()

let getArticleTags (itemToken : JToken) = 
    let tagsToken = itemToken.SelectToken("tags")
    match tagsToken with
    | null -> [| "_untagged_" |]
    | _ -> 
        tagsToken
        |> Seq.collect (fun x -> x |> Seq.map (fun i -> i.SelectToken("tag").ToString()))
        |> Seq.toArray

let getArticleAddedDate (itemToken:JToken)=
    let baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    let dateToken = getItemProperty "time_added" itemToken
    let utcDate = dateToken |> float |> baseDate.AddSeconds
    utcDate.ToLocalTime()

let parseItem (itemToken : JToken) = 
    new Article(getItemProperty "item_id" itemToken |> Int32.Parse, 
                getItemProperty "resolved_title" itemToken, 
                getItemProperty "excerpt" itemToken, 
                getItemProperty "word_count" itemToken |> Int32.Parse, 
                getArticleTags itemToken,
                getArticleAddedDate itemToken)

let parseArticles (json : string) = 
    let articlesJson = JObject.Parse(json).SelectToken("list").ToString()
    
    let articles = 
        JsonConvert.DeserializeObject<JObject>(articlesJson)
        |> Seq.map (fun x -> x.First)
        |> Seq.map parseItem
        |> Seq.toArray
    articles