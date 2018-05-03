using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Text;
using TApiMongo.Interfaces;

namespace TApiMongo.Web.Extentions
{
    public static class IdProtector<TViewModel> where TViewModel : class, IHasID<string>
    {
        public static void Protect(IDataProtector protector, TViewModel viewModel)
        {
            viewModel.ID = protector.Protect(viewModel.ID);
        }

        public static void Protect(IDataProtector protector, List<TViewModel> viewModel)
        {
            foreach (var model in viewModel)
            {
                model.ID = protector.Protect(model.ID);
            }
        }

        public static void Unprotect(IDataProtector protector, TViewModel viewModel)
        {
            viewModel.ID = protector.Unprotect(viewModel.ID);
        }

        public static void Unprotect(IDataProtector protector, List<TViewModel> viewModel)
        {
            foreach (var model in viewModel)
            {
                model.ID = protector.Unprotect(model.ID);
            }
        }
    }
}