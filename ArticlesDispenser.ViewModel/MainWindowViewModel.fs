namespace ArticlesDispenser.ViewModel

open PocketApi
open PocketApiAuthorization
open JSONParser
open ArticlesDispenser
open Article
open System
open System.Collections.ObjectModel
open System.Collections.Generic

type MainWindowViewModel() = 
    inherit ViewModelBase()
    let consumerKey = System.IO.File.ReadAllText("ConsumerKey.txt")
    let mutable accessKey = String.Empty
    let articles = new ObservableCollection<Article>()
    let allLoadedArticles = new List<Article>()
    let mutable selectedArticle : option<Article> = option.None
    let mutable requestTokenCode = String.Empty
    let mutable status = String.Empty
    let filtering = Filtering()
    let dispensing = Dispensing(articles)
    
    let showArticles loadedArticles = 
        articles.Clear()
        for article in loadedArticles do
            articles.Add article
    
    let updateArticles() = 
        let filteredArticles = filtering.FilterArticles allLoadedArticles
        showArticles filteredArticles
    
    let processLoadedArticles loadedArticles = 
        allLoadedArticles.Clear()
        for article in loadedArticles do
            allLoadedArticles.Add article
        updateArticles()
    
    let filter() = 
        filtering.Filter()
        updateArticles()
    
    let requestAuthorization() = 
        let requestTokenCode = getRequestTokenCode consumerKey
        let authorizeUrl = getAuthorizeUrl consumerKey requestTokenCode
        System.Diagnostics.Process.Start(authorizeUrl)
        requestTokenCode
    
    member this.SelectedArticle 
        with get () = selectedArticle
        and set (v) = selectedArticle <- v
    
    member this.Status 
        with get () = status
        and set (v) = 
            status <- v
            this.OnPropertyChanged(<@ this.Status @>)
    
    member val Articles = articles with get, set
    
    member this.LoadArticles() = 
        this.Status <- "Začínám stahovat články"
        let guiContext = System.Threading.SynchronizationContext.Current
        async { 
            let! loadedArticles = getItemsContent parseArticles consumerKey accessKey
            guiContext.Post((fun _ -> processLoadedArticles (loadedArticles)), null)
            this.Status <- "Články byly úspěšně staženy"
        }
    
    member this.Login() = 
        if (requestTokenCode = String.Empty) then 
            requestTokenCode <- requestAuthorization()
            this.Status <- "Nejprve je nutné se přihlásit v prohlížeči a potom to zkusit znovu"
            ()
        else 
            let (userName, accessToken) = authorize consumerKey requestTokenCode
            this.Status <- sprintf "Uživatel %s byl úspěšně přihlášen" userName
            accessKey <- accessToken
    
    member this.DownloadCommand = new AsyncDelegateCommand(fun x -> this.LoadArticles())
    member val OpenArticleCommand = new DelegateCommand(fun x -> dispensing.OpenArticle selectedArticle)
    member val FilterCommand = new DelegateCommand(fun x -> filter())
    member this.LoginCommand = new DelegateCommand(fun x -> this.Login())
    member this.Filtering = filtering
    member this.Dispensing = dispensing
