// Copyright (c) AIR Pty Ltd. All rights reserved.

using AIR.UnityTestPilotRemote.Client;

namespace AIR.UnityTestPilot.Queries
{
    public static class By
    {
        public static ElementQuery Name(string name) =>
            ByBase.Name<NamedElementProxyQueryRemote>(name);

        public static ElementQuery Type<TQueryType>(string name) =>
            ByBase.Type<TQueryType, TypedElementQueryRemote>(name);

        public static ElementQuery Type<TQueryType>() =>
            ByBase.Type<TQueryType, TypedElementQueryRemote>();
    }
}