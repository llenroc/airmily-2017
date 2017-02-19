namespace airmily.Services.ModelsAppService
{
    public interface ITodoItem
    {
        string Text { get; set; }

        bool Complete { get; set; }
    }
}