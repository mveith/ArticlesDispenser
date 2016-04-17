namespace ArticlesDispenser.ViewModel

open ArticlesDispenser
open Article
open System.Diagnostics

type Dispensing(articles : seq<Article>) = 
    let articleUrlPattern = "http://getpocket.com/a/read/"
    let getArticleUrl (article : Article) = articleUrlPattern + article.Id.ToString()
    
    let openArticle (article : option<Article>) = 
        match article with
        | option.Some article -> 
            article
            |> getArticleUrl
            |> Process.Start
            |> ignore
        | option.None -> ()
    
    let openWithStrategy strategy = 
        selectNextArticle strategy articles
        |> option.Some
        |> openArticle
    
    member this.OpenArticle = openArticle
    member val OpenShortestArticleCommand = new DelegateCommand(fun x -> openWithStrategy shortestArticleStrategy)
    member val OpenLongestArticleCommand = new DelegateCommand(fun x -> openWithStrategy longestArticleStrategy)
    member val OpenOldestArticleCommand = new DelegateCommand(fun x -> openWithStrategy oldestArticleStrategy)
    member val OpenNewestArticleCommand = new DelegateCommand(fun x -> openWithStrategy newestArticleStrategy)
    member val OpenRandomArticleCommand = new DelegateCommand(fun x -> openWithStrategy randomArticleStrategy)
