using AutoMapper;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Webapi.ViewModels.ModuloContato;

namespace eAgenda.Webapi.Config.AutoMapperConfig
{
    public class ContatoProfile : Profile
    {
        public ContatoProfile()
        {
            ConverterEntidadeParaViewModel();
            ConverterViewModelParaEntidade();
        }

        private void ConverterViewModelParaEntidade()
        {
            CreateMap<InserirContatoViewModel, Contato>();
            CreateMap<EditarContatoViewModel, Contato>();
        }

        private void ConverterEntidadeParaViewModel()
        {
            CreateMap<Contato, ListarContatoViewModel>();
            CreateMap<Contato, VisualizarContatoViewModel>();

        }
    }
}