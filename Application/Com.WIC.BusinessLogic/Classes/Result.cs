namespace Com.WIC.BusinessLogic.Classes
{
    public class Result<T>
    {
        public ResultStatusEnum Status { get; private set; }
        public T Data { get; private set; }
        public Result(T d, ResultStatusEnum s)
        {
            Status = s;
            Data = d;
        }
    }
}
