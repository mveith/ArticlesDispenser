module ArticlesDispenser

open System
open Article

let random = new Random()
let randomArticleStrategy (article : Article) = random.Next()
let shortestArticleStrategy (article : Article) = article.Length
let longestArticleStrategy (article : Article) = -article.Length
let newestArticleStrategy (article : Article) = -article.AddedDateTime.Ticks
let oldestArticleStrategy (article : Article) = article.AddedDateTime

let getNextArticle filter selectedStrategy articles = 
    articles
    |> filter
    |> Seq.sortBy selectedStrategy
    |> Seq.head
