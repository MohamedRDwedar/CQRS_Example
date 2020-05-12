using CSharpFunctionalExtensions;
using System;
using TestPlanning.Common.Interfaces;
using TestPlanning.Common.Models;
using TestPlanning.Method.Context;

namespace TestPlanning.Method.Queries
{
    public class GetMethodQuery : IQuery<MethodModel>
    {
        public long Id { get; private set; }

        public GetMethodQuery()
        {

        }

        public GetMethodQuery(long id)
        {
            Id = id;
        }
    }

    public sealed class GetMethodQueryHandler : IQueryHandler<GetMethodQuery, MethodModel>
    {
        private readonly MethodContext _context;

        public GetMethodQueryHandler(MethodContext context)
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
