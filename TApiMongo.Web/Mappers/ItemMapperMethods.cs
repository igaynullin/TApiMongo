using System;
using System.Collections.Generic;
using System.Text;
using TApiMongo.Data.Entities;
using TApiMongo.Web.ViewModels.Items;

namespace TApiMongo.Web.Mappers
{
    public class ItemMapperMethods
    {
        public static List<List> MapEntityToListItemViewModel(IEnumerable<Item> entites)
        {
            var result = new List<List>();
            foreach (var entite in entites)
            {
                var temp = new List();
                temp.ID = entite.ID.ToString();
                temp.Description = entite.Description;
                if (entite.Tags != null && entite.Tags.Count > 0)
                {
                    temp.Tags = new List<string>(entite.Tags);
                }
                result.Add(temp);
            }
            return result;
        }

        public static Get MapEntityToGetViewModel(Item entity)
        {
            var result = new Get();
            result.ID = entity.ID.ToString();
            result.Description = entity.Description;
            if (entity.Tags != null && entity.Tags.Count > 0)
            {
                result.Tags = new List<string>(entity.Tags);
            }

            return result;
        }

        public static Item MapViewModelToItemEntity(Get viewModel)
        {
            Item result = new Item();

            result.ID = Convert.ToInt32(viewModel.ID);
            result.Description = viewModel.Description;
            if (viewModel.Tags != null && viewModel.Tags.Count > 0)
            {
                result.Tags = new List<string>(viewModel.Tags);
            }
            return result;
        }

        public static Item MapViewModelToItemEntity(Post viewModel)
        {
            Item result = new Item();

            result.Description = viewModel.Description;
            if (viewModel.Tags != null && viewModel.Tags.Count > 0)
            {
                result.Tags = new List<string>(viewModel.Tags);
            }
            return result;
        }

        public static Item MapViewModelToItemEntity(Put viewModel)
        {
            Item result = new Item();

            result.ID = Convert.ToInt32(viewModel.ID);
            result.Description = viewModel.Description;
            if (viewModel.Tags != null && viewModel.Tags.Count > 0)
            {
                result.Tags = new List<string>(viewModel.Tags);
            }
            return result;
        }
    }
}