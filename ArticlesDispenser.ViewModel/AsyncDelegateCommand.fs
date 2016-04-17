namespace ArticlesDispenser.ViewModel

open System.Windows.Input

type AsyncDelegateCommand(action : 'a -> Async<unit>) = 
    let CanExecuteChangedEvent = new Event<_, _>()
    interface System.Windows.Input.ICommand with
        member this.CanExecute(parameter : obj) : bool = true
        
        [<CLIEvent>]
        member this.CanExecuteChanged = CanExecuteChangedEvent.Publish
        
        member this.Execute(parameter) = Async.Start(action parameter)
