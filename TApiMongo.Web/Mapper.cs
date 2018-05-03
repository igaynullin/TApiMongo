using System;
using System.Collections.Generic;
using System.Text;
using TApiMongo.Interfaces;

namespace TApiMongo.Web
{
    public class Mapper<TEntity, TViewModel> : IHasToEntity<TEntity, TViewModel>, IHasToViewModel<TViewModel, TEntity>, IMapper<TEntity, TViewModel>
    {
        private Func<TEntity, TViewModel> _entityToViewModel;
        private Func<TViewModel, TEntity> _viewModelToEntity;

        public Mapper(Func<TEntity, TViewModel> entityToViewModel, Func<TViewModel, TEntity> viewModelToEntity)
        {
            _entityToViewModel = entityToViewModel;
            _viewModelToEntity = viewModelToEntity;
        }

        public TEntity MapEntity(TViewModel viewModel)
        {
            if (_viewModelToEntity == null)
                throw new NullReferenceException(nameof(_viewModelToEntity));
            return _viewModelToEntity(viewModel);
        }

        public TViewModel MapViewModel(TEntity entity)
        {
            if (_entityToViewModel == null)
                throw new NullReferenceException(nameof(_entityToViewModel));
            return _entityToViewModel(entity);
        }
    }
}