namespace buildingBlock.Exceptions
{
    public class InternalServerException : Exception
    {
        public InternalServerException(string err): base(err){}
    }
}
