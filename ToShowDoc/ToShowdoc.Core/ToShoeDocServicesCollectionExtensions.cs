using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ToShowDoc.Core.ShowDoc;

namespace ToShowDoc.Core
{
    public static class ToShoeDocServicesCollectionExtensions
    {
        public static IServiceCollection AddShowDocCore(this IServiceCollection services)
        {
            services.AddSingleton<IShowDocDataProvider, JsonShowDocDataProvider>();
            services.AddSingleton<IShowDocStore, DefaultShowDocStore>();

            return services;
        }
    }
}
