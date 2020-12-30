using System;

namespace BGPViewerOpenApi.Model
{
    public abstract class ApiBase
    {
        public abstract int Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Type ApiType { get; }
    }
}