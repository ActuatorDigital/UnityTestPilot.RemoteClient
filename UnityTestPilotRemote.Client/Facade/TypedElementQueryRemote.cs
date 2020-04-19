// Copyright (c) AIR Pty Ltd. All rights reserved.

using System;
using AIR.UnityTestPilot.Interactions;
using AIR.UnityTestPilotRemote.Client;
using AIR.UnityTestPilotRemote.Common;

namespace AIR.UnityTestPilot.Queries
{
    public class TypedElementQueryRemote : TypedElementQuery, IProxyQuery
    {
        private UnityDriverHostProcess _process;

        public TypedElementQueryRemote(Type type, string name)
            : base(type, name) { }

        public TypedElementQueryRemote(Type type)
            : base(type) { }

        public void ProxyVia(UnityDriverHostProcess process) 
            => _process = process;

        public override UiElement[] Search()
        {
            
            var remoteElementQuery = new RemoteElementQuery(
                QueryFormat.TypedQuery,
                _queryName,
                _queryType.AssemblyQualifiedName
            );

            var remoteElementQueryTask = _process
                .Query(remoteElementQuery);

            var result = remoteElementQueryTask.Result;

            var resultElements = new[] {
                new UiElementRemote(result, _process) };

            return resultElements;
        }
        
    }
}