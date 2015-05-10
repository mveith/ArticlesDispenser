module ArticleFilters

open Article

let isArticleWithAllTags tags (article : Article) = 
    tags |> Seq.forall (fun t -> article.Tags |> Seq.exists (fun at -> at = t))
let isArticleWithoutTags tags (article : Article) = 
    not (tags |> Seq.exists (fun t -> article.Tags |> Seq.exists (fun at -> at = t)))
let isArticleWithMaxLength legth (article : Article) = article.Length <= legth
let isForFilters filters article = filters |> Seq.forall (fun f -> f article)

let filter filters articles = 
    let isForFilters = isForFilters filters
    articles |> Seq.filter isForFilters
