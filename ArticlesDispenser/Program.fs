open PocketApi
open JSONParser
open ArticleFilters
open ArticlesDispenser
open Article

let consumerKey = "TODO: Add consumer key generating"
let accessKey = "TODO: Add access key generating"

let showArticle (article : Article) = 
    System.Diagnostics.Process.Start("http://getpocket.com/a/read/" + article.Id.ToString())

[<EntryPoint>]
let main argv = 
    let articles = Async.RunSynchronously(getItemsContent parseArticles consumerKey accessKey)
    
    let userSelectedTags = argv

    let filter = filter [| isArticleWithAllTags userSelectedTags |]
    
    let nextArticle = articles |> getNextArticle filter randomArticleStrategy
    
    showArticle nextArticle

    0
