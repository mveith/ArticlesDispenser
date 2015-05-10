module Article

type Article(id : int, title : string, summary : string, length : int, tags : string [], addedDateTime : System.DateTime) = 
    member this.Id = id
    member this.Title = title
    member this.Summary = summary
    member this.Length = length
    member this.Tags = tags
    member this.AddedDateTime = addedDateTime
