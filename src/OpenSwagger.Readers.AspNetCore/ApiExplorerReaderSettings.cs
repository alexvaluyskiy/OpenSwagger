using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSwagger.Readers.AspNetCore
{
    public class ApiExplorerReaderSettings
    {
        public ApiExplorerReaderSettings()
        {
            OperationFilters = new List<IOperationFilter>();
            DocumentFilters = new List<IDocumentFilter>();
        }

        public IList<IOperationFilter> OperationFilters { get; private set; }

        public IList<IDocumentFilter> DocumentFilters { get; private set; }

    }
}
