using CSharpFunctionalExtensions;
using TestPlanning.Common.Interfaces;
using TestPlanning.Common.Models;
using TestPlanning.Method.Context;

namespace TestPlanning.Method.Queries
{
    public class GetMethodsQuery : IQuery<MethodModel>
    {
        public string Value { get; private set; }

        public GetMethodsQuery(string value)
        {
            Value = value;
        }
    }

    public sealed class GetMethodsQueryHandler : IQueryHandler<GetMethodQuery, MethodModel>
    {
        private readonly MethodContext _context;

        public GetMethodsQueryHandler(MethodContext context)
        {
            _context = context;
        }

        public Result<MethodModel> Handle(GetMethodQuery query)
        {
            var result = _context.Methods.Find(query.Id);
            return Result.Ok(result);
        }
    }
}
