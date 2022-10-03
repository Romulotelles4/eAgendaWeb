using AutoMapper;
using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Webapi.ViewModels.ModuloContato;
using eAgenda.Webapi.ViewModels.ModuloTarefa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContatosController : eAgendaControllerBase
    {
        private readonly ServicoContato servicoContato;
        private readonly IMapper mapeadorContatos;

        public ContatosController(ServicoContato servicoContato, IMapper mapeadorContatos)
        {
            this.servicoContato = servicoContato;
            this.mapeadorContatos = mapeadorContatos;
        }

        [HttpGet]
        public ActionResult<List<ListarContatoViewModel>> SelecionarTodos()
        {
            var contatoResult = servicoContato.SelecionarTodos(UsuarioLogado.Id);

            if (contatoResult.IsFailed)
                return InternalError(contatoResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorContatos.Map<List<ListarContatoViewModel>>(contatoResult.Value)
            });
        }

        [HttpGet("vizualizar-completo/{id:guid}")]
        public ActionResult<VisualizarContatoViewModel> SelecionarContatoCompletoPorId(Guid id)
        {
            var contatoResult = servicoContato.SelecionarPorId(id);

            if (contatoResult.IsFailed && RegistroNaoEncontrado(contatoResult))
                return NotFound(contatoResult);

            if (contatoResult.IsFailed)
                return InternalError(contatoResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorContatos.Map<VisualizarContatoViewModel>(contatoResult.Value)
            });
        }

        [HttpPost]
        public ActionResult<FormsContatoViewModel> Inserir(InserirContatoViewModel contatoVM)
        {
            var contato = mapeadorContatos.Map<Contato>(contatoVM);

            contato.UsuarioId = UsuarioLogado.Id;

            var contatoResult = servicoContato.Inserir(contato);

            if (contatoResult.IsFailed)
                return InternalError(contatoResult);

            return Ok(new
            {
                sucesso = true,
                dados = contatoVM
            });
        }

        [HttpPut("{id:guid}")]
        public ActionResult<EditarContatoViewModel> Editar(Guid id, EditarContatoViewModel contatoVM)
        {
            var contatoSelecionadaResult = servicoContato.SelecionarPorId(id);

            if (contatoSelecionadaResult.IsFailed && RegistroNaoEncontrado(contatoSelecionadaResult))
                return NotFound(contatoSelecionadaResult);

            var contato = mapeadorContatos.Map(contatoVM, contatoSelecionadaResult.Value);

            var contatoResult = servicoContato.Editar(contato);

            if (contatoResult.IsFailed)
                return InternalError(contatoResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorContatos.Map<VisualizarContatoViewModel>(contatoResult.Value)
            });
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var contatoResult = servicoContato.Excluir(id);

            if (contatoResult.IsFailed && RegistroNaoEncontrado<Contato>(contatoResult))
                return NotFound(contatoResult);

            if (contatoResult.IsFailed)
                return InternalError<Contato>(contatoResult);

            return NoContent();
        }
    }
}
