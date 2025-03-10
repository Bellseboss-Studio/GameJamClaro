﻿using System.Collections;
using System.Globalization;
using ServiceLocatorPath;
using StatesOfEnemies;
using TMPro;
using UnityEngine;

public class PjView : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private AnimationClip danioAnim, idle, muerte;
    [SerializeField] private GameObject personaje;
    private Personaje _personaje;
    private ConfiguracionDelPeronsaje _configuracionDelPeronsaje;
    private ControladorDeBatallaParaPersonajes _controladorDeBatallaParaPersonajes;

    public OnApplyDamage OnApplyDamageCustomNormal;
    public OnApplyDamage OnApplyDamageCustomEspecial;
    [SerializeField] private TextMeshProUGUI lifeText;

    public delegate void OnApplyDamage(string nameOfFrom, string nameOfTarger, float damage);
    public void Configurate(Personaje personajesJugablesElegido, ControladorDeBatallaParaPersonajes controladorDeBatallaParaPersonajes, bool isOwnCharacter)
    {
        Debug.Log($"Aqui debe de instanciar a {personajesJugablesElegido.nombre}");
        _controladorDeBatallaParaPersonajes = controladorDeBatallaParaPersonajes;
        var unoP = Resources.Load<GameObject>($"Prefab/{personajesJugablesElegido.nombre}");
        var pj = Instantiate(unoP, transform);
            anim = pj.GetComponent<Animator>();
        _configuracionDelPeronsaje = pj.GetComponent<ConfiguracionDelPeronsaje>();
        if (controladorDeBatallaParaPersonajes.DebeConfigurar)
        {
        }
        _personaje = personajesJugablesElegido;
        lifeText.text = _personaje.vida.ToString();
        pj.transform.rotation = controladorDeBatallaParaPersonajes.transform.rotation;
        if (isOwnCharacter) pj.GetComponent<BoxCollider>().enabled = false;
    }

    private void OnDropInTargetAtaqueEspecial(PjView target)
    {
        //Animar el propio personaje
        anim.Play(_configuracionDelPeronsaje.AtaqueEspecial.name);
        //SfxManager.Instance.PlaySound(_configuracionDelPeronsaje.AtaqueEspecial.name);
        Debug.Log($"El ataque especial para {target.PJ.nombre}");
        StartCoroutine(LuegoDeLaAnimacionRecibir(target, _configuracionDelPeronsaje.AtaqueEspecial));
    }

    private void OnDropInTargetInAtacaqueNormal(PjView target)
    {
        anim.Play(_configuracionDelPeronsaje.AtaqueNormal.name);
        //SfxManager.Instance.PlaySound(_configuracionDelPeronsaje.AtaqueEspecial.name);
        Debug.Log($"El ataque normal para {target.PJ.nombre}");
        StartCoroutine(LuegoDeLaAnimacionRecibir(target, _configuracionDelPeronsaje.AtaqueNormal));
    }

    IEnumerator LuegoDeLaAnimacionRecibir(PjView target, AnimationClip waitTime)
    {
        yield return new WaitForSeconds(waitTime.length);
        anim.Play(idle.name);
        target.AplicaDanoDe(_personaje.ataque);
        OnApplyDamageCustomNormal?.Invoke(_personaje.nombre, target.PJ.nombre, _personaje.ataque);
    }

    public void AplicaDanoDe(float danio)
    {
        anim.Play(danioAnim.name);
        //SfxManager.Instance.PlaySound(danioAnim.name);
        //probablemente esto defina cuando se muestra la pantalla de ganaste o perdiste
        Debug.Log($"Vida antes {_personaje.vida}");
        _personaje.vida -= danio;
        Debug.Log($"Vida Despues {_personaje.vida}");
        StartCoroutine(ComeBackIdle(danioAnim));
    }

    IEnumerator ComeBackIdle(AnimationClip concurrentAnimation)
    {
        yield return new WaitForSeconds(concurrentAnimation.length);
        if (_personaje.vida <= 0)
        {
            anim.Play(muerte.name);
            lifeText.text = "Muerto";
            if(_controladorDeBatallaParaPersonajes.GetPanelDePoderesController() != null) _controladorDeBatallaParaPersonajes.GetPanelDePoderesController().DesactivarPanelDePoderes();
            _configuracionDelPeronsaje.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            lifeText.text = _personaje.vida.ToString();
            anim.Play(idle.name);
        }
    }

    public Personaje PJ => _personaje;

    public bool EstaVivo()
    {
        return _personaje.vida > 0;
    }

    public void HacerAnimacionDeAtaque(string tipoDeDanio)
    {
        anim.Play(tipoDeDanio == "n" ? _configuracionDelPeronsaje.AtaqueNormal.name : _configuracionDelPeronsaje.AtaqueEspecial.name);
        StartCoroutine(ComeBackIdle(danioAnim));
    }

    public void DesConfigure(ControladorDeBatallaParaPersonajes controladorDeBatallaParaPersonajes)
    {
    }   
}