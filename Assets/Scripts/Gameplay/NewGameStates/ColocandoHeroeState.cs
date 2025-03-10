﻿using System;
using System.Threading.Tasks;
using DG.Tweening;
using Gameplay.PersonajeStates;
using Gameplay.UsoDeCartas;
using ServiceLocatorPath;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace Gameplay.NewGameStates
{
    public class ColocandoHeroeState : IEstadoDeJuego
    {
        private IMediadorDeEstadosDelJuego _mediadorDeEstadosDelJuego;
        private readonly RectTransform _panelColocarHeroe;
        private readonly FactoriaCarta _factoriaCarta;

        public ColocandoHeroeState(IMediadorDeEstadosDelJuego mediadorDeEstadosDelJuego, RectTransform panelColocarHeroe, FactoriaCarta factoriaCarta)
        {
            _mediadorDeEstadosDelJuego = mediadorDeEstadosDelJuego;
            _panelColocarHeroe = panelColocarHeroe;
            _factoriaCarta = factoriaCarta;
        }

        public void InitialConfiguration()
        {
            ServiceLocator.Instance.GetService<IServicioDeTiempo>().ComienzaAContarElTiempo(1);
            _mediadorDeEstadosDelJuego.PedirColocacionDeHeroe();
            var sequence = DOTween.Sequence();
            sequence.Insert(0, _panelColocarHeroe.DOScale(1, .45f).SetEase(Ease.OutBack));
            _mediadorDeEstadosDelJuego.OcultarCartas();
        }

        public void FinishConfiguration()
        {
            _mediadorDeEstadosDelJuego.YaNoPedirColocacionDeHeroe();
            ServiceLocator.Instance.GetService<IServicioDeTiempo>().DejaDeContarElTiempo();
            _panelColocarHeroe.gameObject.SetActive(false);
            _mediadorDeEstadosDelJuego.CrearPrimerasCartas();
        }

        public async Task<PersonajeStateResult> DoAction(object data)
        {
            while (!_mediadorDeEstadosDelJuego.SeCololoaronLosHeroes())
            {
                if(!_mediadorDeEstadosDelJuego.SeColocoElHeroe()) _mediadorDeEstadosDelJuego.PedirColocacionDeHeroe();
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            //ServiceLocator.Instance.GetService<IEnemyInstantiate>().InstanciateHeroEnemy(_mediadorDeEstadosDelJuego.GetFactoryHero());
            return new PersonajeStateResult(ConfiguracionDeLosEstadosDelJuego.Pausa);
        }
    }
}