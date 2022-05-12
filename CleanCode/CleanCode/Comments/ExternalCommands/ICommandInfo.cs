namespace CleanCode.Comments.ExternalCommands
{
    public interface ICommandInfo
    {
        public string Name { get; }
        
        public string Description { get; }
       
        public string Picture { get; }
        
        public string Version { get; }
    }
}
