﻿using System;
using Gameplay.Personajes.InteraccionComponents;
using Gameplay.Personajes.RutaComponents;
using Gameplay.Personajes.TargetComponents;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay
{
    public class PersonajeBuilder
    {
        private Personaje _personaje;
        private RutaBehaviour _rutaComponent;
        private TargetBehaviour _targetBehaviour;
        private InteraccionBehaviour _interaccionBehaviour;
        private Vector3 _position;
        private GameObject _prefab;
        private EstadisticasCarta _estadisticasCarta;
        private bool _renderizarUI;

        public PersonajeBuilder WithPersonaje(Personaje personaje)
        {
            _personaje = personaje;
            return this;
        }

        public PersonajeBuilder WithTargetComponent(TargetComponentEnum targetComponentId)
        {
            switch (targetComponentId)
            {
                case TargetComponentEnum.BuscarEnemigoMasCercano:
                    _targetBehaviour = new BuscarEnemigoMasCercano();
                    break;
                
                case TargetComponentEnum.BuscarTresEnemigosMasCercanos:
                    _targetBehaviour = new BuscartresEnemigosMasCercanos();
                    break;
                
                case TargetComponentEnum.BuscarEnemigoMasCercanoYNoActualizar:
                    _targetBehaviour = new BuscarEnemigoMasCercanoYNoActualizar();
                    break;
                
                default:
                    throw new Exception($"El componente con la id {targetComponentId} no existe");
            }
            return this;
        }

        public PersonajeBuilder WithInteraccionComponent(InteraccionComponentEnum interaccionBehaviourId)
        {
            switch (interaccionBehaviourId)
            {
                case InteraccionComponentEnum.DaniarPersonaje:
                    _interaccionBehaviour = new DaniarPersonaje();
                    break;

                case InteraccionComponentEnum.AtacarConDanioProgresivo:
                    _interaccionBehaviour = new AtacarConDanioProgresivo();
                    break;
                
                case InteraccionComponentEnum.DaniarPersonajeSegunLaDistancia:
                    _interaccionBehaviour = new DaniarPersonajeSegunLaDistancia();
                    break;
                
                case InteraccionComponentEnum.DaniarPersonajeYAutoCurarse:
                    _interaccionBehaviour = new DaniarPersonajeYAutoCurarse();
                    break;
                
                case InteraccionComponentEnum.DaniarPersonajeYReflejarDanio:
                    _interaccionBehaviour = new DaniarPersonajeYReflejarDanio();
                    break;

                case InteraccionComponentEnum.DaniarYAumentarVelocidadAlMatar:
                    _interaccionBehaviour = new DaniarYAumentarVelocidadAlMatar();
                    break;
                
                case InteraccionComponentEnum.AtacarYCurarAliadoCercanoMasDañado:
                    _interaccionBehaviour = new AtacarYCurarAliadoCercanoMasDañado();
                    break;
                
                case InteraccionComponentEnum.AtacarYReducirVelocidadDeMovimientoYAtaqueAlObjetivo:
                    _interaccionBehaviour = new AtacarYReducirVelocidadDeMovimientoYAtaqueAlObjetivo();
                    break;
                
                case InteraccionComponentEnum.AumentarVelocidadDeAtaqueYDeMovimientoDeAliadosCercanosAlMorir:
                    _interaccionBehaviour = new AumentarVelocidadDeAtaqueYDeMovimientoDeAliadosCercanosAlMorir();
                    break;
                
                case InteraccionComponentEnum.DaniarTresTargetsMasCercanos:
                    _interaccionBehaviour = new DaniarTresTargetsMasCercanos();
                    break;
                
                case InteraccionComponentEnum.ObtenerEnergiaAlIniciarTurno:
                    _interaccionBehaviour = new ObtenerEnergiaAlIniciarTurno();
                    break;
                
                case InteraccionComponentEnum.AumentarVelocidadGeneralDeAliadosAlIniciarTurno:
                    _interaccionBehaviour = new AumentarVelocidadGeneralDeAliadosAlIniciarTurno();
                    break;
                    
                case InteraccionComponentEnum.AtacarYCurarAliadoMasCercanoAlImpacto:
                    _interaccionBehaviour = new AtacarYCurarAliadoMasCercanoAlImpacto();
                    break;
                case InteraccionComponentEnum.InstanciarProyectilQueCuraYDania:
                    _interaccionBehaviour = new DispararProyectilQueCuraYDania();
                    break;
                case InteraccionComponentEnum.DaniarPersonajeYAutoDestruirse:
                    _interaccionBehaviour = new DaniarPersonajeCurarAliadoMasCercanoAndAutoDestruirse();
                    break;
                
                case InteraccionComponentEnum.DaniarPersonajeCurarAliadoCercanoMasDaniadoAndAutoDestruirse:
                    _interaccionBehaviour = new DaniarPersonajeCurarAliadoCercanoMasDaniadoAndAutoDestruirse();
                    break;
                
                case InteraccionComponentEnum.DispararProyectilQueDaniaYCuraAliadoMasCercano:
                    _interaccionBehaviour = new DispararProyectilQueDaniaYCuraAliadoMasCercano();
                    break;
                
                case InteraccionComponentEnum.DaniarPersonajeAndAutoDestruirse:
                    _interaccionBehaviour = new DaniarPersonajeAndAutoDestruirse();
                    break;
                
                case InteraccionComponentEnum.DispararProyectilesQueDanian:
                    _interaccionBehaviour = new DispararProyectilesQueDanian();
                    break;
                
                /*case InteraccionComponentEnum.:
                    _interaccionBehaviour = new ();
                    break;*/
                
                default:
                    throw new Exception($"El componente con la id {interaccionBehaviourId} no existe");
            }
            return this;
        }
        
        public PersonajeBuilder WithRutaComponent(RutaComponentEnum rutaComponentId)
        {
            switch (rutaComponentId)
            {
                case RutaComponentEnum.RutaMasCorta:
                    _rutaComponent = new RutaMasCorta();
                    break;
                case RutaComponentEnum.MovimientoBala:
                    _rutaComponent = new MovimientoBala();
                    break;
                default:
                    throw new Exception($"El componente con la id {rutaComponentId} no existe");
                break;
            }
            return this;
        }

        public PersonajeBuilder WithPosition(Vector3 position)
        {
            _position = position;
            //Debug.Log(position);
            //EditorApplication.isPaused = true;
            return this;
        }
        
        public PersonajeBuilder With3DObject(GameObject gameObject)
        {
            _prefab = gameObject;
            return this;
        }

        public PersonajeBuilder WithEstadisticasCarta(EstadisticasCarta estadisticasCarta)
        {
            _estadisticasCarta = estadisticasCarta;
            return this;
        }
        
        public Personaje Build()
        {
            var personaje = Object.Instantiate(_personaje);
            personaje.SetComponents(_targetBehaviour, _interaccionBehaviour, _rutaComponent, _estadisticasCarta, _prefab, _renderizarUI);
            personaje.transform.position = _position;
            
            return personaje;
        }

        public PersonajeBuilder WithUi(bool renderizarUI)
        {
            _renderizarUI = renderizarUI;
            return this;
        }
    }
}