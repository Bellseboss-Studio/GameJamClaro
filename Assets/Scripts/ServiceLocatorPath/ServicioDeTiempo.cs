﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace ServiceLocatorPath
{
    public class ServicioDeTiempo : MonoBehaviour, IServicioDeTiempo
    {

        public const int EsTiempoDeColocacionHeroe = 1;
        public const int EsTiempoDePausa = 2;
        public const int EsTiempoDeJuego = 3;
        
        [SerializeField] private float tiempoDePausa;
        [SerializeField] private float tiempoDeJuego;
        [SerializeField] private float tiempoDeColocacionHeroe;
        [SerializeField] private Slider progresBarTiempo;
        [SerializeField] private Image pasuaIcono, playIcono;

        private bool _puedoContarElTiempo;
        private float _deltaTimeLocal = 0;
        private float _valorDeLaProgresBar;
        private int _tiempoQueEstoyContando;

        public int TiempoQueEstoyContando => _tiempoQueEstoyContando;

        public delegate void OnPausarPersonajes();
        
        public OnPausarPersonajes PausarPersonajes;
        public delegate void OnDespausarPersonajes();
        
        public OnDespausarPersonajes DespausarPersonajes;
        
        private void Awake()
        {
            
        }

        private void Update()
        {
            if (_puedoContarElTiempo)
            {
                _deltaTimeLocal += Time.deltaTime;
                DarValorALaProgressBar();
            }
        }

        private void DarValorALaProgressBar()
        {
            if (_tiempoQueEstoyContando == EsTiempoDeColocacionHeroe)
            {
                _valorDeLaProgresBar = _deltaTimeLocal / tiempoDeColocacionHeroe;
                progresBarTiempo.value = 1 - _valorDeLaProgresBar;
            }
            else
            {
                if (_tiempoQueEstoyContando == EsTiempoDePausa)
                {
                    _valorDeLaProgresBar = _deltaTimeLocal / tiempoDePausa;
                    progresBarTiempo.value = 1 - _valorDeLaProgresBar;
                }
                else
                {
                    _valorDeLaProgresBar = _deltaTimeLocal / tiempoDeJuego;
                    progresBarTiempo.value = 1 - _valorDeLaProgresBar;
                }
            }
        }

        public bool EstaPausadoElJuego()
        {
            if (_deltaTimeLocal <= tiempoDePausa)
            {
                return true;
            }
            DescongelarPersonajes();
            playIcono.enabled = true;
            pasuaIcono.enabled = false;
            return false;
        }

        public void DescongelarPersonajes()
        {
            DespausarPersonajes?.Invoke();
        }
        
        public void ComienzaAContarElTiempo(int queTiempoEstoyContando)
        {
            _puedoContarElTiempo = true;
            _tiempoQueEstoyContando = queTiempoEstoyContando;
        }

        public void DejaDeContarElTiempo()
        {
            _puedoContarElTiempo = false;
            _deltaTimeLocal = 0;
        }

        public bool EstanJugando()
        {
            if (_deltaTimeLocal <= tiempoDeJuego)
            {
                return true;
            }
            CongelarPersonajes();
            playIcono.enabled = false;
            pasuaIcono.enabled = true;
            return false;
        }

        public void CongelarPersonajes()
        {
            PausarPersonajes?.Invoke();
        }

        public bool SeEstaColocandoElHeroe()
        {
            return _deltaTimeLocal <= tiempoDeColocacionHeroe;
        }

        public void AniadirCantidadDeEnergiaAlSiguienteTurno(int entergiaASumar)
        {
            _deltaTimeLocal += entergiaASumar;
        }
    }
}