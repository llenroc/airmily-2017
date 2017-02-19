namespace airmily.Services.ModelsAppService
{
    public class TodoItem : EntityDataOs, ITodoItem
    {
        public string Text { get; set; }

        public bool Complete { get; set; }
    }
}